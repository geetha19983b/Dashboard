//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DashBoard.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Onboarding
    {
        public int ResourceId { get; set; }
        public string EmployeeName { get; set; }
        public string LanId { get; set; }
        public string ResourceLocationType { get; set; }
        public Nullable<bool> CurrentEmployee { get; set; }
        public Nullable<System.DateTime> OnboardingDate { get; set; }
        public string Comments { get; set; }
        public Nullable<System.DateTime> OffboardingDate { get; set; }
        public string Tower { get; set; }
    }
}