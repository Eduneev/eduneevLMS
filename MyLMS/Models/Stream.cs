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
        public string SaveStream(int SessionID, string EntityCode, string ProgCode, string CourseCode, string SubjectCode, bool Transcode=true)
        {
            // Create the stream attached to session
            try
            {
                SqlParameter[] SParam;
                string stream = "rtsp://18.224.156.241:1935/" + EntityCode + "/" + EntityCode + "_" + ProgCode + "_" + CourseCode + "_" + SubjectCode; // replace eduneev with server ip
                string player_stream = "rtsp://18.224.156.241:543/" + EntityCode + "/" + EntityCode + "_" + ProgCode + "_" + CourseCode + "_" + SubjectCode;
                string stream_low = player_stream + "_DVD";
                string stream_med = player_stream + "_HD";
                string stream_high = player_stream + "_FHD";
                string stream_obs = stream;
                
                SParam = new SqlParameter[3];
                SParam[0] = new SqlParameter("@SessionID", SqlDbType.Int);
                SParam[1] = new SqlParameter("@Stream", SqlDbType.VarChar);
                SParam[2] = new SqlParameter("@Type", SqlDbType.Int);
                SParam[0].Value = SessionID;

                if (Transcode)
                {
                    SParam[1].Value = stream_low;
                    SParam[2].Value = 1;

                    DAL.ExecuteScalar("CreateStream", SParam);

                    SParam[1].Value = stream_med;
                    SParam[2].Value = 2;
                    DAL.ExecuteScalar("CreateStream", SParam);

                    SParam[1].Value = stream_high;
                    SParam[2].Value = 3;
                    DAL.ExecuteScalar("CreateStream", SParam);

                }
                else
                {
                    SParam[1].Value = player_stream;
                    SParam[2].Value = 1;
                    DAL.ExecuteScalar("CreateStream", SParam);
                }

                SParam[1].Value = stream_obs;
                SParam[2].Value = -10;
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