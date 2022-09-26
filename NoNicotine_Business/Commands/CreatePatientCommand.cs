using MediatR;
using Microsoft.AspNetCore.Identity;
using NoNicotin_Business.Value_Objects;
using NoNicotine_Data.Entities;
using NoNicotineAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotin_Business.Commands
{
    public class CreatePatientCommand:IRequest<Response<Patient>>
    {
        public string? Name { get; set; }
        public char Sex { get; set; }
        public DateTime BirthDate { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }

        public string? Identification { get; set; }

        public string IdentificationPatientType { get; set; } = IdentificationType.IDENTIFICATION_CARD;

    }
}
