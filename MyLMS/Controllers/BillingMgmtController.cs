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

            List<BillingModel> BillingList = new List<BillingModel>();
            SqlParameter[] StreamObj = new SqlParameter[3];
            StreamObj[0] = new SqlParameter("@ClassroomID", SqlDbType.Int);
            StreamObj[0].Value = id;
            StreamObj[1] = new SqlParameter("@StartDate", SqlDbType.VarChar);
            StreamObj[1].Value = StartDate;
            StreamObj[2] = new SqlParameter("@EndDate", SqlDbType.VarChar);
            StreamObj[2].Value = EndDate;
            DataTable StreamList = DAL.GetDataTable("GetStreamLogsForClassroom", StreamObj);

            for (int i = 0; i < StreamList.Rows.Count; i++)
            {
                // For each stream, if xduration is not null, check the stream type and then retrieve the cost for that 
                // combination. (EntityID, BillingTypeID, StreamTypeID). 
                // Once cost is retrieved, assign amount as Ceiling(xduration/xfactor)*cost
                BillingModel Bill = new BillingModel();
                Bill.Client = Convert.ToInt32(StreamList.Rows[i]["Client"]);
                Bill.Bytes = StreamList.Rows[i]["Bytes"].ToString();
                Bill.Date = StreamList.Rows[i]["date"].ToString();
                Bill.Stream = StreamList.Rows[i]["Stream"].ToString();

                string xduration = StreamList.Rows[i]["Duration"].ToString();
                int EntityID = Convert.ToInt32(StreamList.Rows[i]["EntityID"]);
                int BillingTypeID = Convert.ToInt32(StreamList.Rows[i]["BillingTypeID"]);

                if (xduration != null)
                {
                    double Duration = Convert.ToDouble(xduration)/60.0;
                    int BillingFactor = Convert.ToInt32(StreamList.Rows[i]["BillingFactor"]);
                    Bill.Duration = (int)Math.Ceiling(Duration / BillingFactor) * BillingFactor;
                    int StreamTypeID=0;

                    // AUTOMATE THE BELOW STUFF RATHER THAN HARDCODE VALUES
                    string StreamEnd = Bill.Stream.Substring(Bill.Stream.Length - 5);
                    if (StreamEnd.Contains("SSD"))
                        StreamTypeID = 1;
                    else if (StreamEnd.Contains("DVD"))
                        StreamTypeID = 2;
                    else if (StreamEnd.Contains("FHD"))
                        StreamTypeID = 4;
                    else if (StreamEnd.Contains("HD"))
                        StreamTypeID = 3;

                    SqlParameter[] FObj = new SqlParameter[3];
                    FObj[0] = new SqlParameter("@EntityID", SqlDbType.Int);
                    FObj[0].Value = EntityID;
                    FObj[1] = new SqlParameter("@BillingTypeID", SqlDbType.Int);
                    FObj[1].Value = BillingTypeID;
                    FObj[2] = new SqlParameter("@StreamTypeID", SqlDbType.Int);
                    FObj[2].Value = StreamTypeID;

                    DataTable C = DAL.GetDataTable("GetBillingCost", FObj);
                    if (C.Rows.Count > 0)
                    {
                        int Cost = Convert.ToInt32(C.Rows[0]["Cost"]);
                        Bill.Amount = (int)Math.Ceiling(Duration / BillingFactor) * Cost;
                    }
                }
                BillingList.Add(Bill);
            }

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(BillingList);
            return JSONString;
        }
    }
}