using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoNicotine_Business.Commands;
using NoNicotine_Business.Services;
using NoNicotine_Data.Entities;
using NoNicotineAPI.Models;
using System.Security.Claims;

namespace NoNicotineAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "patient")]
    public class EntriesController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IMediator _mediator;
        public EntriesController(IMediator mediator, IAuthenticationService authenticationService)
        {
            _mediator = mediator;
            _authenticationService = authenticationService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateEntry(CreateEntryCommand request)
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
                var entry = result.Data;
                var entryResponse = new EntryResponse
                {
                    Id = entry.ID,
                    PatientId = entry.PatientId,
                    Message = entry.Message,
                    TherapistAllowed = entry.TherapistAllowed,
                    Feelings = MapStringToList(entry.Feelings),
                    Symptoms = MapStringToList(entry.Symptoms)
                };
                return Ok(entryResponse);
            }

            return BadRequest(result);
        }

        private static List<string> MapStringToList(string element)
        {

            return element.Trim().Split(',').ToList();
        }

    }
}
