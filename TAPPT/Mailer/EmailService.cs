using MailKit.Net.Smtp;
using MimeKit;
using System.Linq;

namespace TAPPT.Web.Mailer
{
    public class EmailService : IEmailService
    {
        private readonly IEmailConfiguration _emailConfiguration;

        public EmailService(IEmailConfiguration emailConfiguration)
        {
            _emailConfiguration = emailConfiguration;
        }
        public void Send(EmailMessage emailMessage,EmailConfiguration emailConfiguration)
        {
            var message = new MimeMessage();
            message.To.AddRange(emailMessage.ToAddresses.Select(p => new MailboxAddress(p.Name, p.Address)));
            message.From.AddRange(emailMessage.FromAddresses.Select(p => new MailboxAddress(p.Name, p.Address)));
            message.Subject = emailMessage.Subject;
            var builder = new BodyBuilder
            {
                HtmlBody = emailMessage.Content
            };
            emailMessage.Attachments.ForEach(p =>
            {
                builder.Attachments.Add(p);
            });
            message.Body = builder.ToMessageBody();
            using (var emailClient = new SmtpClient())
            {
                emailClient.Connect(emailConfiguration.SmtpServer, emailConfiguration.SmtpPort,false);
                emailClient.AuthenticationMechanisms.Remove("XOAUTH2");
                emailClient.Authenticate(emailConfiguration.SmtpUsername, emailConfiguration.SmtpPassword);
                emailClient.Send(message);
                emailClient.Disconnect(true);
            }
        }
    }
}
