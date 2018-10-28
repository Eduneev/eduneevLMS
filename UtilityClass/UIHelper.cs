using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UtilityClass
{
    class UIHelper
    {
        public static string RemoveHTMLTags(string goalText)
        {
            Regex reg = new Regex("<[^>]*>");
            string retStr = "";
            retStr = reg.Replace(goalText, "");
            return retStr;
        }

        public static string IgnoreSpecialChars(string anyText)
        {
            string retStr = "";
            retStr = anyText.Replace("&nbsp;", " ").Replace("&amp;", "&").Replace("\r\n?", "\n.").Replace("&gt;", ">").Replace("&lt;", "<");
            return retStr;
        }
    }
}

