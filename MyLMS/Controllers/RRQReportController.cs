using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyLMS.Models;

namespace MyLMS.Controllers
{
    public class RRQReportController : Controller
    {
        public ActionResult RRQReport()
        {
            return View();
        }

        public ActionResult RRQDashboard(int id)
        {
            DashboardData DashboardObj = new DashboardData();
            Session["RRQ_ID_Display"] = id;
            DashboardObj.GetRespPrcnt(id);
            ViewBag.VBDashboard = DashboardObj;
            return View();
        }

    }
}