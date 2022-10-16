﻿using MediatR;
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
        public async Task<IActionResult> GetPatientConsumptionMethod(GetPatientConsumptionMethodQuery request)
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

