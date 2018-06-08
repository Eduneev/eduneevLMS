using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyLMS.Models
{
    public class QuestionOptions
    {
        public Question Question { get; set; }
        public List<Option> Options { get; set; }
    }
}