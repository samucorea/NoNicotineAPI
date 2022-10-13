using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NoNicotine_Business.Queries;
using NoNicotine_Data.Context;
using NoNicotine_Data.Entities;
using NoNicotineAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NoNicotineAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LoginController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Post(AuthenticateQuery request)
        {
            var result = await _mediator.Send(request);

            if (result.Succeeded)
            {
                var cookieOptions = new CookieOptions()
                {
                    HttpOnly = true,
                    Expires = result.Data.RefreshToken.ExpiresAt
                };

                Response.Cookies.Append("refreshToken", result.Data.RefreshToken.Token, cookieOptions);

                var response = new AuthResponse()
                {
                    Token = result.Data.Token
                };
                return Ok(response);
            }



            return BadRequest(result);

        }

        [HttpPost]
        [Route("RefreshToken")]
        public async Task<IActionResult> RefreshToken(RefreshTokenQuery request)
        {
            string? requestRefreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(requestRefreshToken))
            {
                return BadRequest("No refresh token found");
            }

            request.Token = requestRefreshToken;

            var result = await _mediator.Send(request);
            if (result.Data == null)
            {
                return BadRequest("Something went wrong");
            }


            if (result.Succeeded)
            {
                var refreshToken = result.Data.RefreshToken;
                var cookieOptions = new CookieOptions()
                {
                    HttpOnly = true,
                    Expires = refreshToken.ExpiresAt
                };

                Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);

                var response = new AuthResponse()
                {
                    Token = result.Data.Token
                };

                return Ok(response);
            }

            return Unauthorized(result.Message);


        }
    }

}