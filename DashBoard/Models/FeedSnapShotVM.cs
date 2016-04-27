using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DashBoard.Models
{
    public class FeedSnapShotVM
    {
        public string ParentIncident { get; set; }
        public string IncidentPriority { get; set; }
        public Nullable<System.DateTime> Opened { get; set; }
        public string State { get; set; }

        public string Tower { get; set; }

        public int? ResolutionSLA { get; set; }
        
    }
}