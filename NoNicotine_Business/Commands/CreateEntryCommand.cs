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
    public class CreateEntryCommand : IRequest<Response<Entry>>
    {
        public string UserId { get; set; } = string.Empty;

        public bool TherapistAllowed { get; set; }

        public string Message { get; set; } = string.Empty;
        public List<string> Symptoms { get; set; } = new List<string>();
        public List<string> Feelings { get; set; } = new List<string>();

    }
}
