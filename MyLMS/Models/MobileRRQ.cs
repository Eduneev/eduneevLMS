using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyLMS.Models
{
    public class MobileRRQ
    {
        public int RRQID { get; set; }
        public string SubjectName { get; set; }
        public int NumQuestions { get; set; }
        public StudentModel Student { get; set; }
    }
}