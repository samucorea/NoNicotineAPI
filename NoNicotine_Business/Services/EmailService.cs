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
    public class EmailService : IEmailService
    {
        private readonly EmailClient _client;
        private readonly ILogger<EmailService> _logger;
        private static readonly string emailConfirmationSubject = "Confirmación de correo electrónico";
        private static readonly string forgotPasswordSubject = "Olvido de la contraseña";
        private static readonly string noNicotineEmail = Environment.GetEnvironmentVariable("NO_NICOTINE_EMAIL") ?? "";

        public EmailService(ILogger<EmailService> logger)
        {
            _logger = logger;
            string connectionString = Environment.GetEnvironmentVariable("COMMUNICATION_SERVICES_CONNECTION_STRING") ?? "";
            if (connectionString == "")
            {
                throw new Exception("Communication services connection string not found");
            }
            _client = new EmailClient(connectionString);

        }

        private string SendEmail(string subject, string[] rawEmailRecipients, string emailHtmlContent, object templateVariables)
        {
            var emailAddressRecipients = rawEmailRecipients.Select(email => new EmailAddress(email: email));
            var emailRecipients = new EmailRecipients(emailAddressRecipients);

            var stubble = new StubbleBuilder().Build();

            var output = stubble.Render(emailHtmlContent, templateVariables);
            var emailContent = new EmailContent(subject)
            {
                Html = output
            };

            var emailMessage = new EmailMessage(noNicotineEmail, emailContent, emailRecipients);

            _logger.LogInformation("{sender} {reciever}", noNicotineEmail, rawEmailRecipients[0]);

            var result = _client.Send(emailMessage);
           

            return result.Value.MessageId;
        }

        public string SendEmailConfirmation(string recipientEmail, string confirmationLink)
        {
            string emailConfirmationTemplate = File.ReadAllText($"wwwroot/Assets/EmailTemplates/EmailConfirmationTemplate.Mustache");
            return SendEmail(emailConfirmationSubject, new string[] { recipientEmail }, emailConfirmationTemplate, new { ConfirmationLink = confirmationLink });
        }

        public string SendPasswordRecoveryEmail(string recipientEmail, string resetPasswordLink)
        {
            string forgotPasswordTemplate = File.ReadAllText($"wwwroot/Assets/EmailTemplates/ForgotPasswordTemplate.Mustache");
            return SendEmail(forgotPasswordSubject, new string[] { recipientEmail }, forgotPasswordTemplate, new { ForgotPasswordLink = resetPasswordLink });
        }
    }
}
