﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using NoNicotin_Business.Commands;

namespace NoNicotineAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : Controller
    {
        private readonly IMediator _mediator;

        public RolesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleCommand request)
        {
            var result = await _mediator.Send(request);

            if(result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }
}
