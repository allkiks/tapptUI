using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAPPT.Web.Models
{
    public class TodaysRevenueDownloadSerializer
    {
        public int Id { get; set; }
        public int WSPId { get; set; }
        public int ZoneId { get; set; }

        public int meterCode { get; set; }

        public int lastTransmitted { get; set; }

        public int accumulatedFlowRate { get; set; }

        public int bill { get; set; }
    }
}
