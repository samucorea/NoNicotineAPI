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
    public class UpdatePatientCommand : IRequest<Response<Patient>>
    {
        public string Id { get; set; } = string.Empty;
        public string? Name { get; set; }

        public DateTime? BirthDate { get; set; }

        public char? Sex { get; set; }
    }
}
