using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
        [Route("api/getSession/{macid}")]
        [HttpGet]
        public int GetSession(string macid)
        {
            // TODO: add MacAddr to classroom and get the session id based on if Macaddress matches
            int SessionId;
            SqlParameter[] SParam = new SqlParameter[1];
            SParam[0] = new SqlParameter("@MACAddr", SqlDbType.VarChar);
            SParam[0].Value = macid;

            // Current Logic
            // Link classroom with session. When classroom joins session, this value gets set.
            // Current logic. Link classroom with MACAddr. Each classroom has a macaddr, which is set when classroom registers.
            // When api is called, we check is macaddr exists in CenterName database and then retrieve the session Id associated
            //DataTable rooms = DAL.GetDataTable("getSession")

            //return SessionId;
            return 0;
        }

        [Route("api/{sessionId:int}/getStream/{type:int}")]
        [HttpGet]
        public string GetStream(int sessionId, int type)
        {
            string url = "";
            // TODO: have a url associated with session

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(url);
            return JSONString;
        }

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
    }
}