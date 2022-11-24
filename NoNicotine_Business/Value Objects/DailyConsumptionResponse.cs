using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Business.Value_Objects
{
    public class ConsumptionExpensesResponse
    {
        public int Total { get; set; }
        public int CigaretteTotal { get; set; }
        public int ElectronicCigaretteTotal { get; set; }
        public int CigarTotal { get; set; }
        public int HookahTotal { get; set; }
    }
}
