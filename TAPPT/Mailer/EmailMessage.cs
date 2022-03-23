using System.Collections.Generic;

namespace TAPPT.Web.Mailer
{
    public class EmailMessage
    {
        public EmailMessage()
        {
            ToAddresses = new List<EmailAddress>();
            FromAddresses = new List<EmailAddress>();
            Attachments = new List<string>();
        }
        public List<EmailAddress> ToAddresses { get; private set; }
        public List<EmailAddress> FromAddresses { get; private set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public List<string> Attachments { get; private set; }
    }

}
