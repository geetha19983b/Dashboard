using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DashBoard.Models
{
    public class RCAVM
    {
      
        
      
        public string ParentIncident { get; set; }
        
        public string Parent_IncidentPriority { get; set; }

        public string Number { get; set; }
        public string RCA_State { get; set; }

        public string ProblemTicket { get; set; }

        public string ProblemTicketOpenDate { get; set; }

        public int? SLAMet { get; set; }
    }
}