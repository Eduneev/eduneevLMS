using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace UtilityClass
{
    public class UserTools
    {
        public static string GetLoggedOnUser()
        {
            string loggedOnUser = HttpContext.Current.User.Identity.Name.ToLower();
            loggedOnUser = loggedOnUser.Substring(loggedOnUser.IndexOf("\\") + 1);
            return loggedOnUser;
        }

        public static UtilityClass.LoggedOnUser getLoggedOnUserDetails(string username, string password)
        {
            UtilityClass.LoggedOnUser UserDetails = new UtilityClass.LoggedOnUser();
            DataTable dt = new DataTable();
            SqlParameter[] sparams = new SqlParameter[2];
            sparams[0] = new SqlParameter("@UserName", SqlDbType.VarChar);
            sparams[0].Value = username;
            sparams[1] = new SqlParameter("@Password", SqlDbType.VarChar);
            sparams[1].Value = EDHelper.EncryptTripleDES(password);

            dt = DAL.GetDataTable("ValidateUser", sparams);

            if (dt.Rows.Count > 0)
            {
                UserDetails.UserID = int.Parse(Convert.IsDBNull(dt.Rows[0]["UserID"]) ? "0" : dt.Rows[0]["UserID"].ToString());
                UserDetails.UserName = Convert.IsDBNull(dt.Rows[0]["UserName"]) ? "" : dt.Rows[0]["UserName"].ToString();
                UserDetails.FullName = Convert.IsDBNull(dt.Rows[0]["FullName"]) ? "" : dt.Rows[0]["FullName"].ToString();
                UserDetails.EmailID = Convert.IsDBNull(dt.Rows[0]["EmailID"]) ? "" : dt.Rows[0]["EmailID"].ToString();
                UserDetails.RoleID = int.Parse(Convert.IsDBNull(dt.Rows[0]["RoleID"]) ? "0" : dt.Rows[0]["RoleID"].ToString());
                UserDetails.IsActive = (bool)(Convert.IsDBNull(dt.Rows[0]["IsActive"]) ? false : dt.Rows[0]["IsActive"]);
                UserDetails.CreatedBy = int.Parse(Convert.IsDBNull(dt.Rows[0]["CreatedBy"]) ? "0" : dt.Rows[0]["CreatedBy"].ToString());
                UserDetails.AccountID = int.Parse(Convert.IsDBNull(dt.Rows[0]["AccountID"]) ? "0" : dt.Rows[0]["AccountID"].ToString());
            }
            return UserDetails;
        }      

        public static bool authenticateUserOnDefault(LoggedOnUser loggedonuserdetails)
        {
            if (loggedonuserdetails.IsActive == false)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
