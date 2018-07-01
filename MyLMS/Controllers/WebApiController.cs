﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MyLMS.Models;
using Newtonsoft.Json;
using UtilityClass;

namespace MyLMS.Controllers
{
    public class WebApiController : ApiController
    {
        /** Get session Id
         *  Parameters: MAC ID
         *  Output: Session ID
         **/ 
        [Route("api/getSession/{streamKey}/{macid}")]
        [HttpGet]
        public string GetSession(string macid, string streamKey)
        {
            // TODO: add MacAddr to classroom and get the session id based on if Macaddress matches
            int SessionId=-1;
            SqlParameter[] SParam = new SqlParameter[2];
            SParam[0] = new SqlParameter("@MACAddr", SqlDbType.VarChar);
            SParam[0].Value = macid;
            SParam[1] = new SqlParameter("@StreakKey", SqlDbType.VarChar);
            SParam[1].Value = streamKey;

            string JSONString = string.Empty;

            DataTable keys = DAL.GetDataTable("GetSession", SParam);
            if (keys.Rows.Count == 1)
            {
                SessionId = Convert.ToInt32(keys.Rows[0]["SessionID"]);
            }
            else
            {
                Debug.WriteLine("0 or Too many sessions contain this StreamKey or MacAddr");
                
                JSONString = JsonConvert.SerializeObject(-1);
                return JSONString;
            }
            // Current Logic
            // Link classroom with session. When classroom joins session, this value gets set.
            // Current logic. Link classroom with MACAddr. Each classroom has a macaddr, which is set when classroom registers.
            // When api is called, we check is macaddr exists in CenterName database and then retrieve the session Id associated
            //DataTable rooms = DAL.GetDataTable("getSession")

            JSONString = JsonConvert.SerializeObject(SessionId);
            return JSONString;
        }

        [Route("api/{sessionId:int}/getStream/{type:int}")]
        [HttpGet]
        public string GetStream(int sessionId, int type)
        {
            string url = string.Empty;

            //Workflow: Send in streamKey and MacAddr to get sessionId, pass in sessionId along with type to get stream
            SqlParameter[] SParam = new SqlParameter[2];
            SParam[0] = new SqlParameter("@SessionID", SqlDbType.Int);
            SParam[0].Value = sessionId;
            SParam[1] = new SqlParameter("@Type", SqlDbType.Int);
            SParam[1].Value = type;

            DataTable keys = DAL.GetDataTable("GetStream", SParam);
            if (keys.Rows.Count > 0)
            {
                url = Convert.ToString(Convert.IsDBNull(keys.Rows[0]["Stream"]) ? string.Empty : keys.Rows[0]["Stream"]);
            }
            else
            {
                SParam[1].Value = 1; //get default stream
                DataTable keys2 = DAL.GetDataTable("GetStream", SParam);
                if (keys2.Rows.Count > 0)
                {
                    url = Convert.ToString(Convert.IsDBNull(keys2.Rows[0]["Stream"]) ? string.Empty : keys2.Rows[0]["Stream"]);
                }
            }

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(url);
            return JSONString;
        }


        //TODO: Need to check the key somehow. Then if the key exists and the macaddr is also correct, get back session. 
        // Start the RRQ App based on session and get the session active RRQID.

        [Route("api/{sessionId:int}/checkKey/{key}")]
        [HttpGet]
        public string CheckKey(int sessionId, string key)
        {
            Boolean check=false;

            // TODO: Check if Key in session if correct and if so, return True
            SqlParameter[] SParam = new SqlParameter[2];
            SParam[0] = new SqlParameter("@SessionID", SqlDbType.Int);
            SParam[0].Value = sessionId;
            SParam[1] = new SqlParameter("@Key", SqlDbType.VarChar);
            SParam[1].Value = key;
            DataTable keys = DAL.GetDataTable("GetKey", SParam);
            if (keys.Rows.Count > 0)
            {
                for (int i = 0; i < keys.Rows.Count; i++)
                {
                    string temp = Convert.ToString(Convert.IsDBNull(keys.Rows[i]["StreamKey"]) ? "-1" : keys.Rows[i]["StreamKey"]);
                    if (string.Equals(temp, key))
                        check = true;
                    else
                        check = false;
                }
            }

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(check);
            return JSONString;
        }

        [Route("api/{sessionId:int}/getRRQ")]
        [HttpGet]
        public string GetRRQ(int sessionId)
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
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(r);
            return JSONString;
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

        [Route("api/{sessionId:int}/{rrqId:int}/saveRRQResponse/{QId:int}/{remoteId}/{response}")]
        [HttpGet]
        public void SaveRRQResponse(int sessionId, int rrqId, int QId, string remoteId, string response)
        {
            // first get the studentId
            // Also, can this be done in one go rather than first get the studenId?
            SqlParameter[] SParam = new SqlParameter[1];
            SParam[0] = new SqlParameter("@RemoteID", SqlDbType.VarChar);
            SParam[0].Value = remoteId;
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

            if (studentId != -1 && optionSeq != -1)
                QuestionBankController.SaveQuestionResponse(rrqId, QId, studentId, optionSeq);
        }
    }
}