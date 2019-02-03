using MyLMS.Models;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using UtilityClass;

namespace MyLMS.Controllers
{
    public class OrganisationController : Controller
    {
        // GET: Organisation
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Entity()
        {
            return View();
        }

        public ActionResult CreateEntityUser()
        {
            return View();
        }

        public ActionResult ShippingReceiver()
        {
            return View();
        }

        public ActionResult ShippingRemote()
        {
            return View();
        }
        public ActionResult CreateOrgUser()
        {
            return View();
        }

        [HttpPost]
        public void CreateEntity(string EntityName, string EntityCode, string ManagerName, string Email, string Mobile, string Landline, string Address)
        {
            OrganisationModel OrgObj = new OrganisationModel();            
            SqlParameter[] SParam = new SqlParameter[8];

            SParam[0] = new SqlParameter("@UserID", SqlDbType.Int);
            SParam[0].Value = Convert.ToInt32(Session["USER_ID"]);
            SParam[1] = new SqlParameter("@EntityName", SqlDbType.VarChar);
            SParam[1].Value = EntityName;
            SParam[2] = new SqlParameter("@EntityCode", SqlDbType.VarChar);
            SParam[2].Value = EntityCode;
            SParam[3] = new SqlParameter("@ManagerName", SqlDbType.VarChar);
            SParam[3].Value = ManagerName;
            SParam[4] = new SqlParameter("@Email", SqlDbType.VarChar);
            SParam[4].Value = Email;
            SParam[5] = new SqlParameter("@Mobile", SqlDbType.VarChar);
            SParam[5].Value = Mobile;
            SParam[6] = new SqlParameter("@Landline", SqlDbType.VarChar);
            SParam[6].Value = Landline;
            SParam[7] = new SqlParameter("@Address", SqlDbType.Text);
            SParam[7].Value = Address;

            try
            {
                OrgObj.CreateEntity(SParam);
            }
            catch (Exception ex)
            {

            }
        }

        [HttpGet]
        public string GetOrgUserList()
        {
            DataTable OrgUserList = DAL.GetDataTable("GetOrgUserList");

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(OrgUserList);
            return JSONString;
        }

        [HttpGet]
        public string GetEntityList()
        {
            SqlParameter[] FObj = new SqlParameter[1];
            FObj[0] = new SqlParameter("@UserID", SqlDbType.Int);
            FObj[0].Value = Convert.ToInt32(Session["USER_ID"]);
            DataTable EntityList = DAL.GetDataTable("GetEntityList", FObj);

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(EntityList);
            return JSONString;
        }

        [HttpGet]
        public string GetEntityUserList(int ID)
        {
            SqlParameter[] FObj = new SqlParameter[1];
            FObj[0] = new SqlParameter("@EntityID", SqlDbType.Int);
            FObj[0].Value = ID;
            DataTable EntityUserList = DAL.GetDataTable("GetEntityUserList", FObj);

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(EntityUserList);
            return JSONString;
        }

        [HttpGet]
        public string GetEntityAdminRole()
        {
            DataTable EntityUserList = DAL.GetDataTable("GetEntityAdminRole");

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(EntityUserList);
            return JSONString;
        }

        [HttpPost]
        public void AddOrgUser(string UserName, string Password, string FullName, string EmailID, string Mobile)
        {
            OrganisationModel OrgObj = new OrganisationModel();
            SqlParameter[] SParam = new SqlParameter[6];

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

            try
            {
                OrgObj.AddOrgUser(SParam);
            }
            catch (Exception ex)
            {

            }
        }

        [HttpPost]
        public void UpdatePassword(string UserID, string Password)
        {
            OrganisationModel OrgObj = new OrganisationModel();
            SqlParameter[] SParam = new SqlParameter[2];

            SParam[0] = new SqlParameter("@UserID", SqlDbType.Int);
            SParam[0].Value = UserID;
            SParam[1] = new SqlParameter("@Password", SqlDbType.VarChar);
            SParam[1].Value = EDHelper.EncryptTripleDES(Password);

            try
            {
                OrgObj.UpdatePassword(SParam);
            }
            catch (Exception ex)
            {

            }
        }


        [HttpPost]
        public void AddEntityUser(int EntityID, string UserName, string Password, string FullName, string EmailID, string Mobile, int RoleID)
        {
            OrganisationModel OrgObj = new OrganisationModel();
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
            SParam[7] = new SqlParameter("@EntityID", SqlDbType.VarChar);
            SParam[7].Value = EntityID;

            try
            {
                OrgObj.AddEntityUser(SParam);
            }
            catch (Exception ex)
            {

            }
        }

        [HttpPost]
        public void EditUser(string UserName, string Password, string FullName, string EmailID, string Mobile, int RoleID, int UserID)
        {
            OrganisationModel OrgObj = new OrganisationModel();
            SqlParameter[] SParam = new SqlParameter[7];

            SParam[0] = new SqlParameter("@UserID", SqlDbType.Int);
            SParam[0].Value = UserID;
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
            SParam[6] = new SqlParameter("@RoleID", SqlDbType.Int);
            SParam[6].Value = RoleID;

            try
            {
                OrgObj.EditUser(SParam);
            }
            catch (Exception ex)
            {

            }
        }

        [HttpGet]
        public string GetReceiversList(int ID)
        {
            SqlParameter[] FObj = new SqlParameter[2];

            FObj[0] = new SqlParameter("@UserID", SqlDbType.Int);
            FObj[0].Value = Convert.ToInt32(Session["USER_ID"]);
            FObj[1] = new SqlParameter("@EntityID", SqlDbType.Int);
            FObj[1].Value = ID;
            DataTable ReceiversList = DAL.GetDataTable("GetReceiverList", FObj);

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(ReceiversList);
            return JSONString;
        }

        [HttpGet]
        public string GetRemotesList(int ID)
        {
            SqlParameter[] FObj = new SqlParameter[2];

            FObj[0] = new SqlParameter("@UserID", SqlDbType.Int);
            FObj[0].Value = Convert.ToInt32(Session["USER_ID"]);
            FObj[1] = new SqlParameter("@EntityID", SqlDbType.Int);
            FObj[1].Value = ID;
            DataTable RemotesList = DAL.GetDataTable("GetRemoteList", FObj);

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(RemotesList);
            return JSONString;
        }

        [HttpPost]
        public void SaveShippedReceiver(string ReceiverSerialNo, string ReceiverModel, int EntityID)
        {
            OrganisationModel OrgObj = new OrganisationModel();
            SqlParameter[] SParam = new SqlParameter[4];

            SParam[0] = new SqlParameter("@UserID", SqlDbType.Int);
            SParam[0].Value = Convert.ToInt32(Session["USER_ID"]);
            SParam[1] = new SqlParameter("@ReceiverSerialNo", SqlDbType.VarChar);
            SParam[1].Value = ReceiverSerialNo;
            SParam[2] = new SqlParameter("@ReceiverModel", SqlDbType.VarChar);
            SParam[2].Value = ReceiverModel;
            SParam[3] = new SqlParameter("@EntityID", SqlDbType.VarChar);
            SParam[3].Value = EntityID;
           
            try
            {
                OrgObj.SaveShippedReceiver(SParam);
            }
            catch (Exception ex)
            {

            }
        }

        [HttpPost]
        public void SaveShippedRemote(int ShipmentAmount, string RemoteModel, int EntityID)
        {
            OrganisationModel OrgObj = new OrganisationModel();
            SqlParameter[] SParam = new SqlParameter[4];

            SParam[0] = new SqlParameter("@UserID", SqlDbType.Int);
            SParam[0].Value = Convert.ToInt32(Session["USER_ID"]);
            SParam[1] = new SqlParameter("@ShipmentAmount", SqlDbType.VarChar);
            SParam[1].Value = ShipmentAmount;
            SParam[2] = new SqlParameter("@RemoteModel", SqlDbType.VarChar);
            SParam[2].Value = RemoteModel;
            SParam[3] = new SqlParameter("@EntityID", SqlDbType.VarChar);
            SParam[3].Value = EntityID;

            try
            {
                OrgObj.SaveShippedRemote(SParam);
            }
            catch (Exception ex)
            {

            }
        }
    }
}