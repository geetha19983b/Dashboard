using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DashBoard.Models
{
    public class ProgramCSLVM
    {
        public string Category { get; set; }
        public decimal? Categoryavg { get; set; }

        public List<CSL_table> TowerList { get; set; }


    }
    public class TowerSel
    {
        public string Tower { get; set; }
        public string TowerValue { get; set; }
    }
    
}