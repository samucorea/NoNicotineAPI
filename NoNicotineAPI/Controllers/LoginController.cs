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
using System.Web;

namespace NoNicotineAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IAuthenticationService _authenticationService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailService _emailService;

        public LoginController(IMediator mediator, IAuthenticationService authenticationService, UserManager<IdentityUser> userManager, IEmailService emailService)
        {
            _mediator = mediator;
            _authenticationService = authenticationService;
            _userManager = userManager;
            _emailService = emailService;
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

        [HttpPost]
        [Route("PasswordReset")]
        public async Task<IActionResult> PasswordReset(ForgotPasswordRequest request)
        {
            if (request.Email == string.Empty)
            {
                return BadRequest(new Response<int>
                {
                    Message = "You must specify an email to reset password"
                });
            }
            var user = await _userManager.FindByEmailAsync(request.Email);
            if(user == null)
            {
                return BadRequest(new Response<int>
                {
                    Message = "No user was found with specified email"
                });
            }

            var forgotPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            if(forgotPasswordToken == null)
            {
                return BadRequest(new Response<int>
                {
                    Message = "Something went wrong"
                });
            }

            var forgotPasswordLink = Url.ActionLink("Index", "ForgotPassword", new { token = HttpUtility.UrlEncode(forgotPasswordToken), email = user.Email });
            if (forgotPasswordLink == null)
            {
                return BadRequest(new Response<int>
                {
                    Message = "Something went wrong"
                });
                
            }

            _emailService.SendPasswordRecoveryEmail(user.Email, forgotPasswordLink);

            return Ok(new Response<int>
            {
                Message = "Reset password email sent successfully"
            });
        }
    }

}