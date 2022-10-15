using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Business.Value_Objects
{
    public static class Symptoms
    {
        public readonly static  Dictionary<string, bool> Values = new()
        {
            {"tired", true },
            {"headache", true }
        };
    }
}
