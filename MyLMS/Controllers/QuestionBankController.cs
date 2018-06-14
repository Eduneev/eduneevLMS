using MyLMS.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using UtilityClass;

namespace MyLMS.Controllers
{
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

        [HttpPost]
        public void SaveQuestion(string QuestionText)
        {
            QuestionModel QuesObj1 = new QuestionModel();
            SqlParameter[] SParam = new SqlParameter[1];

            SParam[0] = new SqlParameter("@QuestionText", SqlDbType.Int);
            SParam[0].Value = QuestionText;
           
            try
            {
              Session["QID"] = QuesObj1.SaveQuestion(SParam);
            }
            catch (Exception ex)
            {
                                
            }
        }

        [HttpPost]
        public void SaveOptions(int OptionSeq, string OptionText, int OptionMark, bool IsOptionCorrect )
        {
            QuestionModel QuesObj1 = new QuestionModel();
            SqlParameter[] SParam = new SqlParameter[5];
            SParam[0] = new SqlParameter("@QID", SqlDbType.Int);
            SParam[0].Value = Session["QID"];
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

        [HttpGet]
        public string GetQuestions()
        {
            List<QuestionOptions> CO = new List<QuestionOptions>();
            DataTable QuestionsList = DAL.GetDataTable("GetQuestions");
            SqlParameter[] SParam = new SqlParameter[1];
            List<Question> QuestionList = new List<Question>();
            

            for (int i = 0; i < QuestionsList.Rows.Count; i++)
            {
                Question Question = new Question();
                Question.QID = Convert.ToInt32(QuestionsList.Rows[i]["QID"]);
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

        [HttpPost]
        public static void SaveQuestionResponse (int RRQId, int QId, int studentId, int optionSeq) // response could be A,B,C,D or E
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

        }
    }
}