using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using NoNicotine_Business.Commands;
using NoNicotine_Business.Queries;
using NoNicotine_Business.Services;
using System.Security.Claims;

namespace NoNicotineAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        public PatientsController(IMediator mediator, IAuthenticationService authenticationService)
        {
            _mediator = mediator;
            _authenticationService = authenticationService;
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
        [Authorize(Roles = "patient")]
        [Route("GetPatient")]
        public async Task<IActionResult> GetPatient()
        {

            if (HttpContext.User.Identity is not ClaimsIdentity identity)
            {
                return Unauthorized();
            }
            var patientUserId = _authenticationService.GetUserIdFromClaims(identity);

            var request = new GetPatientQuery()
            {
                UserId = patientUserId
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

            var patientUserId = _authenticationService.GetUserIdFromClaims(identity);
            if(patientUserId == "")
            {
                return BadRequest("Something went wrong");
            }

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
