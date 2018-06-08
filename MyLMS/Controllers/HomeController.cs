using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Web.UI;
using UtilityClass;


namespace MyLMS.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            CreateUser Obj = new CreateUser();            

            SqlParameter[] UsrParam = new SqlParameter[7];
            UsrParam[0] = new SqlParameter("@UserName", SqlDbType.VarChar);
            UsrParam[0].Value = "just.chandan@gmail.com";
            UsrParam[1] = new SqlParameter("@Password", SqlDbType.Int);
            UsrParam[1].Value = EDHelper.EncryptTripleDES("zerosoft");
            UsrParam[2] = new SqlParameter("@FullName", SqlDbType.Int);
            UsrParam[2].Value = "Chandan Mohanty";
            UsrParam[3] = new SqlParameter("@EmailID", SqlDbType.Int);
            UsrParam[3].Value = "just.chandan@gmail.com";
            UsrParam[4] = new SqlParameter("@IsActive", SqlDbType.Int);
            UsrParam[4].Value = 1;
            UsrParam[5] = new SqlParameter("@CreatedBy", SqlDbType.Int);
            UsrParam[5].Value = 1;
            UsrParam[6] = new SqlParameter("@RoleID", SqlDbType.Int);
            UsrParam[6].Value = 1;

            //var Msg = Obj.CreateNewUser(UsrParam);
            return View();
        }

        public ActionResult Logon()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Login()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            //return RedirectToAction("Index", "Default");
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


       public bool ValidateUser(string UserName, string Password)
        {
            LoggedOnUser LOU = (Session["LoggedOnUserDetails"] == null ? UserTools.getLoggedOnUserDetails(UserName, Password) : (LoggedOnUser)Session["LoggedOnUserDetails"]);
            if(UserTools.authenticateUserOnDefault(LOU) == true)
            {
                Session["USER_ID"] = LOU.UserID;
                Session["USER_NAME"] = LOU.FullName;
                Session["EMAIL"] = LOU.EmailID;
                Session["RoleID"] = LOU.RoleID;

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}