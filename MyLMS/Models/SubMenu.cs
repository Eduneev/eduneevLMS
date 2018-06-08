using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyLMS.Models
{
    public class SubMenu
    {
        public int SubMenuID { get; set; }
        public string SubMenuName { get; set; }
        public string SubMenuIconClass { get; set; }
        public string SubMenuURL { get; set; }
        public int SubMenuOrder { get; set; }
    }
}