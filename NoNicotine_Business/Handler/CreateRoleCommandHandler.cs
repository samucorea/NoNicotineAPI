using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NoNicotine_Business.Commands;
using NoNicotineAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Business.Handler
{
    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, Response<IdentityRole>>
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<CreateRoleCommandHandler> _logger;

        public CreateRoleCommandHandler(RoleManager<IdentityRole> roleManager, ILogger<CreateRoleCommandHandler> logger)
        {
            _roleManager = roleManager;
            _logger = logger;
        }

        public async Task<Response<IdentityRole>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            if (request.Name == string.Empty)
            {
                return new Response<IdentityRole>()
                {

                    Succeeded = false,
                    Message = "Must specify a name for the role"
                };
            }

            var newRole = new IdentityRole()
            {
                Name = request.Name
            };

            var result = await _roleManager.CreateAsync(newRole);

            if (!result.Succeeded)
            {
                _logger.LogError("creating a new role", result.Errors);

                return new Response<IdentityRole>
                {
                    Succeeded = false,
                    Message = "Something went wrong"
                };
            }

            return new Response<IdentityRole>()
            {
                Succeeded = true,
                Data = newRole
            };
        }
    }
}
