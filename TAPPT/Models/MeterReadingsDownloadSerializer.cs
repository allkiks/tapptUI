using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAPPT.Web.Models
{
    public class MeterReadingsDownloadSerializer
    {
        public int Id { get; set; }
        public string TimeTaken { get; set; }
        public string DaillyFowRate { get; set; }
        public string AccumulatedFlowRate { get; set; }
        public string BateryVoltage { get; set; }
        public string SignalStrength { get; set; }
        public string WaterPressure { get; set; }
        public string WaterTemperature { get; set; }
        public string MeterCode { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
    }
}
