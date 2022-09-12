using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Data.Entities
{
    public class Patient : BaseEntity
    {
        public string Name { get; set; }
        public char Sex { get; set; }
        public DateTime BirthDate { get; set; }
        public decimal DailyConsumption { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime AffiliationDate { get; set; }

        [ForeignKey("Therapist")]
        public string? TherapistId { get; set; }
        public Therapist? Therapist { get; set; }
        public List<Entry> Entries { get; set; }
        public List<LinkRequest> LinkRequests { get; set; }
        public List<PatientRelapseHistory> PatientRelapseHistoric { get; set; }
        public List<PatientHabit> PatientHabits { get; set; }
    }
}
