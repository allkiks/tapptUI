using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAPPT.Web.Models
{
    public class UtilityUploadSerializer
    {
        [Required(ErrorMessage = "Name Is Required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "PhysicalAddress Is Required")]
        public string PhysicalAddress { get; set; }
        [Required(ErrorMessage = "PostalAddress Is Required")]
        public string PostalAddress { get; set; }
        [Required(ErrorMessage = "TPhoneNumber Is Required")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Email Is Required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "County Is Required")]
        public string County { get; set; }
        [Required(ErrorMessage = "PIN Is Required")]
        public string KRAPin { get; set; }
        public string Website { get; set; }
        public IFormFile LogoFile { get; set; }
        public string LogoUrl { get; set; }
        public int Id { get; set; }
        [Required(ErrorMessage = "Bouquet Id Is Required")]
        public int BouquetId { get; set; }
    }
}
