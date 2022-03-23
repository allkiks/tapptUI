using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAPPT.Web.Models
{
    public class AuditLogUploadViewModel
    {
        public string CreatedBy { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public int Id { get; set; }

        public string Username { get; set; }
    }
}
