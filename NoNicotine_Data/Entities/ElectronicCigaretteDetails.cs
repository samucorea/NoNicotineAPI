using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Data.Entities
{
    public class ElectronicCigaretteDetails : BaseEntity
    {
        public short cartridgeLifespan { get; set; }
        public short unitsPerBox { get; set; }
        public decimal boxPrice { get; set; }

        public string PatientConsumptionMethodsId { get; set; }
        public PatientConsumptionMethods PatientConsumptionMethods { get; set; }
    }
}
