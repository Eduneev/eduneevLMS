using MyLMS.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UtilityClass;

namespace MyLMS.Controllers
{
    public class SessionMgmtController : Controller
    {
        // GET: SessionMgmt
        public ActionResult NewSession()
        {
            return View();
        }

        public ActionResult ViewSessions()
        {
            return View();
        }

        public ActionResult StudentAttendance()
        {
            return View();
        }

        public ActionResult SelectSession()
        {
            return View();
        }

        public ActionResult RRQIntroduction(int ID)
        {
            RRQData RRQInfoObj = new RRQData();
            RRQInfoObj.GetRRQInformation(ID);
            ViewBag.RRQInfo = RRQInfoObj;
            return View();
        }

        public ActionResult DisplayRRQ()
        {
            return View();
        }

        public ActionResult Dashboard(int id)
        {
            DashboardData DashboardObj = new DashboardData();
            DashboardObj.GetRespPrcnt(id);            
            ViewBag.VBDashboard = DashboardObj;
            return View();
        }

        public ActionResult RRQDashboard(int id)
        {
            DashboardData DashboardObj = new DashboardData();
            DashboardObj.GetRespPrcnt(id);
            ViewBag.VBDashboard = DashboardObj;
            return View();
        }

        public ActionResult AddRRQ()
        {
            Session["SessionID"] = Request.QueryString["ID"];
            return View();
        }

        public ActionResult ViewRRQ()
        {
            return View();
        }

        public ActionResult AddQuestionsToRRQ()
        {
            return View();
        }

        [HttpGet]
        public string GetStudios()
        {
            SqlParameter[] FObj = new SqlParameter[1];
            FObj[0] = new SqlParameter("@UserID", SqlDbType.Int);
            FObj[0].Value = Convert.ToInt32(Session["USER_ID"]);
            DataTable StudiosList = DAL.GetDataTable("GetStudios", FObj);

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(StudiosList);
            return JSONString;
        }

        [HttpGet]
        public string GetTop10Students()
        {
            SqlParameter[] RRQObj = new SqlParameter[1];
            RRQObj[0] = new SqlParameter("@RRQ_ID", SqlDbType.Int);
            RRQObj[0].Value = 1;
            DataTable StudentsList = DAL.GetDataTable("GetTop10Students", RRQObj);

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(StudentsList);
            return JSONString;
        }

        [HttpGet]
        public string GetSessions()
        {
            SqlParameter[] FObj = new SqlParameter[1];
            FObj[0] = new SqlParameter("@UserID", SqlDbType.Int);
            FObj[0].Value = Convert.ToInt32(Session["USER_ID"]);
            DataTable SessionsList = DAL.GetDataTable("GetSessions");

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(SessionsList);
            return JSONString;
        }

        [HttpPost]
        public Int32 SetSession(int SessionID)
        {
            Session["CurrSessionID"] = SessionID;
            
            return Convert.ToInt32(Session["CurrSessionID"].ToString());
        }

        [HttpGet]
        public Int32 GetSession()
        {
            try
            {
                return Convert.ToInt32(Session["CurrSessionID"].ToString());
            }
            catch (Exception e)
            {
                return -1;
            }
        }

        [HttpPost]
        public Int32 SetSessionRRQ(int rrqID)
        {
            Session["RRQID"] = rrqID;
            return Convert.ToInt32(Session["RRQID"].ToString());
        }

        [HttpGet]
        public Int32 GetSessionRRQ()
        {
            try
            {
                return Convert.ToInt32(Session["RRQID"].ToString());
            }
            catch (Exception e)
            {
                return -1;
            }
        }

        [HttpGet]
        public string GetStartedSessions(int id)
        {
            SqlParameter[] FObj = new SqlParameter[1];
            FObj[0] = new SqlParameter("@SubjectID", SqlDbType.Int);
            FObj[0].Value = id;
            DataTable StartedSessionsList = DAL.GetDataTable("StartedSessionsList", FObj);

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(StartedSessionsList);
            return JSONString;
        }

        [HttpPost]
        public void SaveSession(string SessionName, DateTime SessionDate, string StartTime, string EndTime, int StudioID, int ProgID, int CourseID, int SubjectID, string TopicID, int FacultyID, string PlannedCoverage)
        {
            SessionModel SessionObj1 = new SessionModel();
            SqlParameter[] SParam = new SqlParameter[11];

            SParam[0] = new SqlParameter("@SessionName", SqlDbType.VarChar);
            SParam[0].Value = SessionName;
            SParam[1] = new SqlParameter("@SessionDate", SqlDbType.DateTime);
            SParam[1].Value = SessionDate;
            SParam[2] = new SqlParameter("@StartTime", SqlDbType.VarChar);
            SParam[2].Value = StartTime;
            SParam[3] = new SqlParameter("@EndTime", SqlDbType.VarChar);
            SParam[3].Value = EndTime;
            SParam[4] = new SqlParameter("@StudioID", SqlDbType.Int);
            SParam[4].Value = StudioID;
            SParam[5] = new SqlParameter("@ProgID", SqlDbType.Int);
            SParam[5].Value = ProgID;
            SParam[6] = new SqlParameter("@CourseID", SqlDbType.Int);
            SParam[6].Value = CourseID;
            SParam[7] = new SqlParameter("@SubjectID", SqlDbType.Int);
            SParam[7].Value = SubjectID;
            SParam[8] = new SqlParameter("@FacultyID", SqlDbType.Int);
            SParam[8].Value = FacultyID;
            SParam[9] = new SqlParameter("@CreatedBy", SqlDbType.Int);
            SParam[9].Value = Convert.ToInt32(Session["USER_ID"]);

            // Create session stream key
            string streamKey = createRandomKey();
            SParam[10] = new SqlParameter("@Key", SqlDbType.VarChar);
            SParam[10].Value = streamKey;
            try
            {
                SessionObj1.SaveSession(SParam);
            }
            catch (Exception ex)
            {

            }
        }

        [HttpGet]
        public string GetSessionsForCenter()
        {
            SqlParameter[] CenterSessionObj = new SqlParameter[1];
            CenterSessionObj[0] = new SqlParameter("@UserID", SqlDbType.Int);
            CenterSessionObj[0].Value = Convert.ToInt32(Session["USER_ID"]);
            DataTable SessionsList = DAL.GetDataTable("GetSessionsForCenter", CenterSessionObj);

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(SessionsList);
            return JSONString;
        }

        [HttpGet]
        public string GetCenterNameFromSession(int id)
        {
            SqlParameter[] CenterNameObj = new SqlParameter[1];
            CenterNameObj[0] = new SqlParameter("@CenterID", SqlDbType.Int);
            CenterNameObj[0].Value = id;

            DataTable list = DAL.GetDataTable("GetCenterName", CenterNameObj);

            if (list.Rows.Count == 0)
            {
                Debug.WriteLine("Something went wrong while getting center name!!");
                return "center";
            }
            string centerName = list.Rows[0]["CenterName"].ToString();
            return centerName;
        }

        [HttpPost]
        public void StartStopSession(int SessionID, string Status)
        {
            // Send call to Node server to create session
            SessionModel SessionObj2 = new SessionModel();
            SqlParameter[] SParam = new SqlParameter[3];
            SParam[0] = new SqlParameter("@SessionID", SqlDbType.Int);
            SParam[0].Value = SessionID;
            SParam[1] = new SqlParameter("@UserID", SqlDbType.Int);
            SParam[1].Value = Convert.ToInt32(Session["USER_ID"]);
            SParam[2] = new SqlParameter("@Status", SqlDbType.VarChar);
            SParam[2].Value = Status;
            try
            {
                SessionObj2.StartStopSession(SParam);
            }
            catch (Exception ex)
            {

            }
        }

        [HttpPost]
        public void CreateRRQ(string RRQNo, DateTime ActiveFromDate, DateTime ActiveToDate)
        {
            int SessionID = Convert.ToInt32(Session["SessionID"]);

            SessionModel SessionObj3 = new SessionModel();
            SqlParameter[] RRQParam = new SqlParameter[4];
            RRQParam[0] = new SqlParameter("@SessionID", SqlDbType.Int);
            RRQParam[0].Value = SessionID;
            RRQParam[1] = new SqlParameter("@RRQNo", SqlDbType.Int);
            RRQParam[1].Value = RRQNo;
            RRQParam[2] = new SqlParameter("@StartFrom", SqlDbType.Int);
            RRQParam[2].Value = ActiveFromDate;
            RRQParam[3] = new SqlParameter("@EndDate", SqlDbType.Int);
            RRQParam[3].Value = ActiveToDate;
            try
            {
                SessionObj3.CreateRRQ(RRQParam);
            }
            catch (Exception ex)
            {

            }
        }

        [HttpGet]
        public string GetSessionsRRQ(int id)
        {
            SqlParameter[] FObj = new SqlParameter[1];
            FObj[0] = new SqlParameter("@SessionID", SqlDbType.Int);
            FObj[0].Value = id;
            DataTable RRQList = DAL.GetDataTable("GetSessionsRRQ", FObj);

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(RRQList);
            return JSONString;
        }

        [HttpGet]
        public string GetRRQQuestions() 
        {
            SqlParameter[] FObj = new SqlParameter[1];
            FObj[0] = new SqlParameter("@RRQ_ID", SqlDbType.Int);
            FObj[0].Value = 1; //****************************DEFINE RRQ ID
            DataTable SessionsList = DAL.GetDataTable("GetRRQQuestions", FObj);

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(SessionsList);
            return JSONString;
        }

        [HttpGet]
        public string GetRRQQdetails()
        {
            SqlParameter[] FObj = new SqlParameter[1];
            FObj[0] = new SqlParameter("@RRQ_ID", SqlDbType.Int);
            FObj[0].Value = 1; //****************************DEFINE RRQ ID
            DataTable QuestionsPrcnt = DAL.GetDataTable("GetRRQQuestionsPrcnt", FObj);

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(QuestionsPrcnt);
            return JSONString;
        }

        private string createRandomKey()
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            char[] stringChars = new char[8];
            Random random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            string finalString = new String(stringChars);
            return finalString;
        }
    }
}