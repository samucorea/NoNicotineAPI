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
    public class CigarreteDetailsController : Controller
    {

        public CigarreteDetailsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        private readonly IMediator _mediator;

        [HttpPost]
        public async Task<IActionResult> CreateCigarreteDetail(CreateCigarreteDetailsCommand request)
        {

            var result = await _mediator.Send(request);
            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetCigarreteDetail(GetCigarreteDetailsQuery request)
        {

            var result = await _mediator.Send(request);
            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }


        [HttpPut]
        public async Task<IActionResult> UpdateCigarreteDetail(UpdateCigarreteDetailsCommand request)
        {
            var result = await _mediator.Send(request);
            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCigarreteDetail(DeleteCigarreteDetailsCommand request)
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

