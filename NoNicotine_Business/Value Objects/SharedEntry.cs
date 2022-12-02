using NoNicotine_Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Business.Value_Objects
{
    public class SharedEntry : BaseEntity
    {
        public string Message { get; set; } = string.Empty;
        public bool TherapistAllowed { get; set; }
        public string PatientId { get; set; }
        //saved as "tired,headache"
        public string Symptoms { get; set; } = string.Empty;

        //saved as "sad,shamed"
        public string Feelings { get; set; } = string.Empty;
    }
}
