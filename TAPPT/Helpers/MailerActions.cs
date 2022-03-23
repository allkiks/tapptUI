
using TAPPT.Web.Mailer;

namespace TAPPT.Web.Helpers
{
    public class MailerActions
    {
        public static void SendEmail(EmailMessage emailMessage, EmailService emailService, EmailConfiguration emailConfiguration)
        {

            emailService.Send(emailMessage, emailConfiguration);

        }
    }
}
