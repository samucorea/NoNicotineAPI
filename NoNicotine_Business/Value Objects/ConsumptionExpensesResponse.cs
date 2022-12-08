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
        public int Cigarette { get; set; }
        public int ElectronicCigarette { get; set; }
        public int Cigar { get; set; }
        public int Hookah { get; set; }
    }
}
