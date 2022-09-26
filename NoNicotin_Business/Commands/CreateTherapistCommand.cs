using MediatR;
using NoNicotin_Business.Value_Objects;
using NoNicotine_Data.Entities;
using NoNicotineAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotin_Business.Commands
{
    public class CreateTherapistCommand:IRequest<Response<Therapist>>
    {
        public string Name { get; set; }
        public char Sex { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Identification { get; set; }

        public string IdentificationTherapistType { get; set; } = IdentificationType.IDENTIFICATION_CARD;
    }
}
