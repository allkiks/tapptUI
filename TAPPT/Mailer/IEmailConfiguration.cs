namespace TAPPT.Web.Mailer
{
    public interface IEmailConfiguration
    {
        #region Smtp
        string SmtpServer { get; }
        int SmtpPort { get; }
        string SmtpUsername { get; set; }
        string SmtpPassword { get; set; } 
        #endregion
    }
}
