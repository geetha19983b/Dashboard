using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DashBoard.Models
{
    public class CSLInferencesVM
    {
        public string Category { get; set; }

        public List<CLSList> CLSList { get; set; }

        public List<string> Categories { get; set; }

    }
    public class CLSList
    {
        public int? CommentID { get; set; }
        public string Comments { get; set; }
        public Nullable<bool> Approved { get; set; }
    }

}