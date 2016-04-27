using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DashBoard.Models
{
    public class IncidentSumVM
    {
        public List<Incident_summary> IncidentSummary { get; set; }
        public List<UnResolvedIncident> UnResolvIncident { get; set; }
    }
}