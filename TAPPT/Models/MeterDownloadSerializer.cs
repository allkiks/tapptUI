﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAPPT.Web.Models
{
    public class MeterDownloadSerializer
    {
        public int Id { get; set; }
        public string MeterCode { get; set; }
        public string IMSI { get; set; }
        public string IMENumber { get; set; }
        public string SignalStrength { get; set; }
        public string SIMCardNumber { get; set; }
        public string AccountNumber { get; set; }
        public int CustomerAccountId { get; set; }
        public string AccountStatus { get; set; }
        public string County { get; set; }
        public string WSP { get; set; }
        public string Tariff { get; set; }
        public string Owner { get; set; }
        public string OwnerAddress { get; set; }
        public string AccumulatedFlowRate { get; set; }
        public string BateryVoltage { get; set; }
        public string Humidity { get; set; }
        public string WaterPressure { get; set; }
        public string WaterTemperature { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string MeterStatus { get; set; }
        public string TodaysFlow { get; set; }
        public string LastDate { get; set; }
        public List<MeterReadingsDownloadSerializer> Readings { get; set; }
    }
}
