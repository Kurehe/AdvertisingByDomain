using Microsoft.AspNetCore.Mvc;

namespace AdvertisingByDomain.Controllers
{
    [Route("Advertising")]
    [ApiController]
    public class AdvertisingLocationController : ControllerBase
    {
        private readonly ILogger<AdvertisingLocationController> logger;

        public AdvertisingLocationController(ILogger<AdvertisingLocationController> logger)
        {
            this.logger = logger;
        }

        [HttpPut("UploadFile")]
        public async Task<IActionResult> UploadFile()
        {
            await Task.Delay(10);
            logger.Log(LogLevel.Information, " > UploadFile");
            return Ok();
        }

        [HttpGet("Get")]
        public async Task<IActionResult> GetPlatformByLocation([FromQuery] string[] ArrayLocation)
        {
            await Task.Delay(10);
            logger.Log(LogLevel.Information, " > GetPlatformByLocation");
            string mes = "";
            foreach (var item in ArrayLocation)
            {
                mes += $"\t- >  {item}\n";
            }
            logger.Log(LogLevel.Information, mes);
            return Ok();
        }
    }
}
