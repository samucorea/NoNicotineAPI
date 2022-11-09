using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NoNicotineAPI.Models;

namespace NoNicotineAPI.Controllers
{
    [AllowAnonymous]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorsController : ControllerBase
    {
        private readonly ILogger<ErrorsController> _logger;

        public ErrorsController(ILogger<ErrorsController> logger)
        {
            _logger = logger;
        }

        [Route("error")]
        public ErrorResponse Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = context!.Error;
            var code = 500;

            _logger.LogError("Error in {endpoint}: {errMessage}", context.Endpoint!.DisplayName, exception.Message);

            Response.StatusCode = code; 

            return new ErrorResponse()
            {
                Message = "Something went wrong"
            }; 
        }
    }
}
