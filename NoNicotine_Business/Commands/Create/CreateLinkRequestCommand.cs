using MediatR;
using NoNicotine_Data.Entities;
using NoNicotineAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Business.Commands.Create
{
    public class CreateLinkRequestCommand : IRequest<Response<LinkRequest>>
    {
        public string TherapistUserId { get; set; } = string.Empty;

        public string PatientEmail { get; set; } = string.Empty;
    }
}
