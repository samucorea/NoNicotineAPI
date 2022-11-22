﻿using MediatR;
using NoNicotine_Data.Entities;
using NoNicotineAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Business.Commands.Update
{
    public class IndicateRelapseCommand : IRequest<Response<Patient>>
    {
        public string UserId { get; set; } = string.Empty;
        public DateTime RestartDate { get; set; }
    }
}