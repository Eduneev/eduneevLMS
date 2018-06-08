using MyLMS.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UtilityClass;

namespace MyLMS.Controllers
{
    public class StudentMgmtController : Controller
    {
        // GET: StudentMgmt
        public ActionResult RegisterStudent()
        {
            return View();
        }

        public ActionResult ViewStudents()
        {
            return View();
        }

        public ActionResult AllocateRemote()
        {
            return View();
        }

        [HttpPost]
        public void SaveStudent(string StudentName, string Code,int ProgramID, int CourseID, string Email, string Mobile, string Landline, string Address, string Pincode, string Gender, string BirthPlace, string SchoolName, string GuardianName, string GuardianContactNo)
        {
            StudentModel ModelObj1 = new StudentModel();
            SqlParameter[] SParam = new SqlParameter[14];

            SParam[0] = new SqlParameter("@StudentName", SqlDbType.VarChar);
            SParam[0].Value = StudentName;
            SParam[1] = new SqlParameter("@Code", SqlDbType.VarChar);
            SParam[1].Value = Code;
            SParam[2] = new SqlParameter("@ProgID", SqlDbType.Int);
            SParam[2].Value = ProgramID;
            SParam[3] = new SqlParameter("@CourseID", SqlDbType.Int);
            SParam[3].Value = CourseID;
            SParam[4] = new SqlParameter("@Email", SqlDbType.VarChar);
            SParam[4].Value = Email;
            SParam[5] = new SqlParameter("@Mobile", SqlDbType.VarChar);
            SParam[5].Value = Mobile;
            SParam[6] = new SqlParameter("@Landline", SqlDbType.VarChar);
            SParam[6].Value = Landline;
            SParam[7] = new SqlParameter("@Address", SqlDbType.Text);
            SParam[7].Value = Address;
            SParam[8] = new SqlParameter("@Pincode", SqlDbType.VarChar);
            SParam[8].Value = Pincode;
            SParam[9] = new SqlParameter("@Gender", SqlDbType.VarChar);
            SParam[9].Value = Gender;
            SParam[10] = new SqlParameter("@UserID", SqlDbType.Int);
            SParam[10].Value = Convert.ToInt32(Session["USER_ID"]);
            SParam[11] = new SqlParameter("@SchoolName", SqlDbType.VarChar);
            SParam[11].Value = SchoolName;
            SParam[12] = new SqlParameter("@GuardianName", SqlDbType.VarChar);
            SParam[12].Value = GuardianName;
            SParam[13] = new SqlParameter("@GuardianContactNo", SqlDbType.VarChar);
            SParam[13].Value = GuardianContactNo;
            try
            {
                ModelObj1.SaveStudent(SParam);
            }
            catch (Exception ex)
            {

            }
        }

        [HttpGet]
        public string GetStudents()
        {
            SqlParameter[] FObj = new SqlParameter[1];
            FObj[0] = new SqlParameter("@UserID", SqlDbType.Int);
            FObj[0].Value = Convert.ToInt32(Session["USER_ID"]);
            DataTable StudentsList = DAL.GetDataTable("GetStudents", FObj);

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(StudentsList);
            return JSONString;
        }

        [HttpGet]
        public string GetStudentsForAttendance()
        {
            SqlParameter[] FObj = new SqlParameter[1];
            FObj[0] = new SqlParameter("@UserID", SqlDbType.Int);
            FObj[0].Value = Convert.ToInt32(Session["USER_ID"]);
            DataTable StudentsList = DAL.GetDataTable("GetStudentsForAttendance", FObj);

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(StudentsList);
            return JSONString;
        }

        [HttpGet]
        public string GetStudentsForRemoteAllocation(int id)
        {
            SqlParameter[] FObj = new SqlParameter[2];
            FObj[0] = new SqlParameter("@CourseID", SqlDbType.Int);
            FObj[0].Value = id;
            FObj[1] = new SqlParameter("@UserID", SqlDbType.Int);
            FObj[1].Value = Convert.ToInt32(Session["USER_ID"]);
            DataTable StudentsList = DAL.GetDataTable("GetStudentsForRemoteAllocation", FObj);

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(StudentsList);
            return JSONString;
        }

        [HttpPost]
        public void AssignRemoteToStudent(int StudentID, string RemoteNumber)
        {
            StudentModel ModelObj1 = new StudentModel();
            SqlParameter[] SParam = new SqlParameter[2];

            SParam[0] = new SqlParameter("@StudentID", SqlDbType.Int);
            SParam[0].Value = StudentID;
            SParam[1] = new SqlParameter("@RemoteNumber", SqlDbType.VarChar);
            SParam[1].Value = RemoteNumber;
            
            try
            {
                ModelObj1.AssignRemoteToStudent(SParam);
            }
            catch (Exception ex)
            {

            }
        }

        [HttpPost]
        public void InitiateAttendance(int SessionID)
        {
            StudentModel ModelObj1 = new StudentModel();
            SqlParameter[] SParam = new SqlParameter[2];

            SParam[0] = new SqlParameter("@SessionID", SqlDbType.Int);
            SParam[0].Value = SessionID;
            SParam[1] = new SqlParameter("@UserID", SqlDbType.Int);
            SParam[1].Value = Convert.ToInt32(Session["USER_ID"]);
            try
            {
                ModelObj1.InitiateAttendance(SParam);
            }
            catch (Exception ex)
            {

            }
        }


        [HttpPost]
        public void MarkAttendance(int SessionID, int StudentID)
        {
            StudentModel ModelObj1 = new StudentModel();
            SqlParameter[] SParam = new SqlParameter[2];

            SParam[0] = new SqlParameter("@SessionID", SqlDbType.Int);
            SParam[0].Value = SessionID;
            SParam[1] = new SqlParameter("@StudentID", SqlDbType.Int);
            SParam[1].Value = StudentID;
            try
            {
                ModelObj1.MarkAttendance(SParam);
            }
            catch (Exception ex)
            {

            }
        }
    }
}