using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Data.Entities
{
    public class PatientConsumptionMethods : BaseEntity
    {
        [ForeignKey("Patient")]
        public string PatientId { get; set; }
        public Patient Patient { get; set; }

        [ForeignKey("CigaretteDetails")]
        public string? CigaretteDetailsId { get; set; }
        public CigaretteDetails? CigaretteDetails { get; set; }

        [ForeignKey("ElectronicCigaretteDetails")]
        public string? ElectronicCigaretteDetailsId { get; set; }
        public ElectronicCigaretteDetails? ElectronicCigaretteDetails { get; set; }

        [ForeignKey("CigarDetails")]
        public string? CigarDetailsId { get; set; }
        public CigarDetails? CigarDetails { get; set; }

        [ForeignKey("HookahDetails")]
        public string? HookahDetailsId { get; set; }
        public HookahDetails? HookahDetails { get; set; }
    }
}
