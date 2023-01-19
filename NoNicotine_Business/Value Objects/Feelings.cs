using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Business.Value_Objects
{
    public static class Feelings
    {
        public static readonly Dictionary<string, bool> Values = new Dictionary<string, bool>
        {
            {"sad", true },
            {"happy", true },
            {"proud", true },
            {"anxious", true },
            {"irritable", true},
        };
    }
}
