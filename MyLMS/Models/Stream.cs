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

            SqlParameter[] SParam = new SqlParameter[1];
            SParam[0] = new SqlParameter("@SessionID", SqlDbType.Int);
            SParam[0].Value = SessionID;
            DataTable val = DAL.GetDataTable("GetEntityType", SParam);

            int EntityType;
            if (val.Rows.Count > 0)
            {
                EntityType = Convert.ToInt32(Convert.IsDBNull(val.Rows[0]["EntityTypeID"]) ? -1 : val.Rows[0]["EntityTypeID"]);
            }
            else
                return "Failure";

            //Internet Case
            if (EntityType == 1)
            {
                try
                {
                    string stream = "rtsp://18.224.156.241:1935/" + EntityCode + "/" + EntityCode + "_" + ProgCode + "_" + CourseCode + "_" + SubjectCode; // replace eduneev with server ip
                    string player_stream = "rtsp://18.224.156.241:554/" + EntityCode + "/" + EntityCode + "_" + ProgCode + "_" + CourseCode + "_" + SubjectCode;
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
            }

            // Satellite case
            else if (EntityType == 2)
            {
                SParam = new SqlParameter[1];
                SParam[0] = new SqlParameter("@SessionID", SqlDbType.Int);
                SParam[0].Value = SessionID;
                val = DAL.GetDataTable("GetStudioIP", SParam);

                string satelliteIP = string.Empty;
                if (val.Rows.Count > 0)
                {
                    satelliteIP = Convert.ToString(Convert.IsDBNull(val.Rows[0]["SatelliteIP"]) ? String.Empty : val.Rows[0]["SatelliteIP"]);
                }

                try
                {
                    string stream = "udp://@" + satelliteIP;
                    string stream_obs = "udp://" + satelliteIP;

                    SParam = new SqlParameter[3];
                    SParam[0] = new SqlParameter("@SessionID", SqlDbType.Int);
                    SParam[1] = new SqlParameter("@Stream", SqlDbType.VarChar);
                    SParam[2] = new SqlParameter("@Type", SqlDbType.Int);
                    SParam[0].Value = SessionID;

                    SParam[1].Value = stream;
                    SParam[2].Value = 1;
                    DAL.ExecuteScalar("CreateStream", SParam);
                
                    SParam[1].Value = stream_obs;
                    SParam[2].Value = -10;
                    DAL.ExecuteScalar("CreateStream", SParam);
                }
                catch (Exception ex)
                {
                    return "Failure";
                }
            }

            return "Success";
        }
    }
}