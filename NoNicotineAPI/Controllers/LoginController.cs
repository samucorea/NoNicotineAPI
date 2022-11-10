using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NoNicotine_Business.Commands;
using NoNicotine_Business.Queries;
using NoNicotine_Business.Services;
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
        private readonly IAuthenticationService _authenticationService;

        public LoginController(IMediator mediator, IAuthenticationService authenticationService)
        {
            _mediator = mediator;
            _authenticationService = authenticationService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(AuthenticateQuery request)
        {
            var result = await _mediator.Send(request);

            if (result.Succeeded && result.Data != null)
            {
                var cookieOptions = new CookieOptions()
                {
                    HttpOnly = true,
                    Expires = result.Data.RefreshToken.ExpiresAt
                };

                Response.Cookies.Append("refreshToken", result.Data.RefreshToken.Token, cookieOptions);

                var response = new AuthResponse()
                {
                    Token = result.Data.Token,
                    RefreshToken = result.Data.RefreshToken.Token
                };
                return Ok(response);
            }



            return BadRequest(result);

        }

        [HttpPost]
        [Route("RefreshToken")]
        public async Task<IActionResult> RefreshToken(CreateRefreshTokenCommand request)
        {


            string? requestRefreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(requestRefreshToken))
            {
                return BadRequest("No refresh token found");
            }

            request.RefreshToken = requestRefreshToken;

            var result = await _mediator.Send(request);
         
            if (result.Succeeded && result.Data != null)
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
                    Token = result.Data.Token,
                    RefreshToken = result.Data.RefreshToken.Token
                };

                return Ok(response);
            }

            return Unauthorized(result);


        }
    }

}