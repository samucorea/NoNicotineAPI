﻿using NoNicotine_Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Business.Value_Objects
{
    public class AuthenticationData
    {
        public string Token { get; set; } = string.Empty;

        public RefreshToken RefreshToken { get; set; } = new RefreshToken();

    }
}
