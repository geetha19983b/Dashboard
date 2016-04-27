using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DashBoard.Models
{
    public class IncidentInterVM
    {
        public IList<Priority> AvailablePriority { get; set; }
        public IList<Priority> SelectedPriority { get; set; }
        public PostedPriorities PostedPriorities { get; set; }

        public Priority priority { get; set; }
    }
    public class PostedPriorities
    {
        public string[] PriorityIDs { get; set; }
    }
}