using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Data.Entities
{
    public class Feeling : BaseEntity
    {
        public string Name { get; set; }

        public ICollection<Entry> Entries { get; set; }
    }
}
