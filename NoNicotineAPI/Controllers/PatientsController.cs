using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using NoNicotine_Business.Commands;
using NoNicotine_Business.Queries;
using System.Security.Claims;

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

            var result = await _mediator.Send(request);
            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetPatient(string id)
        {
           
            var request = new GetPatientQuery()
            {
                Id = id
            };

            var result = await _mediator.Send(request);
            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }


        [HttpPut]
        [Authorize(Roles = "patient")]
        public async Task<IActionResult> UpdatePatient(UpdatePatientCommand request)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity == null)
            {
                return Unauthorized();
            }

            var patientUserIdClaim = identity.FindFirst("UserId");
            if (patientUserIdClaim == null)
            {
                return BadRequest("Something went wrong");
            }

            var patientUserId = patientUserIdClaim.Value;

            request.Id = patientUserId;
            var result = await _mediator.Send(request);
            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }
}
