using MediatR;
using Microsoft.AspNetCore.Mvc;
using NoNicotin_Business.Commands;
using NoNicotin_Business.Handler;
using NoNicotin_Business.Queries;

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

        [HttpGet]
        public async Task<IActionResult> GetPatientHabit(GetPatientHabitQuery request)
        {

            var result = await _mediator.Send(request);
            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

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
