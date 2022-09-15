using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Data.Entities
{
    public class Habit : BaseEntity
    {
        public string Name { get; set; }

        public List<PatientHabit> PatientHabits { get; set; }
    }
}
