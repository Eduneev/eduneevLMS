using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyLMS.Models
{
    public class RRQDetails
    {
        public int RRQID { get; set; }
        public string RRQNo { get; set; }
        public List<Dictionary<Int32,Option>> Questions { get; set; }
        public int TotalMarks { get; set; }
    }

    public class StudentReport
    {
        public int StudentID { get; set; }
        public string StudentName { get; set; }
        public List<Dictionary<Int32,Option>> Responses { get; set; }
        public int TotalMarks { get; set; }
    }

}