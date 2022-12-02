using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Communication.Email;
using Azure.Communication.Email.Models;
using Microsoft.Extensions.Logging;
using Stubble.Core.Builders;

namespace NoNicotine_Business.Services
{
  public class DummyEmailService : IEmailService
  {


    public DummyEmailService()
    {


    }


    public string SendEmailConfirmation(string recipientEmail, string confirmationLink)
    {
      return "message-id";
    }

    public string SendPasswordRecoveryEmail(string recipientEmail, string resetPasswordLink)
    {
      return "message-id";
    }
  }
}
