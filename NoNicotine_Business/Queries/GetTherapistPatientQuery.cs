using MediatR;
using NoNicotine_Business.Value_Objects;
using NoNicotineAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Business.Queries
{
    public class GetTherapistPatientQuery : IRequest<Response<TherapistPatient>>
    {
        public string UserId { get; set; } = string.Empty;
        public string PatientId { get; set; } = string.Empty;
    }
}
