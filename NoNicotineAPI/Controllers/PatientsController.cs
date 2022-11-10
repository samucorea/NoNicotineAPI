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
    [Authorize(Roles = "patient")]
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
        [AllowAnonymous]
        public async Task<IActionResult> CreatePatient(CreatePatientCommand request)
        {

            var result = await _mediator.Send(request);
            if (result.Succeeded)
            {
                return Ok(result.Data);
            }

            return BadRequest(result);
        }

        [HttpGet]
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
            if (result.Succeeded && result.Data != null)
            {
                return Ok(result.Data);
            }

            return BadRequest(result);
        }

        [HttpGet]
        [Route("consumptionExpenses/{patientConsumptionMethodsId}")]
        public async Task<IActionResult> GetDailyConsumptionExpenses(string patientConsumptionMethodsId)
        {

            var request = new GetDailyConsumptionExpensesQuery()
            {
                PatientConsumptionMethodsId = patientConsumptionMethodsId
            };

            var result = await _mediator.Send(request);
            if (!result.Succeeded)
            {
                return BadRequest();
            }

            return Ok(result.Data);
        }
        
        [HttpPut]
        [Route("indicateRelapse")]
        public async Task<IActionResult> IndicateRelapse(IndicateRelapseCommand request)
        {

            if (HttpContext.User.Identity is not ClaimsIdentity identity)
            {
                return Unauthorized();
            }
            var patientUserId = _authenticationService.GetUserIdFromClaims(identity);

            request.UserId = patientUserId;

            var result = await _mediator.Send(request);
            if (result.Succeeded && result.Data != null)
            {
                return Ok(result.Data);
            }

            return BadRequest(result);
        }
        
        [HttpPut]
        public async Task<IActionResult> UpdatePatient(UpdatePatientCommand request)
        {
            ClaimsIdentity? identity = HttpContext.User.Identity as ClaimsIdentity;

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
            if (result.Succeeded && result.Data != null)
            {
                return Ok(result.Data);
            }

            return BadRequest(result);
        }

        [HttpPost]
        [Route("Habits")]
        public async Task<IActionResult> CreatePatientHabit(CreatePatientHabitCommand request)
        {

            if (HttpContext.User.Identity is not ClaimsIdentity identity)
            {
                return Unauthorized();
            }
            var patientUserId = _authenticationService.GetUserIdFromClaims(identity);

            request.UserId = patientUserId;

            var result = await _mediator.Send(request);
            if (result.Succeeded && result.Data != null)
            {
                return Ok(result.Data);
            }

            return BadRequest(result);
        }

        [HttpPut]
        [Route("Habits")]
        public async Task<IActionResult> UpdatePatientHabit(UpdatePatientHabitCommand request)
        {

            if (HttpContext.User.Identity is not ClaimsIdentity identity)
            {
                return Unauthorized();
            }
            var patientUserId = _authenticationService.GetUserIdFromClaims(identity);

            request.UserId = patientUserId;

            var result = await _mediator.Send(request);
            if (result.Succeeded && result.Data != null)
            {
                return Ok(result.Data);
            }

            return BadRequest(result);
        }

        [HttpGet]
        [Route("Habits")]
        public async Task<IActionResult> GetAllPatientHabits()
        {

            if (HttpContext.User.Identity is not ClaimsIdentity identity)
            {
                return Unauthorized();
            }
            var patientUserId = _authenticationService.GetUserIdFromClaims(identity);

            var request = new GetAllPatientHabitsQuery()
            {
                UserId = patientUserId
            };

            var result = await _mediator.Send(request);
            if (result.Succeeded && result.Data != null)
            {
                return Ok(result.Data);
            }

            return BadRequest(result);
        }


    }
}
