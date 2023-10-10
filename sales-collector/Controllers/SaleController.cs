using csca5028.final.project.components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Prometheus;
using System.Diagnostics;

namespace sales_collector.Controllers
{
    [Route("api/insertsale")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        private readonly ILogger<SaleController> _logger;
        private string azureDBConnectionString = string.Empty;

        public SaleController(ILogger<SaleController> logger, IConfiguration config)
        {
            _logger = logger;
            azureDBConnectionString = config["ConnectionStrings:AzureDB"];
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] string sale)
        {
            try
            {
                if(string.IsNullOrEmpty(sale))
                {
                    return BadRequest();
                }

                var summary = Metrics.CreateSummary("sales_collector_db_insert", "Summary for sales_collector Database insert", new SummaryConfiguration
                {
                    LabelNames = new[] { "method", "endpoint" },
                    SuppressInitialValue = true,
                    Objectives = new[]
                            {
                                        new QuantileEpsilonPair(0.5, 0.05),
                                        new QuantileEpsilonPair(0.9, 0.05),
                                        new QuantileEpsilonPair(0.95, 0.01),
                                        new QuantileEpsilonPair(0.99, 0.005),
                                    },
                });

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                //deserialize the message
                _logger.LogInformation("Message Received...{sale}",sale);
                
                var oSale = Newtonsoft.Json.JsonConvert.DeserializeObject<Sale>(sale);

                if (oSale == null)
                {
                    return BadRequest();
                }

                //insert the sale into the database
                _logger.LogInformation("Inserting Sale into Database...");
                SalesDBController salesDb = new();
                await salesDb.Insert("sales_db", oSale, azureDBConnectionString);

                stopwatch.Stop();
                summary.WithLabels("POST", "api/insertsale").Observe(stopwatch.ElapsedMilliseconds);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in SaleController.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok();
        }
    }
}
