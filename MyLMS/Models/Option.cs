using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyLMS.Models
{
    public partial class Option
    {
        public int OptionID { get; set; }
        public int QID { get; set; }
        public int OptionSeq { get; set; }
        public string OptionText { get; set; }
        public int Mark { get; set; }
        public bool IsCorrect { get; set; }

    }
}