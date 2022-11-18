using MediatR;
using NoNicotine_Data.Entities;
using NoNicotineAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Business.Commands.Update
{
    public class UpdatePatientHabitCommand : IRequest<Response<PatientHabit>>
    {
        public string PatientHabitId { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;
        public DateTime? Hour { get; set; }
        public bool? Monday { get; set; }
        public bool? Tuesday { get; set; }
        public bool? Wednesday { get; set; }
        public bool? Thursday { get; set; }
        public bool? Friday { get; set; }
        public bool? Saturday { get; set; }
        public bool? Sunday { get; set; }
    }
}
