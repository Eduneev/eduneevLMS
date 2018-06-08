using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UtilityClass;
using System.Data.SqlClient;

namespace MyLMS.Models
{
    public class OrganisationModel
    {
        public string CreateEntity(SqlParameter[] sparams)
        {
            string res = "Failure..";
            try
            {
                res = DAL.ExecuteScalar("CreateEntity", sparams);
            }
            catch (Exception ex)
            {
                res = ex.Message;
                throw;
            }
            return res;
        }

        public string AddEntityUser(SqlParameter[] sparams)
        {
            string res = "Failure..";
            try
            {
                res = DAL.ExecuteScalar("CreateEntityUser", sparams);
            }
            catch (Exception ex)
            {
                res = ex.Message;
                throw;
            }
            return res;
        }

        public string SaveShippedReceiver(SqlParameter[] sparams)
        {
            string res = "Failure..";
            try
            {
                res = DAL.ExecuteScalar("SaveShippedReceiver", sparams);
            }
            catch (Exception ex)
            {
                res = ex.Message;
                throw;
            }
            return res;
        }

        public string SaveShippedRemote(SqlParameter[] sparams)
        {
            string res = "Failure..";
            try
            {
                res = DAL.ExecuteScalar("SaveShippedRemote", sparams);
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