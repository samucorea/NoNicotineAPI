using Microsoft.AspNetCore.Identity;
using NoNicotine_Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Business.Services
{
    public interface IAuthenticationService
    {
        public string CreateToken(IdentityUser user, string role);

        public RefreshToken GenerateRefreshToken(string userId);
    }
}
