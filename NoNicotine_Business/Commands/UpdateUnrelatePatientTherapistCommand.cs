using MediatR;
using NoNicotine_Data.Entities;
using NoNicotineAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Business.Commands
{
    public class UpdateUnrelatePatientTherapistCommand : IRequest<Response<bool>>
    {
        public string UserId { get; set; } = string.Empty;
        public string PatientId { get; set; } = string.Empty;
    }
}
