using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using UtilityClass;
using Newtonsoft.Json;
using MyLMS.Models;

namespace MyLMS.Controllers
{
    public class BillingMgmtController : Controller
    {
        // GET: BillingMgmt
        public ActionResult OrgBilling()
        {
            return View();
        }

        public ActionResult Billing()
        {
            return View();
        }

        public ActionResult BillingAgreement()
        {
            return View();
        }

        [HttpPost]
        public void CreateEntityBilling(int EntityID, int BillingTypeID, int StreamTypeID, int Cost)
        {
            BillingModel billingModel = new BillingModel();
            SqlParameter[] FObj = new SqlParameter[4];

            FObj[0] = new SqlParameter("@EntityID", SqlDbType.Int);
            FObj[0].Value = EntityID;
            FObj[1] = new SqlParameter("@BillingTypeID", SqlDbType.Int);
            FObj[1].Value = BillingTypeID;
            FObj[2] = new SqlParameter("@StreamTypeID", SqlDbType.Int);
            FObj[2].Value = StreamTypeID;
            FObj[3] = new SqlParameter("@Cost", SqlDbType.Int);
            FObj[3].Value = Cost;

            try
            {
                billingModel.CreateEntityBilling(FObj);
            }
            catch (Exception ex)
            {

            }
        }

        [HttpGet]
        public string GetStreamTypes()
        {
            DataTable ReceiversList = DAL.GetDataTable("GetStreamTypes");

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(ReceiversList);
            return JSONString;
        }

        [HttpGet]
        public string GetBillingTypes()
        {
            DataTable ReceiversList = DAL.GetDataTable("GetBillingTypes");

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(ReceiversList);
            return JSONString;
        }

        [HttpGet]
        public string GetBillingDetails()
        {
            DataTable ReceiversList = DAL.GetDataTable("GetBillingDetails");

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(ReceiversList);
            return JSONString;
        }

        [HttpGet]
        [Route("BillingMgmt/GetStreamLogsForClassroom/{id:int}/{StartDate}/{EndDate}")]
        public string GetStreamLogsForClassroom(int id, string StartDate, string EndDate)
        {
            if (StartDate == "0" && EndDate == "0")
            {
                StartDate = null;
                EndDate = null;
            }

            List<BillingModel> billingList = new List<BillingModel>();
            SqlParameter[] StreamObj = new SqlParameter[3];
            StreamObj[0] = new SqlParameter("@ClassroomID", SqlDbType.Int);
            StreamObj[0].Value = id;
            StreamObj[1] = new SqlParameter("@StartDate", SqlDbType.VarChar);
            StreamObj[1].Value = StartDate;
            StreamObj[2] = new SqlParameter("@EndDate", SqlDbType.VarChar);
            StreamObj[2].Value = EndDate;
            DataTable StreamList = DAL.GetDataTable("GetStreamLogsForClassroom", StreamObj);



            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(StreamList);
            return JSONString;
        }
    }
}