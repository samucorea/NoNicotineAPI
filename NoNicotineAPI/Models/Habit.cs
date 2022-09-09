using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoNicotineAPI.Models
{
    public class Habit
    {
        public int HabitId { get; set; }
        public string Name { get; set; }

        public List<PatientHabit> PatientHabits { get; set; }
    }
}
