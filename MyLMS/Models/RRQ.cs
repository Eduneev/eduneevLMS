using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyLMS.Models
{
    public class RRQ
    {
        public int RRQId { get; set; }
        public int SessionId { get; set; }
        public string RRQNo { get; set; }
        public string StartFrom { get; set; }
        public string EndDate { get; set; }
    }
}

