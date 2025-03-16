using BusinessLogicLayer;
using Microsoft.AspNetCore.Mvc;

namespace AdvertisingByDomain.Controllers
{
    [Route("Advertising")]
    [ApiController]
    public class AdvertisingLocationController : ControllerBase
    {
        private readonly ILogger<AdvertisingLocationController> logger;
        private readonly DomainServices domainServices;

        public AdvertisingLocationController(ILogger<AdvertisingLocationController> logger, DomainServices domainServices)
        {
            this.logger = logger;
            this.domainServices = domainServices;
        }

        /// <summary>
        /// Метод загрузки файла с рекламными площадками
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPut("UploadFile")]
        public IActionResult UploadFile(IFormFile file)
        {
            // размер файла равен нулю
            if (file.Length == 0) { return BadRequest(); }
            logger.Log(LogLevel.Information, $" > UploadFile: {file.Length} byte");

            // файл отличен от txt формата
            if (file.ContentType != "text/plain") { return BadRequest(); }

            bool res = domainServices.ParseAndUpload(file.OpenReadStream());

            if (res) { return Ok(); }       // если всё хорошо и данные добавлены
            else { return BadRequest(); }   // если возникла какая-то ошибка
        }


        /// <summary>
        /// Метод получения рекламных площадок по локации
        /// </summary>
        /// <param name="Location"></param>
        /// <returns></returns>
        [HttpGet("Get")]
        public ActionResult<List<string>> GetPlatformByLocation([FromQuery] string Location)
        {
            if (!string.IsNullOrWhiteSpace(Location))
            {
                var res = domainServices.ParseLocation(Location);

                if (res != null)
                {
                    if (res.Count > 0)
                    {
                        return Ok(res);
                    }
                }
            }

            logger.Log(LogLevel.Warning, "Not a single occurrence found!");
            return NotFound();
        }

    }
}
