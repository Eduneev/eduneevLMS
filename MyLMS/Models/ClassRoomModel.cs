using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using UtilityClass;

namespace MyLMS.Models
{
    public class ClassRoomModel
    {
        public int ClassRoomId { get; set; }
        public string ClassRoomName { get; set; }
        public int CenterId { get; set; }
        public string CenterName { get; set; }
        public string LastUsedCommand { get; set; }

        public string SaveClassRoom(SqlParameter[] sparams)
        {
            string res = "Failure..";
            try
            {
                res = DAL.ExecuteScalar("SaveClassroom", sparams);
            }
            catch (Exception ex)
            {
                res = ex.Message;
                throw;
            }
            return res;
        }

        public string DeleteClassRoom(SqlParameter[] sparams)
        {
            string res = "Failure..";
            try
            {
                res = DAL.ExecuteScalar("DeleteClassroom", sparams);
            }
            catch (Exception ex)
            {
                res = ex.Message;
                throw;
            }
            return res;
        }
    }

}