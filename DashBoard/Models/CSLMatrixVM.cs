using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DashBoard.Models
{
    public class CSLMatrixVM
    {
        public List<CSL_Summary> CSLSummary { get; set; }
        public List<CriticalServicelevelMatrix> CriticalSLM { get; set; }
    }
}