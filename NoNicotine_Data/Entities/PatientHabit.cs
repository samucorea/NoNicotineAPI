using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Data.Entities
{
    public class PatientHabit : BaseEntity
    {        
        [ForeignKey("Patient")]
        public string PatientId { get; set; }
        public Patient Patient { get; set; }
        
        [ForeignKey("Habit")]
        public string HabitId { get; set; }
        public Habit Habit { get; set; }

        [ForeignKey("HabitSchedule")]
        public string HabitScheduleId { get; set; }
        public HabitSchedule HabitSchedule { get; set; }
    }
}
