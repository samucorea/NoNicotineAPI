using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoNicotine_Business.Commands.Create;
using NoNicotine_Business.Commands.Delete;
using NoNicotine_Business.Commands.Update;
using NoNicotine_Business.Queries;
using System.Data;

namespace NoNicotineAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "patient")]
    public class ElectronicElectronicCigarreteDetailsController : Controller
    {

        public ElectronicElectronicCigarreteDetailsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        private readonly IMediator _mediator;

        [HttpPost]
        public async Task<IActionResult> CreateElectronicCigarreteDetail(CreateElectronicCigarreteDetailsCommand request)
        {
            var user = User.Identity.Name;
            var result = await _mediator.Send(request);
            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpGet]
        [Route("{patientConsumptionMethodsId}")]
        public async Task<IActionResult> GetElectronicCigarreteDetail(string patientConsumptionMethodsId)
        {

            var result = await _mediator.Send(new GetElectronicCigarreteDetailQuery(){PatientConsumtionId = patientConsumptionMethodsId});
            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }


        [HttpPut]
        public async Task<IActionResult> UpdateElectronicCigarreteDetail(UpdateElectronicCigarreteDetailsCommand request)
        {
            var result = await _mediator.Send(request);
            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteElectronicCigarreteDetail(DeleteElectronicCigarreteDetailsCommand request)
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
