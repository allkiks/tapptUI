﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAPPT.Web.Models
{
    public class QueriedMetersDownloadSerializer
    {
        public int Id { get; set; }
        public string MeterCode { get; set; }
        public string LastTransmitted { get; set; }
        public string Longitude{ get; set; }
        public string Lattitude { get; set; }
        public string AccumulatedFlowRate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string Bill{ get; set; }
    }
}
