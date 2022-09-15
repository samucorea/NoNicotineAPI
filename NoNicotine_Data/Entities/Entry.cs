using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Data.Entities
{
    public class Entry : BaseEntity
    {
        public string Message { get; set; }
        public bool TherapistAllowed { get; set; }

        [ForeignKey("Patient")]
        public string PatientId { get; set; }
        public Patient Patient { get; set; }
        public ICollection<Symptom> Symptoms { get; set; }
        public ICollection<Feeling> Feelings { get; set; }
    }
}
