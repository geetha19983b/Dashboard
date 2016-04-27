using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DashBoard.Models
{
    public class PrblmMangVM
    {
        public List<tblProblemSummary> ProblmSummaries { get; set; }
        public List<tblWeeklyProblemmanagement> WeeklyPrblmManag { get; set; }

       
    }
}