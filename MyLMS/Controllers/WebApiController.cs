using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Net;
using System.Web.Http;
using MyLMS.Models;
using Newtonsoft.Json;
using UtilityClass;
//using System.Web.Http.Cors;

namespace MyLMS.Controllers
{
    //[EnableCors(origins: "http://localhost:55082", headers: "*", methods: "*")]
    public class WebApiController : ApiController
    {
        /** Get session Id
         *  Parameters: Auth ID
         *              StreamKey
         *  Output: Session ID
         **/ 
        [Route("api/getSession/{streamKey}/{auth}")]
        [HttpGet]
        public int GetSession(string auth, string streamKey)
        {
            int SessionId=-1;
            SqlParameter[] SParam = new SqlParameter[2];
            SParam[0] = new SqlParameter("@Auth", SqlDbType.VarChar);
            SParam[0].Value = auth;
            SParam[1] = new SqlParameter("@StreamKey", SqlDbType.VarChar);
            SParam[1].Value = streamKey;

            string JSONString = string.Empty;

            DataTable keys = DAL.GetDataTable("GetSession", SParam);
            if (keys.Rows.Count == 1)
            {
                SessionId = Convert.ToInt32(keys.Rows[0]["SessionID"]);
            }
            else
            {
                Debug.WriteLine("0 or Too many sessions contain this StreamKey or Auth key");

                return -1;
            }
            return SessionId;
        }

        [Route("api/{sessionId:int}/getStream/{type:int}")]
        [HttpGet]
        public string GetStream(int sessionId, int type)
        {
            string url = string.Empty;
            SessionMgmtController s = new SessionMgmtController();
            url = s.GetStream(sessionId, type);

            // construct VLC string
            //string VlcCommand = string.Empty;

            //VlcCommand = "vlc.exe --no-sout-video --one-instance --embedded-video --key-record=  -I --disable-qt " + url;
            //return VlcCommand;
            return url;
        }

        [Route("api/getVLCCommand")]
        [HttpGet]
        public string[] GetVLCCommand()
        {
            string[] VlcCommand = new string[2];
            VlcCommand[0] = "\"C:\\Program Files (x86)\\VideoLAN\\VLC\\vlc.exe\"";
            //VlcCommand[0] = "\"C:\\Program Files (x86)\\2WayLive\\2WayLivePlayer\\2WLPlayer.exe\"";
            //VlcCommand[0] = "\"C:\\Program Files (x86)\\2WayLive\\2WL\\2WLPlayer.exe\""; 
            VlcCommand[1] = "--rtsp-tcp --no-video-title-show --sub-filter=marq --marq-marquee=\"@2WayLive\" --marq-x 29 --marq-y 350 --marq-size=20 --marq-opacity=55  --video-title-timeout=2000 --no-video-deco --no-sout-video --network-caching=300 --one-instance --high-priority --embedded-video --key-record=  -I --disable-qt ";
            return VlcCommand;
        }

        [Route("api/{sessionId:int}/getRRQ")]
        [HttpGet]
        public RRQ GetRRQ(int sessionId)
        {
            // Add an active bit. When RRQ finish, turn active bit off. 
            SqlParameter[] SParam = new SqlParameter[2];
            SParam[0] = new SqlParameter("@SessionID", SqlDbType.Int);
            SParam[0].Value = sessionId;
            DataTable val = DAL.GetDataTable("GetSessionsRRQ", SParam);
            RRQ r = new Models.RRQ();

            for (int i = 0; i < val.Rows.Count; i++)
            {
                r.RRQId = Convert.ToInt32(Convert.IsDBNull(val.Rows[i]["RRQ_ID"]) ? "-1" : val.Rows[i]["RRQ_ID"]);
                r.RRQNo = Convert.ToString(Convert.IsDBNull(val.Rows[i]["RRQNo"]) ? "-1" : val.Rows[i]["RRQNo"]);
                r.SessionId = Convert.ToInt32(Convert.IsDBNull(val.Rows[i]["SessionId"]) ? "-1" : val.Rows[i]["SessionId"]);
            }
            return r;
        }

        [Route("api/{sessionId:int}/{rrqId:int}/getQid")]
        [HttpGet]
        public string GetQid(int sessionId, int rrqId)
        {
            string qId = "";

            // TODO complete function

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(qId);
            return JSONString;
        }

        [Route("api/{sessionId:int}/{centerId:int}/getAttendingStudents")]
        [HttpGet]
        public StudentModel[] GetAttendingStudents(int sessionId, int centerId)
        {
            SqlParameter[] SParam = new SqlParameter[2];
            SParam[0] = new SqlParameter("@SessionID", SqlDbType.Int);
            SParam[0].Value = sessionId;
            SParam[1] = new SqlParameter("CenterID", SqlDbType.Int);
            SParam[1].Value = centerId;
            DataTable val = DAL.GetDataTable("GetStudentsAttendanceByCenterID", SParam);
            StudentModel[] result = new StudentModel[val.Rows.Count];

            for (int i = 0; i < val.Rows.Count; i++)
            {
                StudentModel s = new StudentModel();
                s.StudentID = Convert.ToInt32(Convert.IsDBNull(val.Rows[i]["StudentID"]) ? "-1" : val.Rows[i]["StudentID"]);
                s.StudentName = Convert.ToString(Convert.IsDBNull(val.Rows[i]["StudentName"]) ? "-1" : val.Rows[i]["StudentName"]);
                s.StudentImageURL = Convert.ToString(Convert.IsDBNull(val.Rows[i]["StudentImageURL"]) ? "-1" : val.Rows[i]["StudentImageURL"]);
                result[i] = s;
            }
            return result;
        }

        [Route("api/{sessionId:int}/{rrqId:int}/saveRRQResponse/{QId:int}/{remoteId}/{response}")]
        [HttpGet]
        public bool SaveRRQResponse(int sessionId, int rrqId, int QId, string remoteId, string response)
        {
            // first get the studentId
            // Also, can this be done in one go rather than first get the studenId?
            SqlParameter[] SParam = new SqlParameter[2];
            SParam[0] = new SqlParameter("@RemoteNum", SqlDbType.VarChar);
            SParam[0].Value = remoteId;
            SParam[1] = new SqlParameter("@SessionID", SqlDbType.Int);
            SParam[1].Value = sessionId;
            DataTable val = DAL.GetDataTable("GetStudentIdFromRemoteAllocation", SParam);
            int studentId = -1;
            if (val.Rows.Count > 0)
                studentId = Convert.ToInt32(Convert.IsDBNull(val.Rows[0]["StudentID"]) ? "-1" : val.Rows[0]["StudentID"]);
            else
                Debug.WriteLine("Error in retrieving student");

            int optionSeq = -1;
            if (response.Equals("A"))
                optionSeq = 1;
            else if (response.Equals("B"))
                optionSeq = 2;
            else if (response.Equals("C"))
                optionSeq = 3;
            else if (response.Equals("D"))
                optionSeq = 4;
            else if (response.Equals("E"))
                optionSeq = 5;
            else if (response.Equals("F"))
                optionSeq = 6;

            if (studentId != -1 && optionSeq != -1)
            {
                if (QuestionBankController.SaveQuestionResponse(rrqId, QId, studentId, optionSeq))
                    return true;
            }
            return false;
        }
        
        [Route("api/SaveLastUsedCommand")]
        [HttpPost]
        public string SaveLastUsedCommand(int ClassRoomID, string LastUsedCommand)
        {
            string result = String.Empty;
            SqlParameter[] SParam = new SqlParameter[2];
            SParam[0] = new SqlParameter("@ClassRoomID", SqlDbType.Int);
            SParam[0].Value = ClassRoomID;
            SParam[1] = new SqlParameter("@LastUsedCommand", SqlDbType.VarChar);
            SParam[1].Value = LastUsedCommand;

            DataTable val = DAL.GetDataTable("SaveLastUsedCommand", SParam);
            result = Convert.ToString(Convert.IsDBNull(val.Rows[0]["results"]) ? "Failure" : val.Rows[0]["results"]);
            return result;
        }

        [Route("api/GetLastUsedCommand/{ClassRoomID:int}")]
        [HttpGet]
        public string GetLastUsedCommand(int ClassRoomID)
        {
            string result = String.Empty;

            SqlParameter[] SParam = new SqlParameter[1];
            SParam[0] = new SqlParameter("@ClassRoomID", SqlDbType.Int);
            SParam[0].Value = ClassRoomID;

            DataTable val = DAL.GetDataTable("GetLastUsedCommand", SParam);
            if (val.Rows.Count > 0)
                result = Convert.ToString(Convert.IsDBNull(val.Rows[0]["LastUsedCommand"]) ? "Failure" : val.Rows[0]["LastUsedCommand"]);
            else
                result = "Failure";
            return result;
        }

        [Route("api/GetSessionForRRQ/{RRQID:int}")]
        [HttpGet]
        public string GetSessionForRRQ(int RRQID)
        {
            string result = String.Empty;

            SqlParameter[] SParam = new SqlParameter[1];
            SParam[0] = new SqlParameter("@RRQ_ID", SqlDbType.Int);
            SParam[0].Value = RRQID;

            DataTable val = DAL.GetDataTable("GetSessionForRRQ", SParam);
            if (val.Rows.Count > 0)
                result = Convert.ToString(Convert.IsDBNull(val.Rows[0]["SessionID"]) ? "-1" : val.Rows[0]["SessionID"]);
            else
                result = "-1";
            return result;
        }

        [Route("api/GetClassroom/{auth}")]
        [HttpGet]
        public ClassRoomModel GetClassroom(string auth)
        {
            ClassRoomModel result = new Models.ClassRoomModel();

            SqlParameter[] SParam = new SqlParameter[1];
            SParam[0] = new SqlParameter("@Auth", SqlDbType.VarChar);
            SParam[0].Value = auth;

            DataTable val = DAL.GetDataTable("GetClassRoom", SParam);
            if (val.Rows.Count > 0)
            {
                result.ClassRoomId = Convert.ToInt32(Convert.IsDBNull(val.Rows[0]["ClassRoomID"]) ? "-1" : val.Rows[0]["ClassRoomID"]);
                result.ClassRoomName = Convert.ToString(Convert.IsDBNull(val.Rows[0]["ClassRoomName"]) ? "-1" : val.Rows[0]["ClassRoomName"]);
                result.CenterId = Convert.ToInt32(Convert.IsDBNull(val.Rows[0]["CenterID"]) ? "-1" : val.Rows[0]["CenterID"]);
                result.LastUsedCommand = Convert.ToString(Convert.IsDBNull(val.Rows[0]["LastUsedCommand"]) ? "-1" : val.Rows[0]["LastUsedCommand"]);
                result.CenterName = Convert.ToString(Convert.IsDBNull(val.Rows[0]["CenterName"]) ? "-1" : val.Rows[0]["CenterName"]);
            }
            else
            {
                result.ClassRoomId = -1;
            }

            return result;
        }

        // ------Mobile RRQ section

        public static string SenderID = "TWLRRQ";
        public static string ProfileID = "718461";
        public static string OTPKey = "0106t55i00XwQM6mqHgVC6MccfhVOe";
        public static string Message = "2WayLive. Your OTP is: ";
        public static string OTPbaseURL = "http://promo.strantech.co.in/api/pushsms.php?usr=" + ProfileID +
            "&key=" + OTPKey + "&sndr=" + SenderID + "&";

        [Route("api/RegisterStudent")]
        [HttpPost]
        public String RegisterStudent(string StudentRegNo, string Password)
        {
            SqlParameter[] SParam = new SqlParameter[2];
            string result = string.Empty;
            SParam[0] = new SqlParameter("@Code", SqlDbType.VarChar);
            SParam[0].Value = StudentRegNo;
            SParam[1] = new SqlParameter("@Password", SqlDbType.VarChar);
            SParam[1].Value = Password;

            DataTable val = DAL.GetDataTable("CreateMobileStudent", SParam);
            if (val.Rows.Count > 0)
                result = Convert.ToString(val.Rows[0]["StudentName"]);
            return result;
        }

        [Route("api/SendOTP")]
        [HttpPost]
        public Boolean SendOTP(string StudentRegNo, string Mobile)
        {
            string OTP = string.Empty;
            Random r = new Random();
            for (int i = 0; i < 4; i++)
            {
                OTP += r.Next(10);
            }

            SqlParameter[] SParam = new SqlParameter[3];
            SParam[0] = new SqlParameter("@Code", SqlDbType.VarChar);
            SParam[0].Value = StudentRegNo;
            SParam[1] = new SqlParameter("@Mobile", SqlDbType.VarChar);
            SParam[1].Value = Mobile;
            SParam[2] = new SqlParameter("@OTP", SqlDbType.VarChar);
            SParam[2].Value = OTP;

            DataTable val = DAL.GetDataTable("VerifyStudentMobile", SParam);
            if (val.Rows.Count > 0)
            {
                string OTPUrl = OTPbaseURL + "ph=" + Mobile + "&text=" + Message + OTP + "&rpt=1";
                Uri u = new Uri(OTPUrl);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(u);
                request.GetResponse();
                return true;
            }

            return false;
        }

        [Route("api/ConfirmOTP")]
        [HttpPost]
        public Boolean ConfirmOTP(string StudentRegNo, string Mobile, string OTP)
        {
            SqlParameter[] SParam = new SqlParameter[3];
            SParam[0] = new SqlParameter("@Code", SqlDbType.VarChar);
            SParam[0].Value = StudentRegNo;
            SParam[1] = new SqlParameter("@Mobile", SqlDbType.VarChar);
            SParam[1].Value = Mobile;
            SParam[2] = new SqlParameter("@OTP", SqlDbType.VarChar);
            SParam[2].Value = OTP;

            DataTable val = DAL.GetDataTable("ValidateOTP", SParam);
            if (val.Rows.Count > 0)
                return true;

            return false;
        }

        [Route("api/GetRRQDetails/{StudentRegNo}/{RRQID:int}")]
        [HttpGet]
        public MobileRRQ GetRRQDetails(string StudentRegNo, int RRQID)
        {
            MobileRRQ mobile = new MobileRRQ();
            SqlParameter[] SParam = new SqlParameter[2];
            SParam[0] = new SqlParameter("@Code", SqlDbType.VarChar);
            SParam[0].Value = StudentRegNo;
            SParam[1] = new SqlParameter("RRQID", SqlDbType.VarChar);
            SParam[1].Value = RRQID;

            DataTable val = DAL.GetDataTable("GetMobileRRQ", SParam);
            if (val.Rows.Count>0)
            {
                mobile.RRQID = RRQID;
                mobile.SubjectName = Convert.ToString(Convert.IsDBNull(val.Rows[0]["SubjectName"]) ? "-1" : val.Rows[0]["SubjectName"]);
                mobile.NumQuestions = Convert.ToInt32(Convert.IsDBNull(val.Rows[0]["NumQuestions"]) ? "-1" : val.Rows[0]["NumQuestions"]);

                StudentModel s = new StudentModel();
                s.StudentID = Convert.ToInt32(Convert.IsDBNull(val.Rows[0]["StudentID"]) ? "-1" : val.Rows[0]["StudentID"]);
                s.StudentName = Convert.ToString(Convert.IsDBNull(val.Rows[0]["StudentName"]) ? "-1" : val.Rows[0]["StudentName"]);
                mobile.Student = s;
            }

            return mobile;
        }

        [Route("api/SaveMobileRRQResponse")]
        [HttpPost]
        public bool SaveMobileRRQResponse(int StudentID, int RRQID, int QuesNo, string Response)
        {

            int OptionSeq = -1;
            if (Response.Equals("A"))
                OptionSeq = 1;
            else if (Response.Equals("B"))
                OptionSeq = 2;
            else if (Response.Equals("C"))
                OptionSeq = 3;
            else if (Response.Equals("D"))
                OptionSeq = 4;
            else if (Response.Equals("E"))
                OptionSeq = 5;
            else if (Response.Equals("F"))
                OptionSeq = 6;

            if (OptionSeq != -1)
            {
                return QuestionBankController.SaveMobileQuestionResponse(RRQID, QuesNo, StudentID, OptionSeq);
            }
            return false;
        }

        [Route("api/AuthenticateSupportChat/{Response}")]
        [HttpGet]
        public bool AuthenticateSupportChat(string Response)
        {
            if (Response.Equals("2WayLive"))
                return true;
            return false;
        }

        //---Server Allocation
        [Route("api/GetRRQServer/{SessionID:int}")]
        [HttpGet]
        public string GetRRQServer(int SessionID)
        {
            // Determine entity and retrieve IP
            string result = String.Empty;
            SqlParameter[] SParam = new SqlParameter[1];
            SParam[0] = new SqlParameter("@SessionID", SqlDbType.Int);
            SParam[0].Value = SessionID;

            DataTable val = DAL.GetDataTable("GetRRQServer", SParam);
            result = Convert.ToString(Convert.IsDBNull(val.Rows[0]["Address"]) ? "Failure" : val.Rows[0]["Address"]);
            if(!result.Equals("Failure"))
            {
                // RRQ Server is on port 3000
                result = "ws://" + result + ":3000";
            } 

            return result;
        }

    }
}