using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Model
{
    public class BatchTrackerModel
    {
       public string JobGroup {get;set;}

        public string JobName { get; set; }

        public string ExpectedStartTime { get; set; }

        public string ActualStartTime { get; set; }

        public string ExpectedEndTime { get; set; }

        public string ActualEndTime { get; set; }
        
        public string Status { get; set; }
        
        public string Comments { get; set; }
        
        public DateTime RunDate { get; set; }

        
    }   
        
        
    
}
