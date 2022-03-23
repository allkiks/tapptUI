using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAPPT.Web.Models
{
    public class NotTransmittingMetersDownloadSerializer
    {
        public int Id { get; set; }
        public string MeterCode { get; set; }
        public string LastTransmitted { get; set; }
        public string GeoCordinates { get; set; }
        public decimal AccumulatedFlowRate { get; set; }
        public string BateryVoltage { get; set; }
        public string SignalStrength { get; set; }
        public string IMSI { get; set; }
    }
}
