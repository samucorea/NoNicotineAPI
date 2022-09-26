using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Data.Entities
{
    public class LoginResponse
    {

        public List<string> Errors { get; set; } = new List<string>();

        public bool Authenticated { get; set; } = false;

        public bool EmailOrPasswordEmpty { get; set; } = false;

        public bool WrongEmailOrPassword { get; set; } = false;

    }
}
