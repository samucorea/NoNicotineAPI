using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoNicotineAPI.Models
{
    public class Symptom
    {
        public int SymptomId { get; set; }
        public string Name { get; set; }

        public ICollection<Entry> Entries { get; set; }
    }
}
