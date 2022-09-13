using MediatR;
using Microsoft.AspNetCore.Mvc;
using NoNicotin_Business.Commands;

namespace NoNicotineAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : Controller
    {

        public PatientsController(IMediator mediator)
        {
            _mediator = mediator; 
        } 
        private readonly IMediator _mediator;

        [HttpPost]
        public async Task<IActionResult> CreatePatient(CreatePatientCommand request)
        {
            try
            {
                var result = await _mediator.Send(request);
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
