using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ShiftsLoggerApi.Controllers
{
    [ApiController]
    [Route("/error")]
    public class ErrorController : ControllerBase
    {
        private readonly ILogger<ErrorController> logger;

        public ErrorController(ILogger<ErrorController> loggerAssign)
        {
            logger = loggerAssign;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Error(IExceptionHandlerPathFeature exception)
        {
            logger.LogError(exception.Error, "Unhandled exception occurred");

            return StatusCode(500, "Something went wrong");
        }
    }
}