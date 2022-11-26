using NoNicotine_Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Business.Value_Objects
{
    public class CreateTherapistResponse
    {

        public Therapist? Therapist { get; set; }

        public string ConfirmationToken { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
    }
}
