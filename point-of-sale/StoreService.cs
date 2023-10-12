using csca5028.final.project.components;
using Microsoft.Azure.Amqp.Framing;
using Prometheus;
using System.Diagnostics;

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
        private Dictionary<string, string> storeNameByTerminalID = new();
        private Dictionary<string, Tuple<Timer,Stopwatch>> checkOutTimers = new();

        //some instrumentation
        Gauge storesOnline = Metrics.CreateGauge("point_of_sale_app_stores_online", "Number of stores running point-of-sale");
        Gauge posOnline = Metrics.CreateGauge("point_of_sale_app_pos_online", "Number of point-of-sale terminals running");
        Summary checkoutTimeSummary = Metrics.CreateSummary("point_of_sale_app_checkout_time", "Checkout Time (ms) Summary", new SummaryConfiguration
        {
            LabelNames = new[] { "store", "terminal" },
            SuppressInitialValue = true,
            Objectives = new[]
            {
                                new QuantileEpsilonPair(0.5, 0.05),
                                new QuantileEpsilonPair(0.9, 0.05),
                                new QuantileEpsilonPair(0.95, 0.01),
                                new QuantileEpsilonPair(0.99, 0.005),
                            },
        });
        
        private readonly int iTimeScaler = 60000; //default to minutes

        public StoreService(ILogger<StoreService> logger, IPOSTerminalTaskQueue taskQueue, IConfiguration config)
        {
            _logger = logger;
            _taskQueue = taskQueue;
            azureDbConnectionstring = config["ConnectionStrings:AzureDB"];
            azureServiceBusConnectionString = config["ServiceBusConnectionString"];
            if(!int.TryParse(Environment.GetEnvironmentVariable("TimeScaler"), out iTimeScaler))
            {
                iTimeScaler = 60000;
            }

            int terminalCount = 0;
            try
            {
                storeAndTerminals = storeDb.GetStoresAndTerminalsAsync(dbName, azureDbConnectionstring).Result;   //start queueing up the checkout timers

                storesOnline.Set(storeAndTerminals.Count);

                foreach (string storeName in storeAndTerminals.Keys)
                {
                    List<POSTerminal> terminals = storeAndTerminals[storeName].Item2;
                    
                    foreach (POSTerminal terminal in terminals)
                    {
                        storeNameByTerminalID.Add(terminal.posID.ToString(), storeName);
                        _taskQueue.EnqueueTask(terminal, azureServiceBusConnectionString); //1st Task to be queued and timer to be started
                        Timer checkoutIntervalTimer = new Timer(OnCheckOutIntervalExpired, terminal, 0, terminal.checkoutTime * iTimeScaler);
                        Stopwatch checkoutIntervalStopWatch = new Stopwatch();
                        checkoutIntervalStopWatch.Start();

                        checkOutTimers.Add($"{storeName}:{terminal.posID}", new Tuple<Timer, Stopwatch>(checkoutIntervalTimer, checkoutIntervalStopWatch));
                    }
                    terminalCount += terminals.Count;
                }
                posOnline.Set(terminalCount);
            } catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating StoreService.");
            }
        }

        private async void OnCheckOutIntervalExpired(object state)
        {
            POSTerminal terminal = (POSTerminal)state;
            try
            {
                await Checkout(terminal);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in StoreService Timer Callback.");
            }
        }

        private async Task Checkout(POSTerminal terminal)
        {

            string storeName = storeNameByTerminalID[terminal.posID.ToString()];
            Tuple<Timer, Stopwatch> timerAndStopwatch = checkOutTimers[$"{storeName}:{terminal.posID}"];

            //we need to complete checkout for the terminal
            var terminalTask = await _taskQueue.DequeueTask();
            if (terminalTask != null)
            {
                try
                {
                    await terminalTask;
                    var checkOutTime = timerAndStopwatch.Item2.ElapsedMilliseconds;
                    timerAndStopwatch.Item2.Restart();
                    //put next in line

                    checkoutTimeSummary.WithLabels(storeName, terminal.posID.ToString()).Observe(checkOutTime);

                    _taskQueue.EnqueueTask(terminal, azureServiceBusConnectionString);

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred executing {TerminalTask}.", nameof(terminalTask));
                }
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.CompletedTask;
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("TerminalService is stopping.");
            if(checkOutTimers.Count > 0)
            {
                foreach (string storeAndTerminal in checkOutTimers.Keys)
                {
                    Tuple<Timer, Stopwatch> timerAndStopwatch = checkOutTimers[storeAndTerminal];
                    timerAndStopwatch.Item1.Dispose();
                    timerAndStopwatch.Item2.Stop();
                }
            }
            posOnline.Set(0);
            storesOnline.Set(0);
            await base.StopAsync(stoppingToken);
        }
    }
}
