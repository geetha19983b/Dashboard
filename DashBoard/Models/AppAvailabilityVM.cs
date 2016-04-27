using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DashBoard.Models
{
    public class AppAvailabilityVM
    {


        public string ServiceOffering { get; set; }
        public Nullable<double> Average { get; set; }
        public string Metal { get; set; }
        public string Tower { get; set; }


    }
}