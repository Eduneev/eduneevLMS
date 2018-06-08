using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyLMS.Models
{
    public class Menu
    {
        public int MenuID { get; set; }
        public string MenuName { get; set; }
        public string MenuIconClass { get; set; }
        public string MenuURL { get; set; }
        public int MenuOrder { get; set; }
    }
}