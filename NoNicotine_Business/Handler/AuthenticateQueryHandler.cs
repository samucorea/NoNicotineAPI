using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NoNicotine_Business.Queries;
using NoNicotine_Business.Value_Objects;
using NoNicotine_Data.Entities;
using NoNicotineAPI.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Business.Handler
{
    public class AuthenticateQueryHandler : IRequestHandler<AuthenticateQuery, Response<AuthenticationData>>
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthenticateQueryHandler> _logger;

        public AuthenticateQueryHandler(UserManager<IdentityUser> userManager, IConfiguration configuration, ILogger<AuthenticateQueryHandler> logger)
        {
            _userManager = userManager;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<Response<AuthenticationData>> Handle(AuthenticateQuery request, CancellationToken cancellationToken)
        {

            var response = ValidateInputs(request);
            if(response != null)
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
                _logger.LogError("Could not find roles in user", user);
                return new Response<AuthenticationData>
                {
                    Succeeded = false,
                    Message = "Something went wrong"
                };
            }

            var role = roles.FirstOrDefault();

            if (role == null)
            {
                _logger.LogError("Could not find roles in user", user);
                return new Response<AuthenticationData>
                {
                    Succeeded = false,
                    Message = "Something went wrong"
                };
            }


            var tokenString = CreateToken(user, role);
            if (tokenString == "")
            {
                return new Response<AuthenticationData>
                {
                    Succeeded = false,
                    Message = "Could not authenticate user"
                };
            }

            return new Response<AuthenticationData>
            {
                Succeeded = true,
                Data = new AuthenticationData
                {
                    Token = tokenString,
                }
            };


        }
        
        private Response<AuthenticationData>? ValidateInputs(AuthenticateQuery request)
        {
            if (request.Email == null || request.Email == "" || request.Password == "" || request.Password == null)
            {
                return new Response<AuthenticationData>()
                {
                    Succeeded = false,
                    Message = "Email or password missing"

                };
            }

            return null;
        }


        private string CreateToken(IdentityUser user, string role)
        {
            if (user == null || role == "") {
                return "";
            }

            var subject = _configuration["Jwt:Subject"];
            if(subject == null)
            {
                return "";
            }

            var jwtKey = _configuration["Jwt:Key"];
            if (jwtKey == null)
            {
                return "";
            }

            var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, subject),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("Email", user.Email),
                        new Claim(ClaimTypes.Role, role)
                    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            if (key == null)
            {
                return "";
            }
            
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            if (signIn == null)
            {
                return "";
            }
            
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: signIn);
            if(token == null)
            {
                return "";
            }

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}
