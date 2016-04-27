using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DashBoard.Models
{
    public class BarChartVM
    {
        public string IncidentNo { get; set; }
        public string ShortDesc { get; set; }

        public string Priority { get; set; }

        public string State { get; set; }
        
        public DateTime? Opened { get; set; }

        public string Metal { get; set; }
        public string Tower { get; set; }
    }
}