using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoNicotine_Business.Commands.Create;
using NoNicotine_Business.Commands.Update;
using NoNicotine_Business.Queries;
using System.Data;
using System.Security.Claims;

namespace NoNicotineAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class LinkController : Controller
    {
        public LinkController(IMediator mediator)
        {
            _mediator = mediator;
        }
        private readonly IMediator _mediator;

        [HttpPut]
        [Authorize(Roles = "patient")]
        [Route("Request")]
        public async Task<IActionResult> AcceptDenyLinkRequest(UpdateAcceptDenyLinkrequestCommand request)
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }

            request.UserId = userId;

            var result = await _mediator.Send(request);
            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPut]
        [Authorize(Roles = "patient,therapist")]
        public async Task<IActionResult> UnrelatePatientLink(UpdateUnrelatePatientTherapistCommand request)
        {
            var userId = User.FindFirst("UserId")?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            if (userId == null || role == null)
            {
                return Unauthorized();
            }

            request.UserId = userId;
            request.Role = role;

            var result = await _mediator.Send(request);
            if (result.Succeeded)
            {
                return Ok(result.Data);
            }

            return BadRequest(result);
        }

        [HttpPost]
        [Route("Request")]
        [Authorize(Roles = "therapist")]
        public async Task<IActionResult> CreateLinkRequest(CreateLinkRequestCommand request)
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }

            request.TherapistUserId = userId;

            var result = await _mediator.Send(request);
            if (result.Succeeded)
            {
                return Ok(result.Data);
            }

            return BadRequest(result);
        }

        [HttpGet]
        [Route("Request")]
        [Authorize(Roles = "patient")]
        public async Task<IActionResult> GetMostRecentLinkRequest()
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }

            var request = new GetMostRecentLinkRequestQuery();

            request.UserId = userId;

            var result = await _mediator.Send(request);
            if (result.Succeeded)
            {
                return Ok(result.Data);
            }

            return BadRequest(result);
        }
    }
}
