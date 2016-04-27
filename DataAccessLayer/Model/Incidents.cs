using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Model
{
    public class Incidents
    {
        [Required]
        public int FeedID { get; set; }
        [Required]
        public string ParentIncident { get; set; }
        [Required]
        public string IncidentPriority { get; set; }
        [Required]
        public string IncServiceOffering { get; set; }
        [Required]
        public string IncAssignmentGroup { get; set; }
        [Required]
        public DateTime? Opened { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public int? ResponseSLA { get; set; }
        [Required]
        public int? ResolutionSLA { get; set; }
        [Required]
        public string TimeRemaining { get; set; }
        [Required]
        public string Tower { get; set; }
        [Required]
        public string AssignedTo { get; set; }
        [Required]
        public string Metal { get; set; }
        [Required]
        public string Comments { get; set; }
    }
}