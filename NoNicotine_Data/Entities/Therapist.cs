using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Data.Entities
{
    public class Therapist : BaseEntity
    {
        public string Name { get; set; }
        public char Sex { get; set; }
        public DateTime BirthDate { get; set; }
        public string? Identification { get; set; }
        public string? IdentificationType { get; set; }
        public bool Active { get; set; }

        [ForeignKey("IdentityUser")]
        public string? IdentityUserId { get; set; }

        public List<Patient>? Patients { get; set; }
        public List<LinkRequest>? LinkRequests { get; set; }
    }
}
