using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
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
            SqlParameter[] SParam = new SqlParameter[1];
            SParam[0] = new SqlParameter("@SessionId", SqlDbType.Int);
            SParam[0].Value = sessionId;
            DataTable keys = DAL.GetDataTable("GetKey", SParam);
            if (keys.Rows.Count > 0)
            {
                for (int i = 0; i < keys.Rows.Count; i++)
                {
                    string temp = Convert.ToString(Convert.IsDBNull(keys.Rows[i]["Key"]) ? "-1" : keys.Rows[i]["Key"]);
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
            string rrqId = "";

            // TODO connect to database and get RRQ

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(rrqId);
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