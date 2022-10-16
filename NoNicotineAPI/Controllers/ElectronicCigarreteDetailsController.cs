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

            var result = await _mediator.Send(request);
            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetElectronicCigarreteDetail(GetElectronicCigarreteDetailQuery request)
        {

            var result = await _mediator.Send(request);
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
    }
}
