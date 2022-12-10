using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Data.Entities
{
    public class HookahDetails : BaseEntity
    {
        public short daysPerWeek { get; set; }
        public decimal setupPrice { get; set; }

        public string PatientConsumptionMethodsId { get; set; }
    }
}
