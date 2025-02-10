using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class SendGridEmailSender : IEmailSender
    {
        private readonly SendGridSettings _sendGridSettings;

        public SendGridEmailSender(IOptions<SendGridSettings> sendGridSettings)
        {
            _sendGridSettings = sendGridSettings.Value ?? throw new ArgumentNullException(nameof(sendGridSettings));
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new InvalidOperationException("SendGrid API Key is not configured.");
            }

            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(_sendGridSettings.SenderEmail, _sendGridSettings.SenderName);
            var to = new EmailAddress(email);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, message, message);
            var response = await client.SendEmailAsync(msg);
        }
    }

    public class SendGridSettings
    {
        public string SenderEmail { get; set; } = string.Empty;
        public string SenderName { get; set; } = string.Empty;
    }
}