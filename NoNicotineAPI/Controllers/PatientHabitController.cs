using MediatR;
using Microsoft.AspNetCore.Mvc;
using NoNicotin_Business.Commands;

namespace NoNicotineAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientHabitController : Controller
    {
        public PatientHabitController(IMediator mediator)
        {
            _mediator = mediator;
        }
        private readonly IMediator _mediator;

        [HttpPost]
        public async Task<IActionResult> CreatePatientHabit(CreatePatientHabitCommand request)
        {

            var result = await _mediator.Send(request);
            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }
}
