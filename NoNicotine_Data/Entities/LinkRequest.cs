using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Data.Entities
{
    public class LinkRequest : BaseEntity
    {
        public DateTime DateAcceptedOrDeclined { get; set; }

        public bool? RequestAccepted { get; set; }

        [ForeignKey("Therapist")]
        public string? TherapistId { get; set; }
        public Therapist? Therapist { get; set; }

        [ForeignKey("Patient")]
        public string PatientId { get; set; }
        public Patient Patient { get; set; }
    }
}
