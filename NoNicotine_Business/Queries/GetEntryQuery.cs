﻿using MediatR;
using NoNicotine_Data.Entities;
using NoNicotineAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Business.Queries
{
    public class GetEntryQuery : IRequest<Response<Entry>>
    {
        public string UserId { get; set; } = string.Empty;
        public string EntryId { get; set; } = string.Empty;
    }
}
