﻿using MyLMS.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using UtilityClass;

namespace MyLMS.Controllers
{
    [SessionExpire]
    public class SessionMgmtController : Controller
    {
        // GET: SessionMgmt
        public ActionResult NewSession()
        {
            return View();
        }

        public ActionResult EditSession()
        {
            return View();
        }

        public ActionResult NewRRQSession()
        {
            return View();
        }

        public ActionResult ViewSessions()
        {
            return View();
        }

        public ActionResult ViewRRQSessions()
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
        public ActionResult NameFaceScreen()
        {
            return View();
        }
        public ActionResult TwoWayCall()
        {
            return View();
        }
        public ActionResult CreateRRQ()
        {
            return View();
        }

        public ActionResult EditRRQ(int id)
        {
            Session["RRQ_ID"] = id;
            Session["RRQ_ID_Display"] = id;
            return View();
        }

        public ActionResult AddQuestionsToRRQ(int id)
        {
            Session["RRQ_ID"] = id;
            return View();
        }

        public ActionResult RRQIntroduction(int id)
        {
            Session["RRQ_ID_Display"] = id;
            RRQData RRQInfoObj = new RRQData();
            RRQInfoObj.GetRRQInformation(id);
            ViewBag.RRQInfo = RRQInfoObj;
            return View();
        }

        public ActionResult DisplayRRQ()
        {
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

        public ActionResult ConnectedClassroom()
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
        public string GetStudio(int id)
        {
            SqlParameter[] FObj = new SqlParameter[1];
            FObj[0] = new SqlParameter("@SessionID", SqlDbType.Int);
            FObj[0].Value = id;
            DataTable StudiosList = DAL.GetDataTable("GetStudio", FObj);

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(StudiosList);
            return JSONString;
        }

        [HttpGet]
        public string GetTop10Students(int id)
        {
            SqlParameter[] RRQObj = new SqlParameter[1];
            RRQObj[0] = new SqlParameter("@RRQ_ID", SqlDbType.Int);
            RRQObj[0].Value = id;
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
            DataTable SessionsList = DAL.GetDataTable("GetSessions", FObj);

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
            Session["RRQ_ID"] = rrqID;
            return Convert.ToInt32(Session["RRQ_ID"].ToString());
        }

        [HttpGet]
        public Int32 GetSessionRRQ()
        {
            try
            {
                return Convert.ToInt32(Session["RRQ_ID"].ToString());
            }
            catch (Exception e)
            {
                return -1;
            }
        }

        [HttpGet]
        public string GetChannel(int id)
        {
            string channel = string.Empty;
            SqlParameter[] FObj = new SqlParameter[1];
            FObj[0] = new SqlParameter("@SessionID", SqlDbType.Int);
            FObj[0].Value = id;
            DataTable val = DAL.GetDataTable("GetChannel", FObj);
            if (val.Rows.Count > 0)
                channel = Convert.ToString(Convert.IsDBNull(val.Rows[0]["ChannelName"]) ? string.Empty : val.Rows[0]["ChannelName"]);
            return channel;
        }


        [HttpGet]
        public string GetStartedSessions(int id)
        {
            SqlParameter[] FObj = new SqlParameter[2];
            FObj[0] = new SqlParameter("@SubjectID", SqlDbType.Int);
            FObj[0].Value = id;
            FObj[1] = new SqlParameter("@UserID", SqlDbType.Int);
            FObj[1].Value = Convert.ToInt32(Session["USER_ID"].ToString());
            DataTable StartedSessionsList = DAL.GetDataTable("StartedSessionsList", FObj);

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(StartedSessionsList);
            return JSONString;
        }

        [HttpGet]
        [Route("SessionMgmt/GetSessionName/{SubjectID:int}/{CourseID:int}")]
        public string GetSessionName(int SubjectID, int CourseID)
        {
            string SessionName;
            SqlParameter[] TopicsObj = new SqlParameter[2];
            TopicsObj[0] = new SqlParameter("@SubjectID", SqlDbType.Int);
            TopicsObj[0].Value = SubjectID;
            TopicsObj[1] = new SqlParameter("@CourseID", SqlDbType.Int);
            TopicsObj[1].Value = CourseID;
            DataTable SessionNo = DAL.GetDataTable("GenerateSessionNo", TopicsObj);
            SessionName = SessionNo.Rows[0]["SessionName"].ToString();
            return SessionName.Trim();
        }

        [HttpPost]
        public void SaveSession(string SessionName, DateTime SessionDate, string StartTime, string EndTime, int StudioID, int ProgID, string ProgCode, int CourseID, string CourseCode, int SubjectID, string SubjectCode, string TopicID, int FacultyID, string PlannedCoverage)
        {
            SessionModel SessionObj1 = new SessionModel();
            SqlParameter[] SParam = new SqlParameter[12];

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
            SParam[11] = new SqlParameter("@PlannedCoverage", SqlDbType.Text);
            SParam[11].Value = PlannedCoverage;
            bool KeyUnique = false;
            string streamKey = string.Empty;
            // Create session stream key
            while (!KeyUnique)
            {
                streamKey = createRandomKey();
                SqlParameter[] FObj = new SqlParameter[1];
                FObj[0] = new SqlParameter("@StreamKey", SqlDbType.VarChar);
                FObj[0].Value = streamKey;
                DataTable z = DAL.GetDataTable("ValidateStreamKey", FObj);
                if (z.Rows.Count == 0)
                    KeyUnique = true;
            }
            SParam[10] = new SqlParameter("@Key", SqlDbType.VarChar);
            SParam[10].Value = streamKey;

            int SessionID = -1;
            try
            {
                string s = SessionObj1.SaveSession(SParam);
                try
                {
                    SessionID = Convert.ToInt32(s); 
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error converting created SessionID from string to int.");
                }
            }
            catch (Exception ex)
            {

            }

            SParam = new SqlParameter[1];
            SParam[0] = new SqlParameter("@UserID", SqlDbType.Int);
            SParam[0].Value = Convert.ToInt32(Session["USER_ID"]);
            DataTable val = DAL.GetDataTable("GetEntity", SParam);

            string EntityCode = string.Empty;
            string StreamSetName = string.Empty; 
            if (val.Rows.Count > 0)
            {
                EntityCode = Convert.ToString(Convert.IsDBNull(val.Rows[0]["EntityCode"]) ? string.Empty : val.Rows[0]["EntityCode"]);
                StreamSetName = Convert.ToString(Convert.IsDBNull(val.Rows[0]["StreamSetName"]) ? string.Empty : val.Rows[0]["StreamSetName"]);
            }

            Stream stream = new Stream();
            if (StreamSetName.Equals("Passthrough"))
                stream.SaveStream(SessionID, EntityCode, ProgCode, CourseCode, SubjectCode, false);
            else
                stream.SaveStream(SessionID, EntityCode, ProgCode, CourseCode, SubjectCode);

        }

        [HttpPost]
        public void EditSession(int SessionID, string SessionName, DateTime SessionDate, string StartTime, string EndTime, int StudioID, int ProgID, int CourseID, int SubjectID, int FacultyID, string PlannedCoverage, string StreamKey)
        {
            SessionModel SessionObj1 = new SessionModel();
            SqlParameter[] SParam = new SqlParameter[12];

            SParam[0] = new SqlParameter("@SessionID", SqlDbType.Int);
            SParam[0].Value = SessionID;
            SParam[1] = new SqlParameter("@SessionName", SqlDbType.VarChar);
            SParam[1].Value = SessionName;
            SParam[2] = new SqlParameter("@SessionDate", SqlDbType.DateTime);
            SParam[2].Value = SessionDate;
            SParam[3] = new SqlParameter("@StartTime", SqlDbType.VarChar);
            SParam[3].Value = StartTime;
            SParam[4] = new SqlParameter("@EndTime", SqlDbType.VarChar);
            SParam[4].Value = EndTime;
            SParam[5] = new SqlParameter("@StudioID", SqlDbType.Int);
            SParam[5].Value = StudioID;
            SParam[6] = new SqlParameter("@ProgID", SqlDbType.Int);
            SParam[6].Value = ProgID;
            SParam[7] = new SqlParameter("@CourseID", SqlDbType.Int);
            SParam[7].Value = CourseID;
            SParam[8] = new SqlParameter("@SubjectID", SqlDbType.Int);
            SParam[8].Value = SubjectID;
            SParam[9] = new SqlParameter("@FacultyID", SqlDbType.Int);
            SParam[9].Value = FacultyID;
            SParam[10] = new SqlParameter("@Key", SqlDbType.VarChar);
            SParam[10].Value = StreamKey;
            SParam[11] = new SqlParameter("@PlannedCoverage", SqlDbType.Text);
            SParam[11].Value = PlannedCoverage;
            SessionObj1.EditSession(SParam);
        }

        [HttpPost]
        public void DeleteSession(int SessionID)
        {
            SessionModel SessionObj1 = new SessionModel();
            SqlParameter[] SParam = new SqlParameter[2];

            SParam[0] = new SqlParameter("@SessionID", SqlDbType.Int);
            SParam[0].Value = SessionID;
            SParam[1] = new SqlParameter("@UserID", SqlDbType.Int);
            SParam[1].Value = Convert.ToInt32(Session["USER_ID"]);

            SessionObj1.DeleteSession(SParam);
        }

        [HttpGet]
        public string GetStream(int SessionID, int type=1)
        {
            string url = string.Empty;
            //Workflow: Send in streamKey and MacAddr to get sessionId, pass in sessionId along with type to get stream
            SqlParameter[] SParam = new SqlParameter[1];
            SParam[0] = new SqlParameter("@SessionID", SqlDbType.Int);
            SParam[0].Value = SessionID;

            DataTable keys = DAL.GetDataTable("GetStream", SParam);
            if (keys.Rows.Count > 0)
            {
                int index = 0;
                if (type > keys.Rows.Count)
                {
                    int[] types = new int[keys.Rows.Count];
                    for (int i = 0; i < keys.Rows.Count; i++)
                    {
                        types[i] = Convert.ToInt32(keys.Rows[i]["Type"]);
                    }

                    // We would need to loop again since we're not keeping track of the original array indices.
                    // However there would only be 1-3 streams per session, looping is not a big problem.
                    Array.Sort(types); // sort, since we are unsure that type is stored in ascending order in database
                    type = types[types.Length - 1];
                }
                for (int i = 0; i < keys.Rows.Count; i++)
                {
                    if (Convert.ToInt32(keys.Rows[i]["Type"]) == type)
                        index = i;
                }

                url = Convert.ToString(Convert.IsDBNull(keys.Rows[index]["Stream"]) ? string.Empty : keys.Rows[index]["Stream"]);

            }
            else
            {
                SParam[0].Value = 1; //get default stream
                DataTable keys2 = DAL.GetDataTable("GetStream", SParam);
                if (keys2.Rows.Count > 0)
                {
                    url = Convert.ToString(Convert.IsDBNull(keys2.Rows[0]["Stream"]) ? string.Empty : keys2.Rows[0]["Stream"]);
                }
            }

            return url;
        }

        [HttpGet]
        public string GetObsStream(int id)
        {
            return GetStream(id, -10);
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
        public string GetSessionBySessionID(int id)
        {
            SqlParameter[] SessionObj = new SqlParameter[2];
            SessionObj[0] = new SqlParameter("@SessionID", SqlDbType.Int);
            SessionObj[0].Value = id;
            SessionObj[1] = new SqlParameter("@UserID", SqlDbType.Int);
            SessionObj[1].Value = Convert.ToInt32(Session["USER_ID"]);
            DataTable SessionDetails = DAL.GetDataTable("GetSessionBySessionID", SessionObj);

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(SessionDetails);
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

        [HttpGet]
        public string GetRRQList()
        {
            SqlParameter[] FObj = new SqlParameter[1];
            FObj[0] = new SqlParameter("@USER_ID", SqlDbType.Int);
            FObj[0].Value = Convert.ToInt32(Session["USER_ID"]);
            DataTable RRQList = DAL.GetDataTable("GetRRQList", FObj);

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(RRQList);
            return JSONString;
        }

        [HttpPost]
        public void CreateNewRRQ(string RRQNo, int SessionID)
        {
            //int SessionID = Convert.ToInt32(Session["SessionID"]);

            SessionModel SessionObj3 = new SessionModel();
            SqlParameter[] RRQParam = new SqlParameter[2];
            RRQParam[0] = new SqlParameter("@SessionID", SqlDbType.Int);
            RRQParam[0].Value = SessionID;
            RRQParam[1] = new SqlParameter("@RRQNo", SqlDbType.VarChar);
            RRQParam[1].Value = RRQNo;
            try
            {
                SessionObj3.CreateRRQ(RRQParam);
            }
            catch (Exception ex)
            {

            }
        }

        [HttpPost]
        public void DeleteRRQ(int RRQ_ID)
        {
            //int SessionID = Convert.ToInt32(Session["SessionID"]);

            SessionModel SessionObj3 = new SessionModel();
            SqlParameter[] RRQParam = new SqlParameter[2];
            RRQParam[0] = new SqlParameter("@UserID", SqlDbType.Int);
            RRQParam[0].Value = Convert.ToInt32(Session["USER_ID"]);
            RRQParam[1] = new SqlParameter("@RRQ_ID", SqlDbType.Int);
            RRQParam[1].Value = RRQ_ID;
            try
            {
                SessionObj3.DeleteRRQ(RRQParam);
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
        public string GetRRQQuestions(int id) 
        {
            SqlParameter[] FObj = new SqlParameter[1];
            FObj[0] = new SqlParameter("@RRQ_ID", SqlDbType.Int);
            FObj[0].Value = id; //****************************DEFINE RRQ ID
            DataTable SessionsList = DAL.GetDataTable("GetRRQQuestions", FObj);

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(SessionsList);
            return JSONString;
        }

        [HttpGet]
        public string GetRRQQdetails(int id)
        {
            SqlParameter[] FObj = new SqlParameter[1];
            FObj[0] = new SqlParameter("@RRQ_ID", SqlDbType.Int);
            FObj[0].Value = id; //****************************DEFINE RRQ ID
            DataTable QuestionsPrcnt = DAL.GetDataTable("GetRRQQuestionsPrcnt", FObj);

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(QuestionsPrcnt);
            return JSONString;
        }

        public static string createRandomKey(int size=5)
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            char[] stringChars = new char[size];
            Random random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            string finalString = new String(stringChars);
            return finalString;
        }

        [HttpGet]
        public string GetDegreeOfDifficulty(int id)
        {
            SqlParameter[] FObj = new SqlParameter[1];
            FObj[0] = new SqlParameter("@RRQ_ID", SqlDbType.Int);
            FObj[0].Value = id; //****************************DEFINE RRQ ID;
            DataTable StudiosList = DAL.GetDataTable("GetDegreeOfDifficulty", FObj);
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(StudiosList);
            return JSONString;
        }

        [HttpGet]
        public string GetStudentsByResponse(int id, int OptionSeq)
        {
            SqlParameter[] FObj = new SqlParameter[3];
            FObj[0] = new SqlParameter("@RRQ_ID", SqlDbType.Int);
            FObj[0].Value = Convert.ToInt32(Session["RRQ_ID"].ToString());
            FObj[1] = new SqlParameter("@QID", SqlDbType.Int);
            FObj[1].Value = id;
            FObj[2] = new SqlParameter("@OptionSeq", SqlDbType.Int);
            FObj[2].Value = OptionSeq;
            DataTable StudentsDetails = DAL.GetDataTable("GetStudentsByResponse", FObj);
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(StudentsDetails);
            return JSONString;
        }

        [HttpGet]
        public string GetTop10FastestStudents(int id)
        {
            SqlParameter[] FObj = new SqlParameter[2];
            FObj[0] = new SqlParameter("@RRQ_ID", SqlDbType.Int);
            FObj[0].Value = Convert.ToInt32(Session["RRQ_ID"].ToString());
            FObj[1] = new SqlParameter("@QID", SqlDbType.Int);
            FObj[1].Value = id;

            DataTable StudentsDetails = DAL.GetDataTable("GetTop10FastestStudents", FObj);
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(StudentsDetails);
            return JSONString;
        }

            public string GetDashboardData(int id)
        {
            SqlParameter[] FObj = new SqlParameter[2];
            FObj[0] = new SqlParameter("@RRQ_ID", SqlDbType.Int);
            FObj[0].Value = Convert.ToInt32(Session["RRQ_ID"].ToString());
            FObj[1] = new SqlParameter("@QID", SqlDbType.Int);
            FObj[1].Value = id;

            try
            {
                DataTable DashboardData = DAL.GetDataTable("GetDashboardData", FObj);
                string JSONString = string.Empty;
                JSONString = JsonConvert.SerializeObject(DashboardData);
                return JSONString;
            }
            catch
            {
                return string.Empty;
            }
        }

        [HttpGet]
        public string GetDashboardOptionGraph(int id)
        {
            SqlParameter[] FObj = new SqlParameter[2];
            FObj[0] = new SqlParameter("@RRQ_ID", SqlDbType.Int);
            FObj[0].Value = Convert.ToInt32(Session["RRQ_ID"].ToString());
            FObj[1] = new SqlParameter("@QID", SqlDbType.Int);
            FObj[1].Value = id;
            DataTable DashboardGraphData = DAL.GetDataTable("GetDashboardOptionGraph", FObj);
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(DashboardGraphData);
            return JSONString;
        }


        //[HttpGet]
        //public string GetClassrooms()
        //{
        //    DataTable AllClassrooms = DAL.GetDataTable("GetAllClassrooms");
        //    string JSONString = string.Empty;
        //    JSONString = JsonConvert.SerializeObject(AllClassrooms);
        //    return JSONString;
        //}

        [HttpGet]
        public string GetCentersForEntity()
        {
            SqlParameter[] CenterSessionObj = new SqlParameter[1];
            CenterSessionObj[0] = new SqlParameter("@USER_ID", SqlDbType.Int);
            CenterSessionObj[0].Value = Convert.ToInt32(Session["USER_ID"]);
            DataTable CentersList = DAL.GetDataTable("GetCenterForEntity", CenterSessionObj);
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(CentersList);
            return JSONString;
        }

        public string GetCentersForSelectedEntity(int id)
        {
            SqlParameter[] CenterSessionObj = new SqlParameter[1];
            CenterSessionObj[0] = new SqlParameter("@EntityID", SqlDbType.Int);
            CenterSessionObj[0].Value = id;
            DataTable CentersList = DAL.GetDataTable("GetCenterForSelectedEntity", CenterSessionObj);
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(CentersList);
            return JSONString;
        }

        [HttpGet]
        public string GetStudentsByCenterID(int id)
        {
            SqlParameter[] FObj = new SqlParameter[1];
            FObj[0] = new SqlParameter("@CenterID", SqlDbType.Int);
            FObj[0].Value = id;
            DataTable StudentsList = DAL.GetDataTable("GetStudentsByCenterID", FObj);
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(StudentsList);
            return JSONString;
        }

        [HttpGet]
        [Route("SessionMgmt/GetStudentsAttendanceByCenterID/{CenterID:int}/{SessionID:int}")]
        public string GetStudentsAttendanceByCenterID(int CenterID, int SessionID)
        {
            SqlParameter[] FObj = new SqlParameter[2];
            FObj[0] = new SqlParameter("@CenterID", SqlDbType.Int);
            FObj[0].Value = CenterID;
            FObj[1] = new SqlParameter("@SessionID", SqlDbType.Int);
            FObj[1].Value = SessionID;
            DataTable StudentsList = DAL.GetDataTable("GetStudentsAttendanceByCenterID", FObj);
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(StudentsList);
            return JSONString;
        }

        [HttpGet]
        public string GetStudentsByID(int id)
        {
            SqlParameter[] StudObj = new SqlParameter[1];
            StudObj[0] = new SqlParameter("@StudentID", SqlDbType.Int);
            StudObj[0].Value = id;
            DataTable Student = DAL.GetDataTable("GetStudentsByID", StudObj);
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(Student);
            return JSONString;
        }

        public string StartRecording(string url, string EntityCode)
        {
            // Send a command to Streaming Server with url to start recording
            String ServerIP = "18.224.156.241";
            String ServerUrl = "https://sanat:sanat@" + ServerIP + ":8086/livestreamrecord?" +
                "app=" + EntityCode + "&streamname=" + url + "&action=startRecording";
            Uri u = new Uri(ServerUrl);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(u);
            String response = request.GetResponse().ToString();
            return response;
        }

        public string StopRecording(string url, string EntityCode)
        {
            String ServerIP = "18.224.156.241";
            String ServerUrl = "https://sanat:sanat@" + ServerIP + ":8086/livestreamrecord?" +
                "app=" + EntityCode + "&streamname=" + url + "&action=stopRecording";
            Uri u = new Uri(ServerUrl);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(u);
            String response = request.GetResponse().ToString();
            return response;
        }

        public string GetConnectedClassrooms(int id)
        {
            SqlParameter[] SessObj = new SqlParameter[2];
            SessObj[0] = new SqlParameter("@UserID", SqlDbType.Int);
            SessObj[0].Value = Convert.ToInt32(Session["USER_ID"]);
            SessObj[1] = new SqlParameter("@SessionID", SqlDbType.Int);
            SessObj[1].Value = id;
            DataTable ClS = DAL.GetDataTable("GetConnectedClassrooms", SessObj);
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(ClS);
            return JSONString;
        }

        /* Poll information */
        [HttpPost]
        public void SavePollResponse(int SessionID, int CenterID, int OptionSeq)
        {
            SqlParameter[] SessObj = new SqlParameter[3];
            PollModel poll = new PollModel();

            SessObj[0] = new SqlParameter("@SessionID", SqlDbType.Int);
            SessObj[0].Value = SessionID;
            SessObj[1] = new SqlParameter("@CenterID", SqlDbType.Int);
            SessObj[1].Value = CenterID;
            SessObj[2] = new SqlParameter("OptionSeq", SqlDbType.Int);
            SessObj[2].Value = OptionSeq;

            try
            {
                poll.SavePollResponse(SessObj);
            }
            catch (Exception ex)
            {

            }
        }
    }

    public class SessionExpireAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;
            // check  sessions here
            if (ctx.Session != null && ctx.Session["USER_ID"] == null)
            {
                if (ctx.Request.IsAuthenticated)
                    FormsAuthentication.SignOut();

                filterContext.Result = new RedirectResult("~/Home/Login");
                return;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}