using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace salesqsubscriber
{
    public class SalesQSubscriber
    {
        private readonly ILogger _logger;

        public SalesQSubscriber(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<SalesQSubscriber>();
        }

        [Function("InsertSale")]
        public void Run([ServiceBusTrigger("salesq", Connection = "ServiceBusConnectionString")] string myQueueItem)
        {
            
            
            _logger.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
        }
    }
}
