using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NoNicotine_Business.Commands;
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

namespace NoNicotine_Business.Handler
{
    public class CreateRefreshTokenCommandHandler : IRequestHandler<CreateRefreshTokenCommand, Response<AuthenticationData>>
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IAuthenticationService _authenticationService;


        public CreateRefreshTokenCommandHandler(AppDbContext context, ILogger<AuthenticateQueryHandler> logger, IConfiguration configuration, UserManager<IdentityUser> userManager, IAuthenticationService authenticationService)
        {
            _context = context;
            _configuration = configuration;
            _userManager = userManager;
            _authenticationService = authenticationService;
        }

        public async Task<Response<AuthenticationData>> Handle(CreateRefreshTokenCommand request, CancellationToken cancellationToken)
        {

            var response = ValidateRequest(request);
            if (response != null)
            {
                return response;
            }

            var userRefreshToken = _context.RefreshToken.Where(refreshToken => refreshToken.Token == request.RefreshToken
            ).FirstOrDefault();

            if (userRefreshToken == null)
            {
                return new Response<AuthenticationData>
                {
                    Message = "Refresh token for user not found",
                    Succeeded = false
                };
            }

            if (userRefreshToken.ExpiresAt < DateTime.Now)
            {
                return new Response<AuthenticationData>
                {
                    Message = "Refresh token expired",
                    Succeeded = false,
                };

            }

            var newRefreshToken = _authenticationService.GenerateRefreshToken(userRefreshToken.UserId);

            _context.RefreshToken.Add(newRefreshToken);
            _context.RefreshToken.Remove(userRefreshToken);

            var result = _context.SaveChanges();

            if (result <= 1)
            {
                return new Response<AuthenticationData>
                {
                    Message = "Something went wrong",
                    Succeeded = false
                };
            }

            var user = await _userManager.FindByIdAsync(userRefreshToken.UserId);
            if(user == null)
            {
                return new Response<AuthenticationData>()
                {
                    Message = "Something went wrong",
                    Succeeded = false
                };
            }

            var userRole = await _userManager.GetRolesAsync(user);

            var role = userRole.FirstOrDefault();

            if (role == null)
            {
                return new Response<AuthenticationData>()
                {
                    Message = "Something went wrong",
                    Succeeded = false
                };
            }

            var newToken = _authenticationService.CreateToken(user, role);

            return new Response<AuthenticationData>
            {
                Succeeded = true,
                Data = new AuthenticationData()
                {
                    Token = newToken,
                    RefreshToken = newRefreshToken,
                }
            };
        }

        private static Response<AuthenticationData>? ValidateRequest(CreateRefreshTokenCommand request)
        {
            if (request.RefreshToken == "")
            {
                return new Response<AuthenticationData>()
                {
                    Succeeded = false,
                    Message = "Missing Refresh Token"

                };
            }

            return null;
        }
    }

}
