﻿using MediatR;
using NoNicotine_Business.Value_Objects;
using NoNicotine_Data.Entities;
using NoNicotineAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Business.Commands.Create
{
    public class CreateTherapistCommand : IRequest<Response<Therapist>>
    {
        public string Name { get; set; } = string.Empty;
        public char Sex { get; set; } = ' ';
        public DateTime BirthDate { get; set; } = DateTime.Now;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Identification { get; set; } = string.Empty;
        public string IdentificationTherapistType { get; set; } = string.Empty;
    }
}