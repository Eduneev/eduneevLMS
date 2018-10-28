using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityClass
{
    public class CreateUser
    {
        public string CreateNewUser(SqlParameter[] sparams)
        {
            string res = "Failure";
            try
            {
                res = UtilityClass.DAL.ExecuteScalar("CreateUser", sparams);
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
