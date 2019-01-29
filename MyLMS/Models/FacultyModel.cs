using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UtilityClass;

namespace MyLMS.Models
{
    public class FacultyModel
    {
        public string SaveFaculty(SqlParameter[] sparams)
        {
            string res = "Failure..";
            try
            {
                res = DAL.ExecuteScalar("SaveFaculty", sparams);
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