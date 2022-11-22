using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NoNicotine_Data.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace NoNicotine_Business.Services
{
  public class AuthenticationService : IAuthenticationService
  {
    private readonly IConfiguration _configuration;
    private readonly UserManager<IdentityUser> _userManager;
    public AuthenticationService(IConfiguration configuration, UserManager<IdentityUser> userManager)
    {
      _configuration = configuration;
      _userManager = userManager;
    }

    public string CreateToken(IdentityUser user, string role)
    {
      if (user == null || role == "")
      {
        return "";
      }

      var subject = _configuration["Subject"];
      if (subject == null)
      {
        return "";
      }

      var jwtKey = _configuration["Key"];
      if (jwtKey == null)
      {
        return "";
      }

      var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, subject),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserId", user.Id),
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
          _configuration["Issuer"],
          _configuration["Audience"],
          claims,
          expires: DateTime.UtcNow.AddMinutes(10),
          signingCredentials: signIn);
      if (token == null)
      {
        return "";
      }

      return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public RefreshToken GenerateRefreshToken(string userId)
    {
      return new RefreshToken()
      {
        UserId = userId,
        Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
        ExpiresAt = DateTime.Now.AddDays(7)
      };
    }

    public async Task<string> GenerateEmailConfirmationUrlTokenAsync(IdentityUser user)
    {
      var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
      return HttpUtility.UrlEncode(code);
    }

    public string GetUserIdFromClaims(ClaimsIdentity claims)
    {
      var patientUserIdClaim = claims.FindFirst("UserId");
      if (patientUserIdClaim == null)
      {
        return "";
      }

      return patientUserIdClaim.Value;
    }
  }
}
