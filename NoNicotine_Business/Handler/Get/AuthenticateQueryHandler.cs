using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NoNicotine_Business.Queries;
using NoNicotine_Business.Services;
using NoNicotine_Business.Value_Objects;
using NoNicotine_Data.Context;
using NoNicotine_Data.Entities;
using NoNicotineAPI.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Business.Handler.Get
{
    public class AuthenticateQueryHandler : IRequestHandler<AuthenticateQuery, Response<AuthenticationData>>
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthenticateQueryHandler> _logger;
        private readonly IAuthenticationService _authenticationService;

        public AuthenticateQueryHandler(UserManager<IdentityUser> userManager, IConfiguration configuration, AppDbContext context, ILogger<AuthenticateQueryHandler> logger, IAuthenticationService authenticationService)
        {
            _userManager = userManager;
            _configuration = configuration;
            _context = context;
            _logger = logger;
            _authenticationService = authenticationService;
        }

        public async Task<Response<AuthenticationData>> Handle(AuthenticateQuery request, CancellationToken cancellationToken)
        {

            var response = ValidateRequest(request);
            if (response != null)
            {
                return response;
            }

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                return new Response<AuthenticationData>()
                {
                    Succeeded = false,
                    Message = "Wrong email and/or password"
                };
            }

            var result = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!result)
            {
                return new Response<AuthenticationData>()
                {
                    Succeeded = false,
                    Message = "Wrong email and/or password"
                };

            }

            var roles = await _userManager.GetRolesAsync(user);
            if (roles == null || roles.Count < 1)
            {
                _logger.LogError("Could not find roles in user {user}", user);
                return new Response<AuthenticationData>
                {
                    Succeeded = false,
                    Message = "Something went wrong"
                };
            }

            var role = roles.FirstOrDefault();

            var tokenString = _authenticationService.CreateToken(user, role);
            if (tokenString == "")
            {
                return new Response<AuthenticationData>
                {
                    Succeeded = false,
                    Message = "Could not authenticate user"
                };
            }

            var newRefreshToken = _authenticationService.GenerateRefreshToken(user.Id);

            _context.RefreshToken.Add(newRefreshToken);

            var resultCreationRefreshToken = _context.SaveChanges();

            if (resultCreationRefreshToken < 0)
            {
                _logger.LogError("Could not save refresh token");
                return new Response<AuthenticationData>
                {
                    Succeeded = false,
                    Message = "Something went wrong"
                };
            }

            return new Response<AuthenticationData>
            {
                Succeeded = true,
                Data = new AuthenticationData
                {
                    Token = tokenString,
                    RefreshToken = newRefreshToken
                }
            };


        }

        private static Response<AuthenticationData>? ValidateRequest(AuthenticateQuery request)
        {
            if (request.Email == string.Empty || request.Password == string.Empty)
            {
                return new Response<AuthenticationData>()
                {
                    Succeeded = false,
                    Message = "Email or password missing"

                };
            }

            return null;
        }

    }
}
