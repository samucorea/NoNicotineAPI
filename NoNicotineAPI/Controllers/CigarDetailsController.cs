﻿using MediatR;
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
    public class CigarDetailsController : Controller
    {
        public CigarDetailsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        private readonly IMediator _mediator;

        [HttpPost]
        public async Task<IActionResult> CreateCigarDetail(CreateCigarDetailsCommand request)
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
        public async Task<IActionResult> GetCigarDetail(string patientConsumptionId)
        {

            var result = await _mediator.Send(new GetCigarDetailsQuery() { PatientConsumptionId = patientConsumptionId });
            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }


        [HttpPut]
        public async Task<IActionResult> UpdateCigarDetail(UpdateCigarDetailsCommand request)
        {
            var result = await _mediator.Send(request);
            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }


        [HttpDelete]
        public async Task<IActionResult> DeleteCigarDetail(DeleteCigarDetailsCommand request)
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
