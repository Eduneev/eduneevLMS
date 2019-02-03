using MyLMS.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Web.Mvc;
using UtilityClass;

namespace MyLMS.Controllers
{
    [SessionExpire]
    public class QuestionBankController : Controller
    {
        // GET: QuestionBank
        dynamic OptList;
        public ActionResult AddQuestion()
        {
            return View();
        }

        public ActionResult ViewQuestions()
        {
            return View();
        }

        public ActionResult CreateQTags()
        {
            return View();
        }

        [HttpPost]
        public void SaveQuestion(string QuestionText, int QTagID, int DisplayTime)
        {
            QuestionModel QuesObj1 = new QuestionModel();
            SqlParameter[] SParam = new SqlParameter[4];

            SParam[0] = new SqlParameter("@QuestionText", SqlDbType.NVarChar);
            SParam[0].Value = QuestionText;
            SParam[1] = new SqlParameter("@QTagID", SqlDbType.Int);
            SParam[1].Value = QTagID;
            SParam[2] = new SqlParameter("@DisplayTime", SqlDbType.Int);
            SParam[2].Value = DisplayTime;
            SParam[3] = new SqlParameter("@UserID", SqlDbType.Int);
            SParam[3].Value = Convert.ToInt32(Session["USER_ID"]);

            try
            {
                Session["QID"] = QuesObj1.SaveQuestion(SParam).ToString();
            }
            catch (Exception ex)
            {

            }
        }

        [HttpPost]
        public void DeleteQuestion(string QID)
        {
            QuestionModel QuesObj1 = new QuestionModel();
            SqlParameter[] SParam = new SqlParameter[1];

            SParam[0] = new SqlParameter("@QID", SqlDbType.Int);
            SParam[0].Value = QID;

            try
            {
                QuesObj1.DeleteQuestion(SParam);
            }
            catch (Exception ex)
            {

            }
        }

        [HttpPost]
        public void DeleteTag(int QTagID)
        {
            SqlParameter[] SParam = new SqlParameter[1];

            SParam[0] = new SqlParameter("@QTagID", SqlDbType.Int);
            SParam[0].Value = QTagID;

            string res = "Failure";
            try
            {
                res = DAL.ExecuteScalar("DeleteQTag", SParam);
            }
            catch (Exception ex)
            {
            }
        }

        [HttpPost]
        public void CreateTags(string TagText)
        {
            SqlParameter[] SParam = new SqlParameter[2];

            SParam[0] = new SqlParameter("@UserID", SqlDbType.Int);
            SParam[0].Value = Convert.ToInt32(Session["USER_ID"]);
            SParam[1] = new SqlParameter("@TagText", SqlDbType.VarChar);
            SParam[1].Value = TagText;

            string res = "Failure";
            try
            {
                res = DAL.ExecuteScalar("CreateTags", SParam);
            }
            catch (Exception ex)
            {
            }
        }

        [HttpPost]
        public void EditTag(int QTagID, string TagText)
        {
            SqlParameter[] SParam = new SqlParameter[2];

            SParam[0] = new SqlParameter("@QTagID", SqlDbType.Int);
            SParam[0].Value = QTagID;
            SParam[1] = new SqlParameter("@TagText", SqlDbType.VarChar);
            SParam[1].Value = TagText;

            string res = "Failure";
            try
            {
                res = DAL.ExecuteScalar("EditTag", SParam);
            }
            catch (Exception ex)
            {
            }
        }

        [HttpPost]
        public void SaveOptions(int OptionSeq, string OptionText, int OptionMark, bool IsOptionCorrect)
        {
            QuestionModel QuesObj1 = new QuestionModel();
            SqlParameter[] SParam = new SqlParameter[5];
            SParam[0] = new SqlParameter("@QID", SqlDbType.Int);
            SParam[0].Value = Convert.ToInt32(Session["QID"]);
            SParam[1] = new SqlParameter("@OptionSeq", SqlDbType.Int);
            SParam[1].Value = OptionSeq;
            SParam[2] = new SqlParameter("@OptionText", SqlDbType.NVarChar);
            SParam[2].Value = OptionText;
            SParam[3] = new SqlParameter("@OptionMark", SqlDbType.Int);
            SParam[3].Value = OptionMark;
            SParam[4] = new SqlParameter("@IsOptionCorrect", SqlDbType.Bit);
            SParam[4].Value = IsOptionCorrect;

            try
            {
                QuesObj1.SaveOptions(SParam);
            }
            catch (Exception ex)
            {

            }
        }

        [HttpPost]
        public void AddRemoveQuestionToRRQ(int QID)
        {
            QuestionModel QuesObj1 = new QuestionModel();
            SqlParameter[] SParam = new SqlParameter[2];
            SParam[0] = new SqlParameter("@QID", SqlDbType.Int);
            SParam[0].Value = QID;
            SParam[1] = new SqlParameter("@RRQ_ID", SqlDbType.Int);
            SParam[1].Value = Convert.ToInt32(Session["RRQ_ID"]);

            try
            {
                QuesObj1.AddRemoveQuestionToRRQ(SParam);
            }
            catch (Exception ex)
            {

            }
        }

        [HttpGet]
        public string GetQuestions()
        {
            SqlParameter[] FObj = new SqlParameter[1];
            FObj[0] = new SqlParameter("@UserID", SqlDbType.Int);
            FObj[0].Value = Convert.ToInt32(Session["USER_ID"]);
            List<QuestionOptions> CO = new List<QuestionOptions>();
            DataTable QuestionsList = DAL.GetDataTable("GetQuestions", FObj);
            SqlParameter[] SParam = new SqlParameter[1];
            List<Question> QuestionList = new List<Question>();


            for (int i = 0; i < QuestionsList.Rows.Count; i++)
            {
                Question Question = new Question();
                Question.QID = Convert.ToInt32(QuestionsList.Rows[i]["QID"]);
                Question.QTypeID = Convert.ToInt32(QuestionsList.Rows[i]["QTypeID"].ToString());
                Question.QuestionText = QuestionsList.Rows[i]["QuestionText"].ToString();
                Question.IsCompulsory = Convert.ToBoolean(QuestionsList.Rows[i]["IsCompulsory"]);
                Question.TagText = QuestionsList.Rows[i]["TagText"].ToString();
                QuestionList.Add(Question);

                SParam[0] = new SqlParameter("@QID", SqlDbType.Int);
                SParam[0].Value = Convert.ToInt32(QuestionsList.Rows[i]["QID"]);

                DataTable OptionsList = DAL.GetDataTable("GetOptions", SParam);
                List<Option> OptionList = new List<Option>();
                for (int j = 0; j < OptionsList.Rows.Count; j++)
                {
                    Option Options = new Option();
                    Options.OptionID = Convert.ToInt32(Convert.IsDBNull(OptionsList.Rows[j]["OptionID"]) ? "0" : OptionsList.Rows[j]["OptionID"]);
                    Options.QID = Convert.ToInt32(Convert.IsDBNull(OptionsList.Rows[j]["QID"]) ? "0" : OptionsList.Rows[j]["QID"]);
                    Options.OptionSeq = Convert.ToInt32(Convert.IsDBNull(OptionsList.Rows[j]["OptionSeq"]) ? "0" : OptionsList.Rows[j]["OptionSeq"]);
                    Options.OptionText = Convert.IsDBNull(OptionsList.Rows[j]["OptionText"]) ? "" : OptionsList.Rows[j]["OptionText"].ToString();
                    Options.Mark = Convert.ToInt32(Convert.IsDBNull(OptionsList.Rows[j]["Mark"]) ? "0" : OptionsList.Rows[j]["Mark"]);
                    Options.IsCorrect = Convert.ToBoolean(Convert.IsDBNull(OptionsList.Rows[j]["IsCorrect"]) ? "0" : OptionsList.Rows[j]["IsCorrect"]);
                    OptionList.Add(Options);

                    OptList = OptionList;
                }

                CO.Add(new QuestionOptions
                {
                    Question = Question,
                    Options = OptList
                });
            }

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(CO);
            return JSONString;
        }

        [HttpGet]
        public string GetQuestionsByTag(int id)
        {
            SqlParameter[] FObj = new SqlParameter[2];
            FObj[0] = new SqlParameter("@UserID", SqlDbType.Int);
            FObj[0].Value = Convert.ToInt32(Session["USER_ID"]);
            FObj[1] = new SqlParameter("@QTagID", SqlDbType.Int);
            FObj[1].Value = id;
            List<QuestionOptions> CO = new List<QuestionOptions>();
            DataTable QuestionsList = DAL.GetDataTable("GetQuestionsByTag", FObj);
            SqlParameter[] SParam = new SqlParameter[1];
            List<Question> QuestionList = new List<Question>();


            for (int i = 0; i < QuestionsList.Rows.Count; i++)
            {
                Question Question = new Question();
                Question.QID = Convert.ToInt32(QuestionsList.Rows[i]["QID"]);
                Question.QTypeID = Convert.ToInt32(QuestionsList.Rows[i]["QTypeID"].ToString());
                Question.QuestionText = QuestionsList.Rows[i]["QuestionText"].ToString();
                Question.IsCompulsory = Convert.ToBoolean(QuestionsList.Rows[i]["IsCompulsory"]);
                Question.TagText = QuestionsList.Rows[i]["TagText"].ToString();
                QuestionList.Add(Question);

                SParam[0] = new SqlParameter("@QID", SqlDbType.Int);
                SParam[0].Value = Convert.ToInt32(QuestionsList.Rows[i]["QID"]);

                DataTable OptionsList = DAL.GetDataTable("GetOptions", SParam);
                List<Option> OptionList = new List<Option>();
                for (int j = 0; j < OptionsList.Rows.Count; j++)
                {
                    Option Options = new Option();
                    Options.OptionID = Convert.ToInt32(Convert.IsDBNull(OptionsList.Rows[j]["OptionID"]) ? "0" : OptionsList.Rows[j]["OptionID"]);
                    Options.QID = Convert.ToInt32(Convert.IsDBNull(OptionsList.Rows[j]["QID"]) ? "0" : OptionsList.Rows[j]["QID"]);
                    Options.OptionSeq = Convert.ToInt32(Convert.IsDBNull(OptionsList.Rows[j]["OptionSeq"]) ? "0" : OptionsList.Rows[j]["OptionSeq"]);
                    Options.OptionText = Convert.IsDBNull(OptionsList.Rows[j]["OptionText"]) ? "" : OptionsList.Rows[j]["OptionText"].ToString();
                    Options.Mark = Convert.ToInt32(Convert.IsDBNull(OptionsList.Rows[j]["Mark"]) ? "0" : OptionsList.Rows[j]["Mark"]);
                    Options.IsCorrect = Convert.ToBoolean(Convert.IsDBNull(OptionsList.Rows[j]["IsCorrect"]) ? "0" : OptionsList.Rows[j]["IsCorrect"]);
                    OptionList.Add(Options);

                    OptList = OptionList;
                }

                CO.Add(new QuestionOptions
                {
                    Question = Question,
                    Options = OptList
                });
            }

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(CO);
            return JSONString;
        }

        [HttpGet]
        public string GetQuestionAndOptions(int ID)
        {
            List<QuestionOptions> CO = new List<QuestionOptions>();

            SqlParameter[] QParam = new SqlParameter[1];
            QParam[0] = new SqlParameter("@QID", SqlDbType.Int);
            QParam[0].Value = ID;
            DataTable QuestionsList = DAL.GetDataTable("GetQuestionsAndOptions", QParam);
            SqlParameter[] SParam = new SqlParameter[1];
            List<Question> QuestionList = new List<Question>();

            for (int i = 0; i < QuestionsList.Rows.Count; i++)
            {
                Question Question = new Question();
                Question.QID = Convert.ToInt32(QuestionsList.Rows[i]["QID"]);
                Question.QUES_NO = QuestionsList.Rows[i]["QUES_NO"].ToString();
                Question.QTypeID = Convert.ToInt32(QuestionsList.Rows[i]["QTypeID"].ToString());
                Question.QuestionText = QuestionsList.Rows[i]["QuestionText"].ToString();
                Question.IsCompulsory = Convert.ToBoolean(QuestionsList.Rows[i]["IsCompulsory"]);
                QuestionList.Add(Question);

                SParam[0] = new SqlParameter("@QID", SqlDbType.Int);
                SParam[0].Value = Convert.ToInt32(QuestionsList.Rows[i]["QID"]);

                DataTable OptionsList = DAL.GetDataTable("GetOptions", SParam);
                List<Option> OptionList = new List<Option>();
                for (int j = 0; j < OptionsList.Rows.Count; j++)
                {
                    Option Options = new Option();
                    Options.OptionID = Convert.ToInt32(Convert.IsDBNull(OptionsList.Rows[j]["OptionID"]) ? "0" : OptionsList.Rows[j]["OptionID"]);
                    Options.QID = Convert.ToInt32(Convert.IsDBNull(OptionsList.Rows[j]["QID"]) ? "0" : OptionsList.Rows[j]["QID"]);
                    Options.OptionChar = OptionsList.Rows[j]["OptionChar"].ToString();
                    Options.OptionSeq = Convert.ToInt32(Convert.IsDBNull(OptionsList.Rows[j]["OptionSeq"]) ? "0" : OptionsList.Rows[j]["OptionSeq"]);
                    Options.OptionText = Convert.IsDBNull(OptionsList.Rows[j]["OptionText"]) ? "" : OptionsList.Rows[j]["OptionText"].ToString();
                    Options.Mark = Convert.ToInt32(Convert.IsDBNull(OptionsList.Rows[j]["Mark"]) ? "0" : OptionsList.Rows[j]["Mark"]);
                    Options.IsCorrect = Convert.ToBoolean(Convert.IsDBNull(OptionsList.Rows[j]["IsCorrect"]) ? "0" : OptionsList.Rows[j]["IsCorrect"]);
                    OptionList.Add(Options);

                    OptList = OptionList;
                }

                CO.Add(new QuestionOptions
                {
                    Question = Question,
                    Options = OptList
                });
            }

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(CO);
            return JSONString;
        }


        [HttpGet]
        public string GetRRQQuestions()
        {
            List<QuestionOptions> CO = new List<QuestionOptions>();
            SqlParameter[] QSParam = new SqlParameter[1];
            QSParam[0] = new SqlParameter("@RRQ_ID", SqlDbType.Int);
            QSParam[0].Value = Convert.ToInt32(Session["RRQ_ID_Display"].ToString());
            DataTable QuestionsList = DAL.GetDataTable("GetRRQQuestionsByRRQ_ID", QSParam);

            SqlParameter[] SParam = new SqlParameter[1];
            List<Question> QuestionList = new List<Question>();


            for (int i = 0; i < QuestionsList.Rows.Count; i++)
            {
                Question Question = new Question();
                Question.QID = Convert.ToInt32(QuestionsList.Rows[i]["QID"]);
                Question.QuestionText = QuestionsList.Rows[i]["QuestionText"].ToString();
                Question.QTime = Convert.ToInt32(QuestionsList.Rows[i]["QTime"].ToString());
                QuestionList.Add(Question);

                SParam[0] = new SqlParameter("@QID", SqlDbType.Int);
                SParam[0].Value = Convert.ToInt32(QuestionsList.Rows[i]["QID"]);

                DataTable OptionsList = DAL.GetDataTable("GetOptions", SParam);
                List<Option> OptionList = new List<Option>();
                for (int j = 0; j < OptionsList.Rows.Count; j++)
                {
                    Option Options = new Option();
                    Options.OptionID = Convert.ToInt32(Convert.IsDBNull(OptionsList.Rows[j]["OptionID"]) ? "0" : OptionsList.Rows[j]["OptionID"]);
                    Options.QID = Convert.ToInt32(Convert.IsDBNull(OptionsList.Rows[j]["QID"]) ? "0" : OptionsList.Rows[j]["QID"]);
                    Options.OptionSeq = Convert.ToInt32(Convert.IsDBNull(OptionsList.Rows[j]["OptionSeq"]) ? "0" : OptionsList.Rows[j]["OptionSeq"]);
                    Options.OptionText = Convert.IsDBNull(OptionsList.Rows[j]["OptionText"]) ? "" : OptionsList.Rows[j]["OptionText"].ToString();
                    Options.Mark = Convert.ToInt32(Convert.IsDBNull(OptionsList.Rows[j]["Mark"]) ? "0" : OptionsList.Rows[j]["Mark"]);
                    Options.IsCorrect = Convert.ToBoolean(Convert.IsDBNull(OptionsList.Rows[j]["IsCorrect"]) ? "0" : OptionsList.Rows[j]["IsCorrect"]);
                    OptionList.Add(Options);

                    OptList = OptionList;
                }
                CO.Add(new QuestionOptions
                {
                    Question = Question,
                    Options = OptList
                });
            }

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(CO);
            return JSONString;
        }

        [HttpGet]
        public string GetRRQQuestionAndOptions(int ID)
        {
            List<QuestionOptions> CO = new List<QuestionOptions>();

            SqlParameter[] QParam = new SqlParameter[1];
            QParam[0] = new SqlParameter("@QID", SqlDbType.Int);
            QParam[0].Value = ID;
            DataTable QuestionsList = DAL.GetDataTable("GetRRQQuestionsAndOptions", QParam);
            SqlParameter[] SParam = new SqlParameter[1];
            List<Question> QuestionList = new List<Question>();

            for (int i = 0; i < QuestionsList.Rows.Count; i++)
            {
                Question Question = new Question();
                Question.QID = Convert.ToInt32(QuestionsList.Rows[i]["QID"]);
                Question.QUES_NO = QuestionsList.Rows[i]["QUES_NO"].ToString();
                Question.QTypeID = Convert.ToInt32(QuestionsList.Rows[i]["QTypeID"].ToString());
                Question.QuestionText = QuestionsList.Rows[i]["QuestionText"].ToString();
                Question.IsCompulsory = Convert.ToBoolean(QuestionsList.Rows[i]["IsCompulsory"]);
                QuestionList.Add(Question);

                SParam[0] = new SqlParameter("@QID", SqlDbType.Int);
                SParam[0].Value = Convert.ToInt32(QuestionsList.Rows[i]["QID"]);

                DataTable OptionsList = DAL.GetDataTable("GetOptions", SParam);
                List<Option> OptionList = new List<Option>();
                for (int j = 0; j < OptionsList.Rows.Count; j++)
                {
                    Option Options = new Option();
                    Options.OptionID = Convert.ToInt32(Convert.IsDBNull(OptionsList.Rows[j]["OptionID"]) ? "0" : OptionsList.Rows[j]["OptionID"]);
                    Options.QID = Convert.ToInt32(Convert.IsDBNull(OptionsList.Rows[j]["QID"]) ? "0" : OptionsList.Rows[j]["QID"]);
                    Options.OptionChar = OptionsList.Rows[j]["OptionChar"].ToString();
                    Options.OptionSeq = Convert.ToInt32(Convert.IsDBNull(OptionsList.Rows[j]["OptionSeq"]) ? "0" : OptionsList.Rows[j]["OptionSeq"]);
                    Options.OptionText = Convert.IsDBNull(OptionsList.Rows[j]["OptionText"]) ? "" : OptionsList.Rows[j]["OptionText"].ToString();
                    Options.Mark = Convert.ToInt32(Convert.IsDBNull(OptionsList.Rows[j]["Mark"]) ? "0" : OptionsList.Rows[j]["Mark"]);
                    Options.IsCorrect = Convert.ToBoolean(Convert.IsDBNull(OptionsList.Rows[j]["IsCorrect"]) ? "0" : OptionsList.Rows[j]["IsCorrect"]);
                    OptionList.Add(Options);

                    OptList = OptionList;
                }

                CO.Add(new QuestionOptions
                {
                    Question = Question,
                    Options = OptList
                });
            }

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(CO);
            return JSONString;
        }

        [HttpGet]
        public string GetRRQAnswerPercentage(int id)
        {
            SqlParameter[] FObj = new SqlParameter[2];
            FObj[0] = new SqlParameter("@RRQ_ID", SqlDbType.Int);
            FObj[0].Value = Convert.ToInt32(Session["RRQ_ID_Display"].ToString());
            FObj[1] = new SqlParameter("@QID", SqlDbType.Int);
            FObj[1].Value = id;

            DataTable TagsList = DAL.GetDataTable("GetRRQAnswerPrcnt", FObj);
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(TagsList);
            return JSONString;
        }

        [HttpGet]
        public string GetTags()
        {
            SqlParameter[] FObj = new SqlParameter[1];
            FObj[0] = new SqlParameter("@UserID", SqlDbType.Int);
            FObj[0].Value = Convert.ToInt32(Session["USER_ID"]);

            DataTable TagsList = DAL.GetDataTable("GetTags", FObj);
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(TagsList);
            return JSONString;
        }

        [HttpPost]
        public static bool SaveQuestionResponse(int RRQId, int QId, int studentId, int optionSeq) // response could be A,B,C,D or E
        {
            SqlParameter[] SParam = new SqlParameter[4];

            // how do we map from response to optionId? Mapping to OptionSeq instead
            SParam[0] = new SqlParameter("@RRQ_ID", SqlDbType.Int);
            SParam[0].Value = RRQId;
            SParam[1] = new SqlParameter("@QID", SqlDbType.Int);
            SParam[1].Value = QId;
            SParam[2] = new SqlParameter("@StudentID", SqlDbType.Int);
            SParam[2].Value = studentId;
            SParam[3] = new SqlParameter("@OptionSeq", SqlDbType.Int);
            SParam[3].Value = optionSeq;

            try
            {
                DataTable val = DAL.GetDataTable("SaveQuestionResponse", SParam);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
            return false;
        }

        [HttpPost]
        public static bool SaveMobileQuestionResponse(int RRQID, int QuesNo, int StudentID, int OptionSeq)
        {
            SqlParameter[] SParam = new SqlParameter[4];

            // how do we map from response to optionId? Mapping to OptionSeq instead
            SParam[0] = new SqlParameter("@RRQ_ID", SqlDbType.Int);
            SParam[0].Value = RRQID;
            SParam[1] = new SqlParameter("@QUES_NO", SqlDbType.Int);
            SParam[1].Value = QuesNo;
            SParam[2] = new SqlParameter("@StudentID", SqlDbType.Int);
            SParam[2].Value = StudentID;
            SParam[3] = new SqlParameter("@OptionSeq", SqlDbType.Int);
            SParam[3].Value = OptionSeq;

            try
            {
                DataTable val = DAL.GetDataTable("SaveMobileQuestionResponse", SParam);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
            return false;
        }
    }
}