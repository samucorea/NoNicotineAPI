using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoNicotineAPI.Models
{
    public class Entry
    {
        public Guid EntryId { get; set; }
        public DateTime Date { get; set; }
        public string Message { get; set; }
        public bool TherapistAllowed { get; set; }

        public Guid PatientId { get; set; }
        public Patient Patient { get; set; }
        public ICollection<Symptom> Symptoms { get; set; }
        public ICollection<Feeling> Feelings { get; set; }
}
}
