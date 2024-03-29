﻿using MediatR;
using NoNicotine_Business.Value_Objects;
using NoNicotine_Data.Entities;
using NoNicotineAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Business.Commands
{
    public class CreateRefreshTokenCommand : IRequest<Response<AuthenticationData>>
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
