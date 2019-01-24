using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using UtilityClass;

namespace MyLMS.Models
{
    public class BillingModel
    {

        public int Client { get; set; }
        public long Bytes { get; set; }
        public string Date { get; set; }
        public string Stream { get; set; }
        public int Duration { get; set; }
        public int Amount { get; set; }

        public string CreateEntityBilling(SqlParameter[] sparams)
        {
            string res = "Failure..";
            try
            {
                res = DAL.ExecuteScalar("CreateEntityBilling", sparams);
            }
            catch (Exception ex)
            {
                res = ex.Message;
                throw;
            }
            return res;
        }
    }
}