﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyLMS.Models
{
    public partial class Question
    {
        public int QID { get; set; }
        public string QUES_NO { get; set; }
        public int QTypeID { get; set; }
        public int QTagID { get; set; }
        public string QuestionText { get; set; }
        public bool IsCompulsory { get; set; }
        public string TagText { get; set; }
        public int QTime { get; set; }
    }
}