using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [ForeignKey("Habit")]
        public string HabitId { get; set; }
        public Habit Habit { get; set; }

        public DateTime Hour { get; set; }
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }
    }
}
