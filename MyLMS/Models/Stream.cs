using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using UtilityClass;

namespace MyLMS.Models
{
    public class Stream
    {
        public string SaveStream(int SessionID, string EntityCode, string ProgCode, string CourseCode, string SubjectCode)
        {
            // Create the stream attached to session
            try
            {
                SqlParameter[] SParam;
                string stream = "rtsp://eduneev/live:1935_" + EntityCode + "_" + ProgCode + "_" + CourseCode + "_" + SubjectCode;
                string stream_low = stream + "_360p.flv";
                string stream_med = stream + "_480p.flv";
                string stream_high = stream + "_720p.flv";
                // WHAT TO DO AFTER THIS?

                SParam = new SqlParameter[3];
                SParam[0] = new SqlParameter("@SessionID", SqlDbType.Int);
                SParam[0].Value = SessionID;
                SParam[1] = new SqlParameter("@Stream", SqlDbType.VarChar);
                SParam[1].Value = stream_low;
                SParam[2] = new SqlParameter("@Type", SqlDbType.Int);
                SParam[2].Value = 1;

                DAL.ExecuteScalar("CreateStream", SParam);

                SParam[1].Value = stream_med;
                SParam[2].Value = 2;
                DAL.ExecuteScalar("CreateStream", SParam);

                SParam[1].Value = stream_high;
                SParam[2].Value = 3;
                DAL.ExecuteScalar("CreateStream", SParam);
            }
            catch (Exception ex)
            {
                return "Failure";
            }

            return "Success";
        }
    }
}