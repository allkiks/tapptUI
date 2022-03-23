using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAPPT.Web.Models
{
    public class PostCustomerAndMeterModel
    {
        public int WSPUtilityId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string PostalAddress { get; set; }
        public string Residence { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string IdNumber { get; set; }
        public string Group { get; set; }
        public string IdentityId { get; set; }
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
    }
}
