using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAPPT.Web.Models
{
    public class BillingDownloadSerializer
    {
        public int Id { get; set; }
        public string DateCreated { get; set; }
        public string Amount { get; set; }
        public string MeterCode { get; set; }
        public decimal Consumption { get; set; }
        public string TotalAmount { get; set; }
        public string InvoiceNumber { get; set; }
        public string Balance { get; set; }
        public string PaymentStatus { get; set; }
        public string CustomerName { get; set; }
        public string AccountNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string PostalAddress { get; set; }
        public string Email { get; set; }
        public string WSP { get; set; }
        public string WSPPostalAddress { get; set; }
        public string WSPPhysicalAddress { get; set; }
        public string WSPPhoneNumber { get; set; }
        public string WSPEmail { get; set; }
        public string LogoUrl { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
    }
}
