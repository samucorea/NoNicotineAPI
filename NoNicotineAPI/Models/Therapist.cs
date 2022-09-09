using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoNicotineAPI.Models
{
    public class Therapist
    {
        public Guid TherapistId { get; set; }
        public string Name { get; set; }
        public char Sex { get; set; }
        public DateTime BirthDate { get; set; }
        public string Identification { get; set; }

        public int IdentificationTypeId { get; set; }
        public IdentificationType IdentificationType { get; set; }
        public List<Patient> Patients { get; set; }
        public List<LinkRequest> LinkRequests { get; set; }
    }
}
