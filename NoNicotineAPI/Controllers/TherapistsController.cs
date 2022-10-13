using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoNicotine_Business.Commands;
using NoNicotine_Business.Queries;
using NoNicotine_Business.Services;
using System.Security.Claims;

namespace NoNicotineAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TherapistsController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        public TherapistsController(IMediator mediator, IAuthenticationService authenticationService)
        {
            _mediator = mediator;
            _authenticationService = authenticationService;
        }
        private readonly IMediator _mediator;

        [HttpPost]
        public async Task<IActionResult> CreateTherapist(CreateTherapistCommand request)
        {
            var result = await _mediator.Send(request);
            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpGet]
        [Authorize(Roles = "therapist")]
        public async Task<IActionResult> GetTherapist()
        {
            if (HttpContext.User.Identity is not ClaimsIdentity identity)
            {
                return Unauthorized();
            }

            var therapistUserId = _authenticationService.GetUserIdFromClaims(identity);

            var request = new GetTherapistQuery()
            {
                UserId = therapistUserId
            };

            var result = await _mediator.Send(request);
            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPut]
        [Authorize(Roles = "therapist")]
        public async Task<IActionResult> UpdateTherapist(UpdateTherapistCommand request)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity == null)
            {
                return Unauthorized();
            }

            var therapistUserId = _authenticationService.GetUserIdFromClaims(identity);

            request.Id = therapistUserId;
            var result = await _mediator.Send(request);
            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }
}
