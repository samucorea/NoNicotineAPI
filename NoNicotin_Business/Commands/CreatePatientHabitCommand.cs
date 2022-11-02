using MediatR;
using NoNicotine_Data.Entities;
using NoNicotineAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotin_Business.Commands
{
    public class CreatePatientHabitCommand : IRequest<Response<PatientHabit>>
    {
        public string PatientId { get; set; }
        public string HabitId { get; set; }
        public string HabitScheduleId { get; set; }
    }
}
