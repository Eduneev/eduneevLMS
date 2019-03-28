using MyLMS.Models;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using UtilityClass;
using System.Text.RegularExpressions;
using Amazon.S3;
using Amazon.S3.Transfer;
using Amazon;
using System.Threading.Tasks;
using Amazon.S3.Model;
using System.Diagnostics;

namespace MyLMS.Controllers
{
    [SessionExpire]
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

        public ActionResult RRQManualEntry()
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
    }
}