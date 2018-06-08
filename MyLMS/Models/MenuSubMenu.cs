using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyLMS.Models
{
    public class MenuSubMenu
    {
        public Menu Menu { get; set; }
        public List<SubMenu> SubMenu { get; set; }
    }
}