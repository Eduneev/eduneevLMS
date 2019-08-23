using MyLMS.Models;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using UtilityClass;
using System.Text.RegularExpressions;
using Amazon.S3;
using Amazon;
using System.Threading.Tasks;
using Amazon.S3.Model;
using RestSharp;
using RestSharp.Authenticators;
using System.IO.Compression;
using System.Collections.Generic;

namespace MyLMS.Controllers
{
    [SessionExpire]
    public class StudentMgmtController : Controller
    {
        //private new ZipArchive();

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

        public ActionResult RRQManualEntry()
        {
            return View();
        }

        public ActionResult EditStudent()
        {
            return View();
        }

        public static String bucket = "sanats";
        public static String url = "https://s3-us-west-2.amazonaws.com/";
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.USWest2;
        private static IAmazonS3 s3Client;

        public StudentMgmtController()
        {
            s3Client = new AmazonS3Client(bucketRegion);
        }

        [HttpPost]
        public void SaveStudent(string StudentName, string Code,int ProgramID, int CourseID, int SubjectID, string Email, string Mobile, string Landline, string Address, string Pincode, string Gender, string BirthPlace, string SchoolName, string GuardianName, string GuardianContactNo)
        {
            StudentModel ModelObj1 = new StudentModel();
            SqlParameter[] SParam = new SqlParameter[15];

            SParam[0] = new SqlParameter("@StudentName", SqlDbType.VarChar);
            SParam[0].Value = StudentName;
            SParam[1] = new SqlParameter("@Code", SqlDbType.VarChar);
            SParam[1].Value = Code;
            SParam[2] = new SqlParameter("@ProgID", SqlDbType.Int);
            SParam[2].Value = ProgramID;
            SParam[3] = new SqlParameter("@CourseID", SqlDbType.Int);
            SParam[3].Value = CourseID;
            SParam[4] = new SqlParameter("@SubjectID", SqlDbType.Int);
            SParam[4].Value = SubjectID;
            SParam[5] = new SqlParameter("@Email", SqlDbType.VarChar);
            SParam[5].Value = Email;
            SParam[6] = new SqlParameter("@Mobile", SqlDbType.VarChar);
            SParam[6].Value = Mobile;
            SParam[7] = new SqlParameter("@Landline", SqlDbType.VarChar);
            SParam[7].Value = Landline;
            SParam[8] = new SqlParameter("@Address", SqlDbType.Text);
            SParam[8].Value = Address;
            SParam[9] = new SqlParameter("@Pincode", SqlDbType.VarChar);
            SParam[9].Value = Pincode;
            SParam[10] = new SqlParameter("@Gender", SqlDbType.VarChar);
            SParam[10].Value = Gender;
            SParam[11] = new SqlParameter("@UserID", SqlDbType.Int);
            SParam[11].Value = Convert.ToInt32(Session["USER_ID"]);
            SParam[12] = new SqlParameter("@SchoolName", SqlDbType.VarChar);
            SParam[12].Value = SchoolName;
            SParam[13] = new SqlParameter("@GuardianName", SqlDbType.VarChar);
            SParam[13].Value = GuardianName;
            SParam[14] = new SqlParameter("@GuardianContactNo", SqlDbType.VarChar);
            SParam[14].Value = GuardianContactNo;
            try
            {
                ModelObj1.SaveStudent(SParam);
            }
            catch (Exception ex)
            {

            }
        }

        [HttpPost]
        public void EditStudent(int StudentID, string StudentName, string Code, int ProgramID, int CourseID, int SubjectID, string Email, string Mobile, string Landline, string Address, string Pincode, string Gender, string BirthPlace, string SchoolName, string GuardianName, string GuardianContactNo)
        {
            StudentModel ModelObj1 = new StudentModel();
            SqlParameter[] SParam = new SqlParameter[16];

            SParam[0] = new SqlParameter("@StudentName", SqlDbType.VarChar);
            SParam[0].Value = StudentName;
            SParam[1] = new SqlParameter("@Code", SqlDbType.VarChar);
            SParam[1].Value = Code;
            SParam[2] = new SqlParameter("@ProgID", SqlDbType.Int);
            SParam[2].Value = ProgramID;
            SParam[3] = new SqlParameter("@CourseID", SqlDbType.Int);
            SParam[3].Value = CourseID;
            SParam[4] = new SqlParameter("@SubjectID", SqlDbType.Int);
            SParam[4].Value = SubjectID;
            SParam[5] = new SqlParameter("@Email", SqlDbType.VarChar);
            SParam[5].Value = Email;
            SParam[6] = new SqlParameter("@Mobile", SqlDbType.VarChar);
            SParam[6].Value = Mobile;
            SParam[7] = new SqlParameter("@Landline", SqlDbType.VarChar);
            SParam[7].Value = Landline;
            SParam[8] = new SqlParameter("@Address", SqlDbType.Text);
            SParam[8].Value = Address;
            SParam[9] = new SqlParameter("@Pincode", SqlDbType.VarChar);
            SParam[9].Value = Pincode;
            SParam[10] = new SqlParameter("@Gender", SqlDbType.VarChar);
            SParam[10].Value = Gender;
            SParam[11] = new SqlParameter("@UserID", SqlDbType.Int);
            SParam[11].Value = Convert.ToInt32(Session["USER_ID"]);
            SParam[12] = new SqlParameter("@SchoolName", SqlDbType.VarChar);
            SParam[12].Value = SchoolName;
            SParam[13] = new SqlParameter("@GuardianName", SqlDbType.VarChar);
            SParam[13].Value = GuardianName;
            SParam[14] = new SqlParameter("@GuardianContactNo", SqlDbType.VarChar);
            SParam[14].Value = GuardianContactNo;
            SParam[15] = new SqlParameter("@StudentID", SqlDbType.Int);
            SParam[15].Value = StudentID;
            try
            {
                ModelObj1.EditStudent(SParam);
            }
            catch (Exception ex)
            {

            }
        }

        [HttpPost]
        public void DeleteStudent(int StudentID)
        {
            StudentModel ModelObj1 = new StudentModel();
            SqlParameter[] FObj = new SqlParameter[2];
            FObj[0] = new SqlParameter("@UserID", SqlDbType.Int);
            FObj[0].Value = Convert.ToInt32(Session["USER_ID"]);
            FObj[1] = new SqlParameter("@StudentID", SqlDbType.Int);
            FObj[1].Value = StudentID;
            try
            {
                ModelObj1.DeleteStudent(FObj);
            }
            catch (Exception ex)
            {            
            }
        }

        [HttpGet]
        public string GenerateStudentCode()
        {
            SqlParameter[] FObj = new SqlParameter[1];
            FObj[0] = new SqlParameter("@UserID", SqlDbType.Int);
            FObj[0].Value = Convert.ToInt32(Session["USER_ID"]);
            DataTable StudentsList = DAL.GetDataTable("GenerateStudentCode", FObj);

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(StudentsList);
            return JSONString;
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
        public string GetStudentsBySubject(int id)
        {
            SqlParameter[] FObj = new SqlParameter[2];
            FObj[0] = new SqlParameter("@UserID", SqlDbType.Int);
            FObj[0].Value = Convert.ToInt32(Session["USER_ID"]);
            FObj[1] = new SqlParameter("SubjectID", SqlDbType.Int);
            FObj[1].Value = id;
            DataTable StudentsList = DAL.GetDataTable("GetStudentsBySubject", FObj);

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(StudentsList);
            return JSONString;
        }

        [HttpGet]
        [Route("StudentMgmt/GetStudentsByCode/{StudentCode}")]
        public string GetStudentsByStudentCode(string StudentCode)
        {
            SqlParameter[] FObj = new SqlParameter[2];
            FObj[0] = new SqlParameter("@UserID", SqlDbType.Int);
            FObj[0].Value = Convert.ToInt32(Session["USER_ID"]);
            FObj[1] = new SqlParameter("Code", SqlDbType.VarChar);
            FObj[1].Value = StudentCode;
            DataTable StudentsList = DAL.GetDataTable("GetStudentsByCode", FObj);

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(StudentsList);
            return JSONString;
        }

        [HttpPost]
        // Only called by the EditStudent page
        public void SaveNewStudentPhoto(int StudentID, string Path)
        {
            SqlParameter[] FObj = new SqlParameter[2];
            FObj[0] = new SqlParameter("@StudentID", SqlDbType.Int);
            FObj[0].Value = StudentID;
            FObj[1] = new SqlParameter("@Path", SqlDbType.VarChar);
            FObj[1].Value = Path;

            StudentModel s = new StudentModel();
            try
            {
                s.SaveStudentImage(FObj);
            }
            catch
            {

            }
        }

        [HttpPost]
        public void SetStudentPhoto(int StudentID, string StudentImage)
        {
            // Server loses images every time on restart. Need to find a way to not let this happen.
            string cleanImage = Regex.Replace(StudentImage, "data:image/png;base64,", "");
            byte[] encodedDataAsBytes = System.Convert.FromBase64String(cleanImage);
            System.IO.Stream stream = new System.IO.MemoryStream(encodedDataAsBytes);
            
            string Path = url + bucket + "/" + StudentID + ".jpg";
            //System.IO.File.WriteAllBytes(Path, encodedDataAsBytes);
            UploadStudentImageAsync(stream, StudentID);

            SqlParameter[] FObj = new SqlParameter[2];
            FObj[0] = new SqlParameter("@StudentID", SqlDbType.Int);
            FObj[0].Value = StudentID;
            FObj[1] = new SqlParameter("@Path", SqlDbType.VarChar);
            FObj[1].Value = Path;

            StudentModel s = new StudentModel();
            try
            {
                s.SaveStudentImage(FObj);
            }
            catch
            {

            }
        }
        private static async Task UploadStudentImageAsync(System.IO.Stream Image, int StudentID)
        {
            try
            {                
                var putRequest1 = new PutObjectRequest
                {
                    BucketName = bucket,
                    Key = StudentID.ToString() + ".jpg",
                    InputStream = Image,
                    ContentType = "image/jpg"
                };

                PutObjectResponse response1 = await s3Client.PutObjectAsync(putRequest1);
                //await fileTransferUtility.UploadAsync(Image, bucket, StudentID.ToString());
                Console.WriteLine("Upload completed");

            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
            }

        }

        [HttpPost]
        public void SetStudentID(int StudentID)
        {
            Session["StudentID"] = StudentID;
        }

        [HttpGet]
        public string GetStudentsForAttendance(int id)
        {
            SqlParameter[] FObj = new SqlParameter[2];
            FObj[0] = new SqlParameter("@UserID", SqlDbType.Int);
            FObj[0].Value = Convert.ToInt32(Session["USER_ID"]);
            FObj[1] = new SqlParameter("@SessionID", SqlDbType.Int);
            FObj[1].Value = id;
            DataTable StudentsList = DAL.GetDataTable("GetStudentsForAttendance", FObj);

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(StudentsList);
            return JSONString;
        }

        [HttpGet]
        [Route("StudentMgmt/GetStudentsForRemoteAllocation/{SubjectID:int}/{ClassroomID:int}")]
        public string GetStudentsForRemoteAllocation(int SubjectID, int ClassroomID)
        {
            SqlParameter[] FObj = new SqlParameter[3];
            FObj[0] = new SqlParameter("@SubjectID", SqlDbType.Int);
            FObj[0].Value = SubjectID;
            FObj[1] = new SqlParameter("@UserID", SqlDbType.Int);
            FObj[1].Value = Convert.ToInt32(Session["USER_ID"]);
            FObj[2] = new SqlParameter("@ClassRoomID", SqlDbType.Int);
            FObj[2].Value = ClassroomID;
            DataTable StudentsList = DAL.GetDataTable("GetStudentsForRemoteAllocation", FObj);

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(StudentsList);
            return JSONString;
        }

        [HttpPost]
        public void AssignRemoteToStudent(int StudentID, string RemoteNumber, int SubjectID, int ClassroomID)
        {
            StudentModel ModelObj1 = new StudentModel();
            SqlParameter[] SParam = new SqlParameter[4];

            SParam[0] = new SqlParameter("@StudentID", SqlDbType.Int);
            SParam[0].Value = StudentID;
            SParam[1] = new SqlParameter("@RemoteNumber", SqlDbType.VarChar);
            SParam[1].Value = RemoteNumber;
            SParam[2] = new SqlParameter("@SubjectID", SqlDbType.Int);
            SParam[2].Value = SubjectID;
            SParam[3] = new SqlParameter("@ClassRoomID", SqlDbType.Int);
            SParam[3].Value = ClassroomID;

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

        [HttpGet]
        public string GenerateRRQStudentReports(int id)
        {
            int RRQID = id;
            RRQDetails RRQ = new RRQDetails();
            List<StudentReport> StudentReports = new List<StudentReport>();
            RRQReport RRQReport = new RRQReport();

            SqlParameter[] FObj = new SqlParameter[1];
            FObj[0] = new SqlParameter("@RRQ_ID", SqlDbType.Int);
            FObj[0].Value = RRQID;
            DataTable RRQQuestions = DAL.GetDataTable("GetRRQQuestions", FObj);

            // Construct RRQ Details 
            RRQ.TotalMarks = 0;
            RRQ.Questions = new List<Option>();
            for (int i=0; i < RRQQuestions.Rows.Count; i++)
            {
                RRQ.RRQID = Convert.ToInt32(Convert.IsDBNull(RRQQuestions.Rows[i]["RRQ_ID"]) ? 0 : RRQQuestions.Rows[i]["RRQ_ID"]);
                RRQ.RRQNo = Convert.ToString(Convert.IsDBNull(RRQQuestions.Rows[i]["RRQName"]) ? string.Empty : RRQQuestions.Rows[i]["RRQName"]);
                int mark = Convert.ToInt32(Convert.IsDBNull(RRQQuestions.Rows[i]["Mark"]) ? 0 : RRQQuestions.Rows[i]["Mark"]);
                RRQ.TotalMarks += mark;
                Option Option = new Option();
                Option.QID = Convert.ToInt32(Convert.IsDBNull(RRQQuestions.Rows[i]["QID"]) ? 0 : RRQQuestions.Rows[i]["QID"]);
                Option.OptionChar = Convert.ToString(Convert.IsDBNull(RRQQuestions.Rows[i]["OptionChar"]) ? string.Empty : RRQQuestions.Rows[i]["OptionChar"]);
                Option.Mark = mark;
                RRQ.Questions.Add(Option);
            }

            // Construct Student Responses
            FObj = new SqlParameter[1];
            FObj[0] = new SqlParameter("@RRQ_ID", SqlDbType.Int);
            FObj[0].Value = RRQID;
            DataTable Reports = DAL.GetDataTable("GetStudentReports", FObj);

            Dictionary<int, int> StudentMapper = new Dictionary<int, int>();
            Dictionary<int, Dictionary<int, Option>> StudentResponses = new Dictionary<int, Dictionary<int, Option>>();
            for (int i=0; i < Reports.Rows.Count; i++)
            {
                int StudentID = Convert.ToInt32(Convert.IsDBNull(Reports.Rows[i]["StudentID"]) ? 0 : Reports.Rows[i]["StudentID"]);
                if (StudentMapper.ContainsKey(StudentID))
                {
                    // Existing Student - append to the dictionary
                    Option Option = new Option();
                    Option.QID = Convert.ToInt32(Convert.IsDBNull(Reports.Rows[i]["QID"]) ? 0 : Reports.Rows[i]["QID"]);
                    Option.OptionChar = Convert.ToString(Convert.IsDBNull(Reports.Rows[i]["Response"]) ? string.Empty : Reports.Rows[i]["Response"]);
                    Option.Mark = Convert.ToInt32(Convert.IsDBNull(Reports.Rows[i]["Mark"]) ? 0 : Reports.Rows[i]["Mark"]);
                    StudentResponses[StudentID].Add(Option.QID, Option);
                }
                else
                {
                    // Create new Student
                    StudentReport Student = new StudentReport();
                    Student.StudentID = StudentID;
                    Student.StudentName = Convert.ToString(Convert.IsDBNull(Reports.Rows[i]["StudentName"]) ? string.Empty : Reports.Rows[i]["StudentName"]);
                    Student.Email = Convert.ToString(Convert.IsDBNull(Reports.Rows[i]["StudentEmail"]) ? string.Empty : Reports.Rows[i]["StudentEmail"]);
                    Student.TotalMarks = Convert.ToInt32(Convert.IsDBNull(Reports.Rows[i]["TotalMarks"]) ? 0 : Reports.Rows[i]["TotalMarks"]);
                    Option Option = new Option();
                    Option.QID = Convert.ToInt32(Convert.IsDBNull(Reports.Rows[i]["QID"]) ? 0 : Reports.Rows[i]["QID"]);
                    Option.OptionChar = Convert.ToString(Convert.IsDBNull(Reports.Rows[i]["Response"]) ? string.Empty : Reports.Rows[i]["Response"]);
                    Option.IsCorrect = Convert.ToBoolean(Convert.IsDBNull(Reports.Rows[i]["IsCorrect"]) ? 0 : Reports.Rows[i]["IsCorrect"]);
                    Option.Mark = Convert.ToInt32(Convert.IsDBNull(Reports.Rows[i]["Mark"]) ? 0 : Reports.Rows[i]["Mark"]);
                    StudentResponses[StudentID] = new Dictionary<int, Option> { { Option.QID, Option } };
                    StudentReports.Add(Student);
                    // Add to StudentMapper
                    StudentMapper.Add(StudentID, StudentReports.Count - 1);
                }
            }

            // Loop through each Student and Add N/A for all questions that are unanswered
            for (int i=0; i < StudentReports.Count; i++)
            {
                int StudentID = StudentReports[i].StudentID;
                List<Option> Responses = new List<Option>();
                foreach (Option Option in RRQ.Questions)
                {
                    if (StudentResponses[StudentID].ContainsKey(Option.QID))
                    {
                        Responses.Add(StudentResponses[StudentID][Option.QID]);
                    }
                    else
                    {
                        Option o = new Option();
                        o.OptionChar = "NA";
                        o.IsCorrect = false;
                        o.QID = Option.QID;
                        Responses.Add(o);
                    }
                }
                StudentReports[i].Responses = Responses;
            }

            // Construct the RRQReport Object
            RRQReport.RRQ = RRQ;
            RRQReport.StudentReports = StudentReports;

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(RRQReport);
            return JSONString;
        }

        // Send RRQ Reports to all Students
        public static IRestResponse SendSimpleMessage()
        {
            RestClient client = new RestClient();
            client.BaseUrl = new Uri("https://api.mailgun.net/v3"); 

            client.Authenticator = new HttpBasicAuthenticator("api", "YOUR_API_KEY");
            RestRequest request = new RestRequest();
            request.AddParameter("domain", "eduneev.in", ParameterType.UrlSegment);
            request.Resource = "{domain}/messages";
            request.AddParameter("from", "mailgun@YOUR_DOMAIN_NAME>");
            request.AddParameter("to", "bar@example.com");
            request.AddParameter("to", "YOU@YOUR_DOMAIN_NAME");
            request.AddParameter("subject", "Hello");
            request.AddParameter("text", "Testing some Mailgun awesomness!");
            request.Method = Method.POST;
            return client.Execute(request);
        }
    }
}