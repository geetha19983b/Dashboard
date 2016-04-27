using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccessLayer.Model
{
    public class Tasks
    {
        public string Number { get; set; }

        public string Priority { get; set; }

        public string ServiceOffering { get; set; }

        public string AssignedTo { get; set; }

        public string ItState { get; set; }

        public string Metal { get; set; }
        
        public int? ResponseSLA { get; set; }
        
        public int? ResolutionSLA { get; set; }

        public string Comments { get; set; }


    }
}