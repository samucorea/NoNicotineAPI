using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoNicotineAPI.Models
{
    public class LinkRequest
    {
        public int LinkRequestId { get; set; }
        public DateTime DateAcceptedOrDeclined { get; set; }

        public int LinkRequestStatusId { get; set; }
        public LinkRequestStatus LinkRequestStatus { get; set; }
        public ICollection<Therapist> Therapists { get; set; }
        public ICollection<Patient> Patients { get; set; }
    }
}
