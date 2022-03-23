using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAPPT.Web.Models
{
    public class QueryConsumptionDownloadSerializer
    {
        public int Id { get; set; }
        public string MeterCode { get; set; }
        public string LastTransmitted { get; set; }
        public string GeoCordinates { get; set; }
        public string AccumulatedFlowRate { get; set; }
        public string Revenue { get; set; }
    }
}
