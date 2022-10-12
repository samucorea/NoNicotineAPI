using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoNicotine_Business.Commands;
using NoNicotine_Business.Queries;
using System.Security.Claims;

namespace NoNicotineAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TherapistsController : Controller
    {
        public TherapistsController(IMediator mediator)
        {
            _mediator = mediator;
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
        [Route("{id}")]
        public async Task<IActionResult> GetTherapist(string id)
        {
            var request = new GetTherapistQuery()
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
        [Authorize(Roles = "therapist")]
        public async Task<IActionResult> UpdateTherapist(UpdateTherapistCommand request)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity == null)
            {
                return Unauthorized();
            }

            var therapistUserIdClaim = identity.FindFirst("UserId");
            if (therapistUserIdClaim == null)
            {
                return BadRequest("Something went wrong");
            }
            var therapistUserId = therapistUserIdClaim.Value;

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
