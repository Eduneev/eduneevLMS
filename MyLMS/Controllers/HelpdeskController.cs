using MyLMS.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UtilityClass;

namespace MyLMS.Controllers
{
    public class HelpdeskController : Controller
    {
        // GET: Helpdesk
        public ActionResult NewTicket()
        {
            return View();
        }

        public ActionResult ViewMyTickets()
        {
            return View();
        }

        public ActionResult TicketDetails(int ID)
        {
            Session["TicketID"] = ID;
            return View();
        }

        [HttpPost]
        public void SaveNewTicket(string Subject, string TicketContent)
        {
            HelpdeskModel ModelObj1 = new HelpdeskModel();
            SqlParameter[] SParam = new SqlParameter[3];
            SParam[0] = new SqlParameter("@UserID", SqlDbType.Int);
            SParam[0].Value = Convert.ToInt32(Session["USER_ID"]);
            SParam[1] = new SqlParameter("@Subject", SqlDbType.NVarChar);
            SParam[1].Value = Subject;
            SParam[2] = new SqlParameter("@TicketContent", SqlDbType.NVarChar);
            SParam[2].Value = TicketContent;
            try
            {
                ModelObj1.SaveNewTicket(SParam);
            }
            catch (Exception ex)
            {

            }
        }

        [HttpGet]
        public string GetMyTickets()
        {
            SqlParameter[] FObj = new SqlParameter[1];
            FObj[0] = new SqlParameter("@UserID", SqlDbType.Int);
            FObj[0].Value = Convert.ToInt32(Session["USER_ID"]);
            DataTable TicketsList = DAL.GetDataTable("GetTicketsByUserID", FObj);

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(TicketsList);
            return JSONString;
        }

        [HttpGet]
        public string GetTicketByTicketID()
        {
            SqlParameter[] FObj = new SqlParameter[2];
            FObj[0] = new SqlParameter("@UserID", SqlDbType.Int);
            FObj[0].Value = Convert.ToInt32(Session["USER_ID"]);
            FObj[1] = new SqlParameter("@TicketID", SqlDbType.Int);
            FObj[1].Value = Convert.ToInt32(Session["TicketID"]);
            DataTable TicketInfo = DAL.GetDataTable("GetTicketByID", FObj);

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(TicketInfo);

            return JSONString;
        }

        [HttpGet]
        public string GetTicketDetails()
        {
            SqlParameter[] DObj = new SqlParameter[1];
            DObj[0] = new SqlParameter("@TicketID", SqlDbType.Int);
            DObj[0].Value = Convert.ToInt32(Session["TicketID"]);
            DataTable TicketsDetails = DAL.GetDataTable("GetTicketsDetails", DObj);

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(TicketsDetails);

            return JSONString;
        }
    }
}