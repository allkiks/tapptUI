using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAPPT.Web.Models
{
    public class GetAdminDashboardDownloadSerializer
    {
        public int WSPCount { get; set; }
        public string TodaysConsumption { get; set; }
        public int MeterCount { get; set; }
        public int Transmitting { get; set; }
        public int NotTransmitting { get; set; }
        public int TransmittingLast24Hours { get; set; }
        public int TransmittingCurrent24Hours { get; set; }
    }
}
