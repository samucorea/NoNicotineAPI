using NoNicotine_Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoNicotine_Business.Value_Objects
{
    public class TherapistPatient : BaseEntity
    {
        public string Name { get; set; }
        public char Sex { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime StartTime { get; set; }
        public bool Active { get; set; }

        public string? IdentityUserId {get; set;}

        [ForeignKey("Therapist")]
        public string? TherapistId { get; set; }

        [ForeignKey("PatientConsumptionMethods")]
        public string? PatientConsumptionMethodsId { get; set; }
        public PatientConsumptionMethods? PatientConsumptionMethods { get; set; }
    }
}
