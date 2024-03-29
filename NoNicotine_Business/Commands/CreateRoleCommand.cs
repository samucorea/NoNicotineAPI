﻿using MediatR;
using Microsoft.AspNetCore.Identity;
using NoNicotineAPI.Models;


namespace NoNicotine_Business.Commands
{
    public class CreateRoleCommand : IRequest<Response<IdentityRole>>
    {
        public string Name { get; set; } = string.Empty;
    }
}
