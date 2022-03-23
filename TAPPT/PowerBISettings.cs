using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAPPT.Web
{
    public class PowerBISettings
    {
        public string ApplicationId { get; set; }
        public string ApplicationSecret { get; set; }
        public string ReportId { get; set; }
        public string WorkspaceId { get; set; }
        public string AuthorityUrl { get; set; }
        public string ResourceUrl { get; set; }
        public string ApiUrl { get; set; }
        public string EmbedUrlBase { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
