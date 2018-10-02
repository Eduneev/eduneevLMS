using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
//using System.Web.Http.Cors;
using MyLMS.Models;
using Newtonsoft.Json;
using UtilityClass;

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

            //Workflow: Send in streamKey and MacAddr to get sessionId, pass in sessionId along with type to get stream
            SqlParameter[] SParam = new SqlParameter[1];
            SParam[0] = new SqlParameter("@SessionID", SqlDbType.Int);
            SParam[0].Value = sessionId;
            
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
            DataTable val = DAL.GetDataTable("GetAttendingStudents", SParam);
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
            SParam[0] = new SqlParameter("@RemoteID", SqlDbType.VarChar);
            SParam[0].Value = remoteId;
            SParam[1] = new SqlParameter("@SessionID", SqlDbType.Int);
            SParam[1].Value = sessionId;
            DataTable val = DAL.GetDataTable("GetStudentIdFromRemoteAllocation", SParam);
            int studentId = -1;
            studentId = Convert.ToInt32(Convert.IsDBNull(val.Rows[0]["StudentID"]) ? "-1" : val.Rows[0]["StudentID"]);

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
        
        [Route("api/SaveLastUsedCommand/{ClassRoomID:int}/{LastUsedCommand}")]
        [HttpGet]
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
            }
            else
            {
                result.ClassRoomId = -1;
            }

            return result;
        }
    }
}