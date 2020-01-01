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
    [SessionExpire]
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

        public ActionResult CreateCenterUser()
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
            DataTable InvRemList = DAL.GetDataTable("GetEntityRemoteInventory", FObj);

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

        [HttpGet]
        public string GetEntityUserList()
        {
            SqlParameter[] FObj = new SqlParameter[1];

            FObj[0] = new SqlParameter("@UserID", SqlDbType.Int);
            FObj[0].Value = Convert.ToInt32(Session["USER_ID"]);
            DataTable Studios = DAL.GetDataTable("GetEntitysUsers", FObj);

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(Studios);
            return JSONString;
        }

        [HttpGet]
        public string GetCenterUserList(int ID)
        {
            SqlParameter[] FObj = new SqlParameter[1];
            FObj[0] = new SqlParameter("@CenterID", SqlDbType.Int);
            FObj[0].Value = ID;
            DataTable EntityUserList = DAL.GetDataTable("GetCenterUserList", FObj);

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(EntityUserList);
            return JSONString;
        }

        [HttpGet]
        public string GetCenterCoordinatorRole()
        {
            DataTable EntityUserList = DAL.GetDataTable("GetCenterCoordinatorRole");

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(EntityUserList);
            return JSONString;
        }

        [HttpPost]
        public void AddCenterUser(int CenterID, string UserName, string Password, string FullName, string EmailID, string Mobile, int RoleID)
        {
            EntityModel EntObj = new EntityModel();
            SqlParameter[] SParam = new SqlParameter[8];

            SParam[0] = new SqlParameter("@UserID", SqlDbType.Int);
            SParam[0].Value = Convert.ToInt32(Session["USER_ID"]);
            SParam[1] = new SqlParameter("@UserName", SqlDbType.VarChar);
            SParam[1].Value = UserName;
            SParam[2] = new SqlParameter("@Password", SqlDbType.VarChar);
            SParam[2].Value = EDHelper.EncryptTripleDES(Password);
            SParam[3] = new SqlParameter("@FullName", SqlDbType.VarChar);
            SParam[3].Value = FullName;
            SParam[4] = new SqlParameter("@EmailID", SqlDbType.VarChar);
            SParam[4].Value = EmailID;
            SParam[5] = new SqlParameter("@Mobile", SqlDbType.VarChar);
            SParam[5].Value = Mobile;
            SParam[6] = new SqlParameter("@RoleID", SqlDbType.VarChar);
            SParam[6].Value = RoleID;
            SParam[7] = new SqlParameter("@CenterID", SqlDbType.VarChar);
            SParam[7].Value = CenterID;

            try
            {
                EntObj.AddCenterUser(SParam);
            }
            catch (Exception ex)
            {

            }
        }

        [HttpGet]
        [Route("Entity/CheckEntityAndCenter/{EntityName}/{CenterName}")]
        public string CheckEntityAndCenter(string EntityName, string CenterName)
        {
            SqlParameter[] FObj = new SqlParameter[3];

            FObj[0] = new SqlParameter("@UserID", SqlDbType.Int);
            FObj[0].Value = Convert.ToInt32(Session["USER_ID"]);
            FObj[1] = new SqlParameter("@EntityName", SqlDbType.VarChar);
            FObj[1].Value = EntityName;
            FObj[2] = new SqlParameter("@CenterName", SqlDbType.VarChar);
            FObj[2].Value = CenterName;

            DataTable entity = DAL.GetDataTable("CheckEntityAndCenter", FObj);

            string JSONString = string.Empty;
            if (entity.Rows.Count > 0)
                JSONString = "Success";
            else
                JSONString = "Failure";

            return JSONString;
        }

        [HttpGet]
        [Route("Entity/CheckEntity/{EntityName}")]
        public string CheckEntity(string EntityName)
        {
            SqlParameter[] FObj = new SqlParameter[3];

            FObj[0] = new SqlParameter("@UserID", SqlDbType.Int);
            FObj[0].Value = Convert.ToInt32(Session["USER_ID"]);
            FObj[1] = new SqlParameter("@EntityName", SqlDbType.VarChar);
            FObj[1].Value = EntityName;

            DataTable entity = DAL.GetDataTable("CheckEntity", FObj);

            string JSONString = string.Empty;
            if (entity.Rows.Count > 0)
                JSONString = "Success";
            else
                JSONString = "Failure";

            return JSONString;
        }
    }
}