using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Business.Services
{
    public interface IEmailService
    {
        public string SendPasswordRecoveryEmail(string recipient, string resetPasswordLink);

        public string SendEmailConfirmation(string recipient, string link);
    }
}
