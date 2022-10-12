﻿using MediatR;
using NoNicotine_Business.Value_Objects;
using NoNicotine_Data.Entities;
using NoNicotineAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Business.Queries
{
    public class RefreshTokenQuery : IRequest<Response<AuthenticationData>>
    {
        public string UserId { get; set; } = string.Empty;

        public string Token { get; set; } = string.Empty;
    }
}
