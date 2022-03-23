using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAPPT.Web.Models
{
    public class AddtariffRateUploadSerializer
    {
        [Required(ErrorMessage = "WSPUtilityId Is Required")]
        public int WSPUtilityId { get; set; }
        [Required(ErrorMessage = "TariffId Is Required")]
        public int TariffId { get; set; }
        [Required(ErrorMessage = "MinimumFlow Is Required")]
        public decimal MinimumFlow { get; set; }
        [Required(ErrorMessage = "MaximumFlow Is Required")]
        public decimal MaximumFlow { get; set; }
        [Required(ErrorMessage = "Rate Is Required")]
        public decimal Rate { get; set; }
    }
}
