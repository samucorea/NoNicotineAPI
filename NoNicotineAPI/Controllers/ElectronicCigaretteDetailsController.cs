using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoNicotine_Business.Commands;
using NoNicotine_Business.Queries;
using System.Data;

namespace NoNicotineAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "patient")]
    public class ElectronicElectronicCigaretteDetailsController : Controller
    {

        public ElectronicElectronicCigaretteDetailsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        private readonly IMediator _mediator;

        [HttpPost]
        public async Task<IActionResult> CreateElectronicCigaretteDetail(CreateElectronicCigaretteDetailsCommand request)
        {
            var result = await _mediator.Send(request);
            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpGet]
        [Route("{patientConsumptionId}")]
        public async Task<IActionResult> GetElectronicCigaretteDetail(string patientConsumptionId)
        {

            var result = await _mediator.Send(new GetElectronicCigaretteDetailQuery() { PatientConsumptionId = patientConsumptionId });
            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }


        [HttpPut]
        public async Task<IActionResult> UpdateElectronicCigaretteDetail(UpdateElectronicCigaretteDetailsCommand request)
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
