using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAPPT.Web.Models
{
    public class BillingItemDownLoadSerializer
    {
        public int Id { get; set; }
        public decimal Rate { get; set; }
        public List<BillingsViewModel> Billings { get; set; }
    }
}
