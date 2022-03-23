using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace TAPPT.Web.Identity.ViewModels
{
    public class RegisterViewModel
    {

        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string RoleName { get; set; }
        public string Residence { get; set; }
        public string PostaAddress { get; set; }
        public string IdNumber { get; set; }
        public int WSPId { get; set; }
        public string MeterCode { get; set; }
        public string IMENumber { get; set; }
        public string IMSI { get; set; }
        public string SIMCardNumber { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public int TariffId { get; set; }
        public int BillingSettingId { get; set; }
        public int ZoneId { get; set; }
        public int BouquetId { get; set; }
        public string UId { get; set; }
    }
}
