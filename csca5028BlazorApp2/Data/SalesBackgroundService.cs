using csca5028.final.project.components;
using Microsoft.Extensions.Caching.Distributed;

namespace csca5028BlazorApp2.Data
{
    public class SalesBackgroundService : BackgroundService, IDisposable
    {
        private readonly ILogger<SalesBackgroundService> _logger;
        private readonly ISalesAnalyzer _salesAnalyser;
        //private readonly RedisCachingService _cache;
        private readonly MemoryCachingService _cache;
        private readonly string _dbConnectionString = string.Empty;

        private readonly int _intervalInMinutes = 1;
        private Timer _timer;

        public SalesBackgroundService(ILogger<SalesBackgroundService> logger, MemoryCachingService cache, IConfiguration config)
        {
            _logger = logger;
            _dbConnectionString = config["ConnectionStrings:AzureDB"];
            _salesAnalyser = new SalesAnalyzer("sales_db", _dbConnectionString);
            _cache = cache;
        }

        /*public SalesBackgroundService(ILogger<SalesBackgroundService> logger, RedisCachingService cache, IConfiguration config)
        {
            _logger = logger;
            _dbConnectionString = config["ConnectionStrings:AzureDB"];
            _salesAnalyser = new SalesAnalyzer("sales_db", _dbConnectionString);
            _cache = cache;
        }*/

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("SalesBackgroundService is starting.");
            if (!cancellationToken.IsCancellationRequested)
            {
                CreateCache(cancellationToken);
                _timer = new Timer(OnRefreshCache, cancellationToken, 0, _intervalInMinutes * 60000);
            }

            return Task.CompletedTask;
        }

        private async void OnRefreshCache(object state)
        {
            _logger.LogInformation("SalesBackgroundService is working.");
            CancellationToken token = (CancellationToken)state;
            await CreateCache(token);
        }

        private async Task CreateCache(CancellationToken token)
        {
            if (!token.IsCancellationRequested)
            {
                await _cache.SetAsync("SalesPerformance", await _salesAnalyser.LoadSalesPerformance());
                await _cache.SetAsync("StoreLocations", await _salesAnalyser.GetStoreLocations());
                await _cache.SetAsync("StoresAndTerminals", await _salesAnalyser.GetStoresAndTerminals());
                await _cache.SetAsync("TotalSalesRevenue", await _salesAnalyser.GetTotalSalesRevenue());
                await _cache.SetAsync("TotalRevenueForTimeInterval", await _salesAnalyser.GetTotalRevenueForTimeInterval());
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("SalesBackgroundService is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            await base.StopAsync(cancellationToken);
        }

        public override void Dispose()
        {
            _timer?.Dispose();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.CompletedTask;
        }
    }
}