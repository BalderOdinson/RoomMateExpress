using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RoomMateExpressWebApi.Helpers;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace RoomMateExpressWebApi.Services
{
    public class EmailSender : IEmailSender
    {
        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public AuthMessageSenderOptions Options { get; } //set only via Secret Manager

        public Task SendEmailAsync(string email, string subject, string message, bool isImportant)
        {
            return Execute(Options.SendGridKey, subject, message, email, isImportant);
        }

        public Task Execute(string apiKey, string subject, string message, string email, bool isImportant)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("dorianb2@hotmail.com", "RoomMateExpress service"),
                ReplyTo = new EmailAddress("room.mate.express@noreply.com", "RoomMateExpress service"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message,
                MailSettings = new MailSettings
                {
                    BypassListManagement = new BypassListManagement
                    {
                        Enable = isImportant
                    }
                }
            };
            msg.AddTo(new EmailAddress(email));
            return client.SendEmailAsync(msg);
        }
    }
}
