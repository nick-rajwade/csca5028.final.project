using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace credit_card_processor.Controllers
{
    [Route("api/ping")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<string> Get()
        {
            return Ok("pong");
        }
    }
}
