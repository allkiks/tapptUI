using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAPPT.Web.Models
{
    public class GETWSPDashBoardDownloadSerializer
    {
        public int CustomerCount { get; set; }
        public int MeterCount { get; set; }
        public int TransmittingToday { get; set; }
        public int TransmittingYesterday { get; set; }
        public int NotTransmitting { get; set; }
        public decimal CumulativeConsumption { get; set; }
        public WeeklyTrasmittingConsumption TConsumption { get; set; }
        public WeeklyNotTrasmittingConsumption NConsumption { get; set; }
        public int PaidBills { get; set; }
        public int UnPaidBills { get; set; }
        public string TodaysEarnings { get; set; }
        public string TodaysConsumption { get; set; }
        public MonthlyConsumption MonthlyConsumption { get; set; }
    }
}
