using Microsoft.AspNetCore.Mvc;

namespace sales_collector.Controllers
{
    [ApiController]
    [Route("api/ping")]
    public class HealthController : ControllerBase
    {
        private readonly ILogger<HealthController> _logger;

        public HealthController(ILogger<HealthController> logger)
        {
            _logger = logger;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<string> Get()
        {
            return Ok("pong");
        }
    }
}
