using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoNicotineAPI.Models
{
    public class IdentificationType
    {
        public int IdentificationTypeId { get; set; }
        public string Name { get; set; }

        public List<Therapist> Therapists { get; set; }
    }
}
