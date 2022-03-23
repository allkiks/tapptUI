using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAPPT.Web.Models
{
    public class BillingsViewModel
    {
        public int Id { get; set; }
        public int BillingItemId { get; set; }
        public string ItemName { get; set; }
        public int BillingSettingId { get; set; }
    }
}
