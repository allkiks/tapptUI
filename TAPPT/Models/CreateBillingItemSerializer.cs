using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAPPT.Web.Models
{
    public class CreateBillingItemSerializer
    {
        [Required(ErrorMessage = "Billing Item Name Is Required")]
        public string ItemName { get; set; }
        [Required(ErrorMessage = "UserId Is Required")]
        public int UserId { get; set; }
        [Required(ErrorMessage = "Rate Is Required")]
        public decimal Rate { get; set; }
    }
}
