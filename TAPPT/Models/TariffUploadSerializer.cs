using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAPPT.Web.Models
{
    public class TariffUploadSerializer
    {
        [Required(ErrorMessage = "Tariff Name Is Required")]
        public string Name { get; set; }
        public int Id { get; set; }
    }
}

