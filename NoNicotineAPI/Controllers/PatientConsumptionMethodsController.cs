using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoNicotine_Business.Commands;
using NoNicotine_Business.Queries;
using System.Data;
using System.Security.Claims;

namespace NoNicotineAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "patient")]
    public class PatientConsumptionMethodsController : Controller
    {

        public PatientConsumptionMethodsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        private readonly IMediator _mediator;


        [HttpGet]
        [Route("{patientConsumptionMethodsId}")]
        public async Task<IActionResult> GetPatientConsumptionMethod(string patientConsumptionMethodsId)
        {

            var result = await _mediator.Send(new GetPatientConsumptionMethodQuery() { PatientConsumtionId = patientConsumptionMethodsId});
            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }
}

