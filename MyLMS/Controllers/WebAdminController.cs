using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyLMS.Controllers
{
    public class WebAdminController : Controller
    {
        // GET: WebAdmin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult MenuMapper()
        {
            return View();
        }
    }
}