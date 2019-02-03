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
    [SessionExpire]
    public class CourseMgmtController : Controller
    {
        // GET: CentCourseMgmt
        public ActionResult CourseMgmt()
        {
            return View();
        }

        [HttpPost]
        public void SaveProgram(string ProgramName, string ProgramCode)
        {
            CourseModel ModelObj1 = new CourseModel();
            SqlParameter[] SParam = new SqlParameter[3];
            SParam[0] = new SqlParameter("@UserID", SqlDbType.Int);
            SParam[0].Value = Convert.ToInt32(Session["USER_ID"]);
            SParam[1] = new SqlParameter("@ProgramName", SqlDbType.VarChar);
            SParam[1].Value = ProgramName;
            SParam[2] = new SqlParameter("@ProgramCode", SqlDbType.NChar);
            SParam[2].Value = ProgramCode;
            try
            {
                ModelObj1.SaveProgram(SParam);
            }
            catch (Exception ex)
            {

            }
        }

        [HttpPost]
        public void SaveCourse(int ProgID, string CourseName, string CourseCode)
        {
            CourseModel ModelObj1 = new CourseModel();
            SqlParameter[] SParam = new SqlParameter[4];

            SParam[0] = new SqlParameter("@UserID", SqlDbType.Int);
            SParam[0].Value = Convert.ToInt32(Session["USER_ID"]);
            SParam[1] = new SqlParameter("@ProgID", SqlDbType.Int);
            SParam[1].Value = ProgID;
            SParam[2] = new SqlParameter("@CourseName", SqlDbType.VarChar);
            SParam[2].Value = CourseName;
            SParam[3] = new SqlParameter("@CourseCode", SqlDbType.NChar);
            SParam[3].Value = CourseCode;

            try
            {
                ModelObj1.SaveCourse(SParam);
            }
            catch (Exception ex)
            {

            }
        }

        [HttpPost]
        public void SaveSubject(int CourseID, string SubjectName, string SubjectCode)
        {
            CourseModel ModelObj1 = new CourseModel();
            SqlParameter[] SParam = new SqlParameter[4];

            SParam[0] = new SqlParameter("@CourseID", SqlDbType.Int);
            SParam[0].Value = CourseID;
            SParam[1] = new SqlParameter("@SubjectName", SqlDbType.VarChar);
            SParam[1].Value = SubjectName;
            SParam[2] = new SqlParameter("@SubjectCode", SqlDbType.NChar);
            SParam[2].Value = SubjectCode;
            SParam[3] = new SqlParameter("@CreatedBy", SqlDbType.Int);
            SParam[3].Value = Convert.ToInt32(Session["USER_ID"]);
            try
            {
                ModelObj1.SaveSubject(SParam);
            }
            catch (Exception ex)
            {

            }
        }

        [HttpPost]
        public void SaveTopic(int SubjectID, string TopicName, string TopicCode)
        {
            CourseModel ModelObj1 = new CourseModel();
            SqlParameter[] SParam = new SqlParameter[4];

            SParam[0] = new SqlParameter("@CourseID", SqlDbType.Int);
            SParam[0].Value = SubjectID;
            SParam[1] = new SqlParameter("@TopicName", SqlDbType.VarChar);
            SParam[1].Value = TopicName;
            SParam[2] = new SqlParameter("@TopicCode", SqlDbType.NChar);
            SParam[2].Value = TopicCode;
            SParam[3] = new SqlParameter("@CreatedBy", SqlDbType.Int);
            SParam[3].Value = Convert.ToInt32(Session["USER_ID"]);
            try
            {
                ModelObj1.SaveTopic(SParam);
            }
            catch (Exception ex)
            {

            }
        }

        [HttpGet]
        public string GetPrograms()
        {
            SqlParameter[] FObj = new SqlParameter[1];
            FObj[0] = new SqlParameter("@UserID", SqlDbType.Int);
            FObj[0].Value = Convert.ToInt32(Session["USER_ID"]);
            DataTable ProgramsList = DAL.GetDataTable("GetProgram", FObj);

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(ProgramsList);
            return JSONString;
        }

        [HttpGet]
        public string GetProgramsForCenter()
        {
            SqlParameter[] FObj = new SqlParameter[1];
            FObj[0] = new SqlParameter("@UserID", SqlDbType.Int);
            FObj[0].Value = Convert.ToInt32(Session["USER_ID"]);
            DataTable ProgramsList = DAL.GetDataTable("GetProgramForCenter", FObj);

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(ProgramsList);
            return JSONString;
        }

        [HttpGet]
        public string GetCourse(int ID)
        {
            SqlParameter[] CourseObj = new SqlParameter[1];
            CourseObj[0] = new SqlParameter("@ProgID", SqlDbType.Int);
            CourseObj[0].Value = ID;
            DataTable CourseList = DAL.GetDataTable("GetCourse", CourseObj);

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(CourseList);
            return JSONString;
        }

        [HttpGet]
        public string GetSubject(int ID)
        {
            SqlParameter[] SubjectObj = new SqlParameter[1];
            SubjectObj[0] = new SqlParameter("@CourseID", SqlDbType.Int);
            SubjectObj[0].Value = ID;
            DataTable SubjectList = DAL.GetDataTable("GetSubject", SubjectObj);

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(SubjectList);
            return JSONString;
        }

        [HttpGet]
        public string GetTopics(int ID)
        {
            SqlParameter[] TopicsObj = new SqlParameter[1];
            TopicsObj[0] = new SqlParameter("@SubjectID", SqlDbType.Int);
            TopicsObj[0].Value = ID;
            DataTable TopicsList = DAL.GetDataTable("GetTopics", TopicsObj);

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(TopicsList);
            return JSONString;
        }

        [HttpGet]
        public string GetSessionName(int ID)
        {
            string SessionName;
            SqlParameter[] TopicsObj = new SqlParameter[1];
            TopicsObj[0] = new SqlParameter("@SubjectID", SqlDbType.Int);
            TopicsObj[0].Value = ID;
            DataTable SessionNo = DAL.GetDataTable("GenerateSessionNo", TopicsObj);
            SessionName = SessionNo.Rows[0]["SessionName"].ToString();
            return SessionName.Trim();
        }

        [HttpGet]
        public string GetCourseDetails()
        {
            SqlParameter[] FObj = new SqlParameter[1];
            FObj[0] = new SqlParameter("@UserID", SqlDbType.Int);
            FObj[0].Value = Convert.ToInt32(Session["USER_ID"]);
            DataTable QuestionsList = DAL.GetDataTable("GetCoureStructure", FObj);
             
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(QuestionsList);
            return JSONString;
        }
    }
}