﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAPPT.Web.Models
{
    public class QueryMetersViewModel
    {
        public int WSPId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public QueryStatus QueryStatus { get; set; }
    }
}
