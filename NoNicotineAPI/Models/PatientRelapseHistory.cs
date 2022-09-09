using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoNicotineAPI.Models
{
    public class PatientRelapseHistory
    {
        public int PatientRelapseHistoryId { get; set; }
        public decimal AmountSaved { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }

        public Guid PatientId { get; set; }
        public Patient Patient { get; set; }
    }
}
