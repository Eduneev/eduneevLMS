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
    public class EntityController : Controller
    {
        // GET: Entity
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult EntityUsers()
        {
            return View();
        }

        public ActionResult ReceiveEquipment()
        {
            return View();
        }

        public ActionResult Inventory()
        {
            return View();
        }

        public ActionResult Shipped()
        {
            return View();
        }

        public ActionResult Studios()
        {
            return View();
        }

        public ActionResult Billing()
        {
            return View();
        }

        [HttpGet]
        public string GetReceiversList()
        {
            SqlParameter[] FObj = new SqlParameter[1];

            FObj[0] = new SqlParameter("@UserID", SqlDbType.Int);
            FObj[0].Value = Convert.ToInt32(Session["USER_ID"]);
            DataTable ReceiversList = DAL.GetDataTable("GetReceiverListForEntity", FObj);

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(ReceiversList);
            return JSONString;
        }

        [HttpGet]
        public string GetRemotesList()
        {
            SqlParameter[] FObj = new SqlParameter[1];

            FObj[0] = new SqlParameter("@UserID", SqlDbType.Int);
            FObj[0].Value = Convert.ToInt32(Session["USER_ID"]);
            DataTable RemotesList = DAL.GetDataTable("GetRemotesListForEntity", FObj);

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(RemotesList);
            return JSONString;
        }


        [HttpPost]
        public void ReceiveEquipment(int ReceiverID)
        {
            EntityModel EntityObj = new EntityModel();
            SqlParameter[] SParam = new SqlParameter[1];

            SParam[0] = new SqlParameter("@ReceiverID", SqlDbType.Int);
            SParam[0].Value = ReceiverID ;

            try
            {
                EntityObj.ReceiveEquipment(SParam);
            }
            catch (Exception ex)
            {

            }
        }

        [HttpPost]
        public void ReceiveRemote(int RemoteID)
        {
            EntityModel EntityObj = new EntityModel();
            SqlParameter[] SParam = new SqlParameter[1];

            SParam[0] = new SqlParameter("@RemoteID", SqlDbType.Int);
            SParam[0].Value = RemoteID;

            try
            {
                EntityObj.ReceiveRemote(SParam);
            }
            catch (Exception ex)
            {

            }
        }

        [HttpGet]
        public string GetInvRemE()
        {
            SqlParameter[] FObj = new SqlParameter[1];

            FObj[0] = new SqlParameter("@UserID", SqlDbType.Int);
            FObj[0].Value = Convert.ToInt32(Session["USER_ID"]);
            DataTable InvRemList = DAL.GetDataTable("GetInvRemE", FObj);

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(InvRemList);
            return JSONString;
        }

        [HttpGet]
        public string GetInvRecE()
        {
            SqlParameter[] FObj = new SqlParameter[1];

            FObj[0] = new SqlParameter("@UserID", SqlDbType.Int);
            FObj[0].Value = Convert.ToInt32(Session["USER_ID"]);
            DataTable InvRecList = DAL.GetDataTable("GetInvRecE", FObj);

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(InvRecList);
            return JSONString;
        }

        [HttpGet]
        public string GetStudios()
        {
            SqlParameter[] FObj = new SqlParameter[1];

            FObj[0] = new SqlParameter("@UserID", SqlDbType.Int);
            FObj[0].Value = Convert.ToInt32(Session["USER_ID"]);
            DataTable Studios = DAL.GetDataTable("GetStudios", FObj);

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(Studios);
            return JSONString;
        }

        [HttpPost]
        public void SaveStudio(string StudioName, string StudioLocation, string Remarks, string ChannelName)
        {
            EntityModel ModelObj1 = new EntityModel();
            SqlParameter[] SParam = new SqlParameter[5];

            SParam[0] = new SqlParameter("@UserID", SqlDbType.Int);
            SParam[0].Value = Convert.ToInt32(Session["USER_ID"]);
            SParam[1] = new SqlParameter("@StudioName", SqlDbType.VarChar);
            SParam[1].Value = StudioName;
            SParam[2] = new SqlParameter("@StudioLocation", SqlDbType.VarChar);
            SParam[2].Value = StudioLocation;
            SParam[3] = new SqlParameter("@Remarks", SqlDbType.Text);
            SParam[3].Value = Remarks;
            SParam[4] = new SqlParameter("@ChannelName", SqlDbType.VarChar);
            SParam[4].Value = ChannelName;

            try
            {
                ModelObj1.CreateStudio(SParam);
            }
            catch (Exception ex)
            {

            }
        }
    }
}