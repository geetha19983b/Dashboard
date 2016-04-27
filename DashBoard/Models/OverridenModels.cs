using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DashBoard.Models
{
    [MetadataType(typeof(OnboardingHelper))]
    public partial class Onboarding { }

    public partial class OnboardingHelper
    {
       [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> OnboardingDate { get; set; }
       
    }
}