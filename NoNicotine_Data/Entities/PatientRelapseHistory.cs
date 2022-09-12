using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Data.Entities
{
    public class PatientRelapseHistory : BaseEntity
    {
        public decimal AmountSaved { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }

        [ForeignKey("Patient")]
        public string PatientId { get; set; }
        public Patient Patient { get; set; }
    }
}
