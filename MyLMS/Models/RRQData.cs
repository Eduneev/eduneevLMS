using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using UtilityClass;

namespace MyLMS.Models
{
    public class RRQData
    {
        public DataTable RRQInfoDataTable = new DataTable();
        public string RRQNo, Subject, CurrentDate;
        public int RRQID;
        public RRQData()                                                                                                      
        {

        }

        public void GetRRQInformation(int RRQ_ID)
        {
            SqlParameter[] FObj = new SqlParameter[1];
            FObj[0] = new SqlParameter("@RRQ_ID", SqlDbType.Int);
            FObj[0].Value = RRQ_ID; //****************************DEFINE RRQ ID
            RRQInfoDataTable = DAL.GetDataTable("GetRRQInformation", FObj);
            RRQID = RRQ_ID;
            RRQNo = RRQInfoDataTable.Rows[0]["RRQNo"].ToString();
            Subject = RRQInfoDataTable.Rows[0]["SubjectName"].ToString();
            CurrentDate = RRQInfoDataTable.Rows[0]["CurrentDate"].ToString();
        }
    }
}