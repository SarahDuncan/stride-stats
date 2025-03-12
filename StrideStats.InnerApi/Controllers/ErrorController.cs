using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace StrideStats.InnerApi.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ErrorController : ControllerBase
    {
        [HttpGet]
        public IActionResult ErrorInProduction()
        {
            return Problem();
        }

        [Route("development-error")]
        public IActionResult ErrorInDevelopment([FromServices] IHostEnvironment hostEnvironment)
        {
            if (!hostEnvironment.IsDevelopment())
            {
                return NotFound();
            }

            var exceptionHandlerFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();
            return Problem(detail: exceptionHandlerFeature?.Error.InnerException?.StackTrace, title: exceptionHandlerFeature?.Error.Message);
        }
    }
}
