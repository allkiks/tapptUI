using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAPPT.Web.Models
{
    public class MeterUploadSerializer
    {
        [Required(ErrorMessage = "Customer Account Id Is Required")]
        public int CustomerAccountId { get; set; }
        [Required(ErrorMessage = "Meter Code Is Required")]
        public string MeterCode { get; set; }
        [Required(ErrorMessage = "IME Number Is Required")]
        public string IMENumber { get; set; }
        [Required(ErrorMessage = "SIM Card Number Is Required")]
        public string SIMCardNumber { get; set; }
        [Required(ErrorMessage = "Longitude Is Required")]
        public string Longitude { get; set; }
        [Required(ErrorMessage = "Latitude Is Required")]
        public string Latitude { get; set; }
        public int MeterId { get; set; }
        public string SearchTerm { get; set; }
        [Required(ErrorMessage = "Zone Is Required")]
        public int ZoneId { get; set; }
    }
}
