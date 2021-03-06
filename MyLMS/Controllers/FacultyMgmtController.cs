﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyLMS.Models;
using Newtonsoft.Json;
using UtilityClass;

namespace MyLMS.Controllers
{
    [SessionExpire]
    public class FacultyMgmtController : Controller
    {
        // GET: FacultyMgmt
        public ActionResult ManageFaculty()
        {
            return View();
        }

        [HttpPost]
        public void SaveFaculty(string FacultyName, int ProgramID, int CourseID, int SubjectID, string Email, string Mobile, string Gender)
        {
            FacultyModel ModelObj1 = new FacultyModel();
            SqlParameter[] SParam = new SqlParameter[8];

            SParam[0] = new SqlParameter("@StudentName", SqlDbType.VarChar);
            SParam[0].Value = FacultyName;
            SParam[1] = new SqlParameter("@ProgID", SqlDbType.Int);
            SParam[1].Value = ProgramID;
            SParam[2] = new SqlParameter("@CourseID", SqlDbType.Int);
            SParam[2].Value = CourseID;
            SParam[3] = new SqlParameter("@SubjectID", SqlDbType.Int);
            SParam[3].Value = SubjectID;
            SParam[4] = new SqlParameter("@Email", SqlDbType.VarChar);
            SParam[4].Value = Email;
            SParam[5] = new SqlParameter("@Mobile", SqlDbType.VarChar);
            SParam[5].Value = Mobile;
            SParam[6] = new SqlParameter("@Gender", SqlDbType.VarChar);
            SParam[6].Value = Gender;
            SParam[7] = new SqlParameter("@UserID", SqlDbType.Int);
            SParam[7].Value = Convert.ToInt32(Session["USER_ID"]);
            try
            {
                ModelObj1.SaveFaculty(SParam);
            }
            catch (Exception ex)
            {

            }
        }

        [HttpPost]
        public void DeleteFaculty(int FacultyID, int SubjectID)
        {
            FacultyModel ModelObj1 = new FacultyModel();
            SqlParameter[] SParam = new SqlParameter[3];

            SParam[0] = new SqlParameter("@FacultyID", SqlDbType.Int);
            SParam[0].Value = FacultyID;
            SParam[1] = new SqlParameter("@SubjectID", SqlDbType.Int);
            SParam[1].Value = SubjectID;
            SParam[2] = new SqlParameter("@UserID", SqlDbType.Int);
            SParam[2].Value = Convert.ToInt32(Session["USER_ID"]);

            try
            {
                ModelObj1.DeleteFaculty(SParam);
            }
            catch (Exception ex)
            {

            }
        }

        [HttpGet]
        public string GetFaculty(int ID)
        {
            SqlParameter[] FacultyObj = new SqlParameter[2];
            FacultyObj[0] = new SqlParameter("@UserID", SqlDbType.Int);
            FacultyObj[0].Value = Convert.ToInt32(Session["USER_ID"]);
            FacultyObj[1] = new SqlParameter("@SubjectID", SqlDbType.Int);
            FacultyObj[1].Value = ID;
            DataTable FacultyList = DAL.GetDataTable("GetFaculty", FacultyObj);

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(FacultyList);
            return JSONString;
        }
    }
}