using csca5028.final.project.components;
using Prometheus;

namespace point_of_sale
{
    public class StoreService : BackgroundService, IDisposable
    {
        private readonly ILogger<StoreService> _logger;
        private readonly IPOSTerminalTaskQueue _taskQueue;

        private string azureDbConnectionstring = string.Empty;
        private string azureServiceBusConnectionString = string.Empty;

        private readonly string dbName = "sales_db";

        private StoreDBController storeDb = new();
        private Dictionary<string,Tuple<Store,List<POSTerminal>>> storeAndTerminals = new();
        private List<System.Threading.Timer> checkOutTimers = new();

        Gauge storesOnline = Metrics.CreateGauge("point_of_sale_app_stores_online", "Number of stores running point-of-sale");
        Gauge posOnline = Metrics.CreateGauge("point_of_sale_app_pos_online", "Number of point-of-sale terminals running");

        public StoreService(ILogger<StoreService> logger, IPOSTerminalTaskQueue taskQueue, IConfiguration config)
        {
            _logger = logger;
            _taskQueue = taskQueue;
            azureDbConnectionstring = config["ConnectionStrings:AzureDB"];
            azureServiceBusConnectionString = config["ServiceBusConnectionString"];

            try
            {
                storeAndTerminals = storeDb.GetStoresAndTerminalsAsync(dbName, azureDbConnectionstring).Result;   //start queueing up the checkout timers

                storesOnline.Set(storeAndTerminals.Count);

                foreach (string storeName in storeAndTerminals.Keys)
                {
                    List<POSTerminal> terminals = storeAndTerminals[storeName].Item2;
                    foreach (POSTerminal terminal in terminals)
                    {
                        _taskQueue.EnqueueTask(terminal, azureServiceBusConnectionString); //1st Task to be queued and timer to be started
                        Timer checkoutIntervalTimer = new Timer(OnCheckOutIntervalExpired, terminal, 0, terminal.checkoutTime * 60* 1000);
                        checkOutTimers.Add(checkoutIntervalTimer);
                    }
                    posOnline.Set(storeAndTerminals.Count);
                }
            } catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating StoreService.");
            }
        }

        private void OnCheckOutIntervalExpired(object state)
        {
            try { 
            POSTerminal terminal = (POSTerminal)state;
            _taskQueue.EnqueueTask(terminal, azureServiceBusConnectionString);
            } catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in StoreService Timer Callback.");
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            { 
                _logger.LogInformation("TerminalService running at: {time}", DateTimeOffset.Now);
                var terminalTask = await _taskQueue.DequeueTask();
                try
                {
                    await terminalTask;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred executing {TerminalTask}.", nameof(terminalTask));
                }
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("TerminalService is stopping.");
            foreach (System.Threading.Timer timer in checkOutTimers)
            {
                timer.Dispose();
            }
            posOnline.Set(0);
            storesOnline.Set(0);
            await base.StopAsync(stoppingToken);
        }
    }
}
