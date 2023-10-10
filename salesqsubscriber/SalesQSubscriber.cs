using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using csca5028.final.project;
using Microsoft.Extensions.Configuration;

namespace salesqsubscriber
{
    public class SalesQSubscriber
    {
        private readonly ILogger _logger;
        private readonly string serviceURI = "https://csca5028api.azure-api.net/sca/";

        public SalesQSubscriber(ILoggerFactory loggerFactory, IConfiguration config)
        {
            _logger = loggerFactory.CreateLogger<SalesQSubscriber>();
            serviceURI = config["scaServiceURI"];
        }

        [Function("InsertSale")]
        public void Run([ServiceBusTrigger("salesq", Connection = "ServiceBusConnectionString")] string myQueueItem)
        {
            try
            {
                SalesProcessor salesProcessor = new SalesProcessor(serviceURI, new System.Net.Http.HttpClient());
                salesProcessor.InsertsaleAsync(myQueueItem).Wait();
                _logger.LogInformation($"C# ServiceBus queue trigger function processed message");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            
        }
    }
}
