using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;

namespace MyLMS.Controllers
{
    public class WebApi : ApiController
    {
        /** Get session Id
         *  Parameters: MAC ID
         *  Output: Session ID
         **/ 
        [Route("api/{controller}/getSession/{macid}")]
        public int GetSession(string macid)
        {
            // TODO: add MacAddr to classroom and get the session id based on if Macaddress matches
            return 0;
        }

        [Route("api/{controller}/getStream/{sessionId:int}/{type:int}")]
        public string GetStream(int sessionId, int type)
        {
            string url = "";
            // TODO: have a url associated with session

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(url);
            return JSONString;
        }

        [Route("api/{controller}/checkKey/{sessionId:int}/{key}")]
        public string CheckKey(int sessionId, string key)
        {
            Boolean check=false;

            // TODO: Check if Key in session if correct and if so, return True

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(check);
            return JSONString;
        }

        [Route("api/{controller}/getRRQ/{sessionId:int}")]
        public string GetRRQ(int sessionId)
        {
            string rrqId;

            // TODO connect to database and get RRQ

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(rrqId);
            return JSONString;
        }

        [Route("api/{controller}/getQid/{sessionId:int}/{rrqId:int}")]
        public string GetQid(int sessionId, int rrqId)
        {
            string qId;

            // TODO complete function

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(qId);
            return JSONString;
        }
    }
}