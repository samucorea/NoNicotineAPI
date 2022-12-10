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
     
        public List<string> Symptoms { get; set; } = new List<string>();


        public List<string> Feelings { get; set; } = new List<string>();
    }
}
