namespace TAPPT.Web.Mailer
{
    public interface IEmailService
    {
        void Send(EmailMessage message,EmailConfiguration emailConfiguration);
    }
}
