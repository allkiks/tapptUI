using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAPPT.Web.Models
{
    public class UtlitiesDownloadSerializer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhysicalAddress { get; set; }
        public string PostalAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string County { get; set; }
        public string KRAPin { get; set; }
        public int Count { get; set; }
        public int CountNotTransmitting { get; set; }
        public string CountMeters { get; set; }
        public string Consumption { get; set; }
        public string TodaysConsumption { get; set; }
        public string Bouquete { get; set; }
        public string Bill { get; set; }
    }
}
