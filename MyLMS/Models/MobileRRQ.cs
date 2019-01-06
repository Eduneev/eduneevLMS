using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyLMS.Models
{
    public class MobileRRQ
    {
        public int RRQID { get; set; }
        public int SessionID { get; set; }
        public int SubjectID { get; set; }
        public string RRQNo { get; set; }
        public int NumQuestions { get; set; }
    }
}