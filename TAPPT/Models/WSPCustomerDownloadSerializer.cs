using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAPPT.Web.Models
{
    public class WSPCustomerDownloadSerializer
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; }
        public string IdNumber { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Residence { get; set; }
        public string AccountStatus { get; set; }
        public string Tariff { get; set; }
        public string Owner { get; set; }
        public string OwnerAddress { get; set; }
        public string WSP { get; set; }
        public string LastTransmitted { get; set; }
        public string TelemetryId { get; set; }
        public string MeterCode { get; set; }
        
        public List<BillingDownloadSerializer> Billings { get; set; }
    }
}
