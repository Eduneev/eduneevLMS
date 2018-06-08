using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyLMS.Controllers
{
    public class NavBarController : Controller
    {
        // GET: NavBar
        public ActionResult NavBar()
        {
            return View();
        }

        public ActionResult SignOut()
        {
            Session.Abandon();
            return RedirectToAction("Login", "Home");
        }
    }
}