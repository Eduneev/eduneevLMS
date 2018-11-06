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
    public class CenterMgmtController : Controller
    {
        // GET: CenterMgmt
        public ActionResult CreateCenter()
        {
            return View();
        }

        public ActionResult ViewClassrooms()
        {
            return View();
        }

        public ActionResult CenterActivityLogs()
        {
            return View();
        }
        public ActionResult RRQResponseEntry()
        {
            return View();
        }
        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult ViewCenters()
        {
            return View();
        }

        public ActionResult ViewSessions()
        {
            return View();
        }

        [HttpPost]
        public void SaveCenter(string CenterName, string CenterCode, string Email, string Landline1, string Landline2, string Mobile, string Address, string PinCode)
        {
            CenterModel CentObj1 = new CenterModel();
            SqlParameter[] SParam = new SqlParameter[9];

            SParam[0] = new SqlParameter("@CenterName", SqlDbType.VarChar);
            SParam[0].Value = CenterName;
            SParam[1] = new SqlParameter("@CenterCode", SqlDbType.VarChar);
            SParam[1].Value = CenterCode;
            SParam[2] = new SqlParameter("@Email", SqlDbType.VarChar);
            SParam[2].Value = Email;
            SParam[3] = new SqlParameter("@Landline1", SqlDbType.VarChar);
            SParam[3].Value = Landline1;
            SParam[4] = new SqlParameter("@Landline2", SqlDbType.VarChar);
            SParam[4].Value = Landline2;
            SParam[5] = new SqlParameter("@Mobile", SqlDbType.VarChar);
            SParam[5].Value = Mobile;
            SParam[6] = new SqlParameter("@Address", SqlDbType.VarChar);
            SParam[6].Value = Address;
            SParam[7] = new SqlParameter("@PinCode", SqlDbType.VarChar);
            SParam[7].Value = PinCode;
            SParam[8] = new SqlParameter("@UserID", SqlDbType.Int);
            SParam[8].Value = Convert.ToInt32(Session["USER_ID"]);
            try
            {
                CentObj1.SaveCenter(SParam);
            }
            catch (Exception ex)
            {

            }
        }

        [HttpPost]
        public void SaveClassroom(string ClassRoomName, int CenterID, int SittingCapacity)
        {
            CenterModel CentObj1 = new CenterModel();
            SqlParameter[] SParam = new SqlParameter[4];
            SParam[0] = new SqlParameter("@ClassRoomName", SqlDbType.VarChar);
            SParam[0].Value = ClassRoomName;
            SParam[1] = new SqlParameter("@CenterID", SqlDbType.Int);
            SParam[1].Value = CenterID;
            SParam[2] = new SqlParameter("@SittingCapacity", SqlDbType.Int);
            SParam[2].Value = SittingCapacity;
            SParam[3] = new SqlParameter("@LastUsedCommand", SqlDbType.Int);
            SParam[3].Value = "";
            try
            {
                CentObj1.SaveClassRoom(SParam);
            }
            catch (Exception ex)
            {
            }
        }


        [HttpGet]
        public string GetCenters()
        {
            SqlParameter[] FObj = new SqlParameter[1];
            FObj[0] = new SqlParameter("@USER_ID", SqlDbType.Int);
            FObj[0].Value = Convert.ToInt32(Session["USER_ID"]);
            DataTable CentersList = DAL.GetDataTable("GetCenters", FObj);

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(CentersList);
            return JSONString;
        }

        [HttpGet]
        public string GetClassroomsForCenter(int id)
        {
            SqlParameter[] FObj = new SqlParameter[1];
            FObj[0] = new SqlParameter("@CenterID", SqlDbType.Int);
            FObj[0].Value = id;
            DataTable CentersList = DAL.GetDataTable("GetClassroomByCenterID", FObj);

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(CentersList);
            return JSONString;
        }

        [HttpGet]
        public int GetAccountID()
        {
            int AccountID;
            SqlParameter[] FObj = new SqlParameter[1];
            FObj[0] = new SqlParameter("@UserID", SqlDbType.Int);
            FObj[0].Value = Convert.ToInt32(Session["USER_ID"]);
            DataTable DtAccountID = DAL.GetDataTable("GetUserAccountID", FObj);
            AccountID = Convert.ToInt32(DtAccountID.Rows[0]["Column1"].ToString());
            return AccountID;
        }
    }
}