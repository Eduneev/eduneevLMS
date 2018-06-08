using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using UtilityClass;

namespace MyLMS.Models
{
    public class DashboardData
    {
        public DataTable DAFHistory = new DataTable();
        public DashboardData()
        {

        }

        public void GetRespPrcnt(int RRQ_ID)
        {
            SqlParameter[] FObj = new SqlParameter[1];
            FObj[0] = new SqlParameter("@RRQ_ID", SqlDbType.Int);
            FObj[0].Value = RRQ_ID; //****************************DEFINE RRQ ID
            DAFHistory = DAL.GetDataTable("GetRRQQuestionsPrcnt", FObj);
        }
    }
}