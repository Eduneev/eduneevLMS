using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace UtilityClass
{
    public class DAL
    {
        public class SQLSettings
        {
            public SqlConnection GetConnection()
            {
                SqlConnection connection = new SqlConnection(EnvSettings.ConnectionString);
                connection.Open();

                return connection;
            }
        }

        public static string ExecStringCommand(string cmd)
        {
            // SqlConnection that will be used to execute the sql commands
            SqlConnection connection = null;
            DataTable dt = new DataTable();
            string res;
            try
            {
                try
                {
                    connection = new SQLSettings().GetConnection();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                // Call ExecuteDataset static method of SqlHelper class that returns a Dataset
                //ds = SqlHelper.ExecuteDataset(connection, CommandType.StoredProcedure, storedProcedure);
                res = SqlHelper.ExecuteScalar(connection, CommandType.Text, cmd).ToString();

                //CodeTrace.Log("Method End:", MethodBase.GetCurrentMethod());
                return res;
            }
            catch (Exception ex)
            {
                string errMessage = "";
                for (Exception tempException = ex; tempException != null; tempException = tempException.InnerException)
                {
                    errMessage += tempException.Message + Environment.NewLine + Environment.NewLine;
                }
                throw new Exception(errMessage);
            }
            finally
            {
                if (connection != null)
                    connection.Dispose();
                if (dt != null)
                    dt.Dispose();
            }
        }

        public static DataTable GetDataTable(string storedProcedure)
        {
            // SqlConnection that will be used to execute the sql commands
            SqlConnection connection = null;

            DataTable dt = new DataTable();

            try
            {
                //CodeTrace.Log("Method Start:", MethodBase.GetCurrentMethod());
                try
                {
                    connection = new SQLSettings().GetConnection();
                }
                catch (Exception ex)
                {

                    throw ex;
                }

                // DataSet that will hold the returned results
                DataSet ds;

                // Call ExecuteDataset static method of SqlHelper class that returns a Dataset
                ds = SqlHelper.ExecuteDataset(connection, CommandType.StoredProcedure, storedProcedure);

                //CodeTrace.Log("Method End:", MethodBase.GetCurrentMethod());
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                string errMessage = "";
                for (Exception tempException = ex; tempException != null; tempException = tempException.InnerException)
                {
                    errMessage += tempException.Message + Environment.NewLine + Environment.NewLine;
                }

                throw new Exception(errMessage);
            }
            finally
            {
                if (connection != null)
                    connection.Dispose();

                if (dt != null)
                    dt.Dispose();
            }
        }

        /// <method>
        /// Get Data Table
        /// </method>
        public static DataTable GetDataTable(string storedProcedure, SqlParameter[] arParms)
        {
            // SqlConnection that will be used to execute the sql commands
            SqlConnection connection = null;

            DataTable dt = new DataTable();

            try
            {
                //CodeTrace.Log("Method Start:", MethodBase.GetCurrentMethod());

                try
                {
                    connection = new SQLSettings().GetConnection();
                }
                catch (Exception ex)
                {

                    throw ex;
                }

                // DataSet that will hold the returned results
                DataSet ds;

                // Call ExecuteDataset static method of SqlHelper class that returns a Dataset
                ds = SqlHelper.ExecuteDataset(connection, CommandType.StoredProcedure, storedProcedure, arParms);

                //CodeTrace.Log("Method End:", MethodBase.GetCurrentMethod());
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                }

                return dt;
            }
            catch (Exception ex)
            {
                string errMessage = "";
                for (Exception tempException = ex; tempException != null; tempException = tempException.InnerException)
                {
                    errMessage += tempException.Message + Environment.NewLine + Environment.NewLine;
                }
                throw new Exception(errMessage);
            }
            finally
            {
                if (connection != null)
                    connection.Dispose();

                if (dt != null)
                    dt.Dispose();
            }
        }
        
        public static string ExecuteScalar(string storedProcedure, SqlParameter[] arParms)
        {
            // SqlConnection that will be used to execute the sql commands
            SqlConnection connection = null;

            DataTable dt = new DataTable();

            string lv = "";
            try
            {
                //CodeTrace.Log("Method Start:", MethodBase.GetCurrentMethod());
                try
                {
                    connection = new SQLSettings().GetConnection();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                // DataSet that will hold the returned results
                //DataSet ds;

                // Call ExecuteDataset static method of SqlHelper class that returns a Dataset
                lv = SqlHelper.ExecuteScalar(connection, storedProcedure, arParms).ToString();
                return lv;
            }
            catch (Exception ex)
            {
                string errMessage = "";
                for (Exception tempException = ex; tempException != null; tempException = tempException.InnerException)
                {
                    errMessage += tempException.Message + Environment.NewLine + Environment.NewLine;
                }
                throw new Exception(errMessage);
            }
            finally
            {
                if (connection != null)
                    connection.Dispose();

                if (dt != null)
                    dt.Dispose();
            }
        }


        /// <summary>
        /// Execute Non Query Operation
        /// </summary>
        /// <returns>Execution Code</returns>
        public static int ExecuteNonQuery(string storedProcedure, SqlParameter[] arParms)
        {
            // SqlConnection that will be used to execute the sql commands
            SqlConnection connection = null;

            try
            {
                try
                {
                    connection = new SQLSettings().GetConnection();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                int returnCode = SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure, storedProcedure, arParms);
                return returnCode;
            }
            catch (Exception ex)
            {
                string errMessage = "";
                for (Exception tempException = ex; tempException != null; tempException = tempException.InnerException)
                {
                    errMessage += tempException.Message + Environment.NewLine + Environment.NewLine;
                }
                throw new Exception(errMessage);
            }
            finally
            {
                if (connection != null)
                    connection.Dispose();
            }
        }

        /// <summary>
        /// <summary>
        /// Execute SQL Commands
        /// </summary>
        public static void ExecuteSQLCommands(DataSet ds, string tableName, string addProcedure, string updateProcedure, string deleteProcedure, string[] addSourceColumns, object[] addValues, string[] updateSourceColumns, object[] updateValues, string[] deleteSourceColumns, object[] deleteValues)
        {
            // SqlConnection that will be used to execute the sql commands
            SqlConnection connection = null;

            try
            {
                try
                {
                    connection = new SQLSettings().GetConnection();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                DataTable dt = ds.Tables[tableName];

                // Add a new row to existing Data Table
                DataRow addedRow = dt.Rows.Add(addValues);

                // Create the command that will be used for insert operation
                SqlCommand insertCommand = SqlHelper.CreateCommand(connection, addProcedure, addSourceColumns);

                // Modify a row
                foreach (object obj in updateValues)
                {
                    //dt.Rows[0]["ProductName"] = "Modified product";
                }

                // Create the command that will be used for update operations
                // The stored procedure also performs a SELECT to allow updating the DataSet with other changes ***                
                SqlCommand updateCommand = SqlHelper.CreateCommand(connection, updateProcedure, updateSourceColumns);

                // Create the command that will be used for delete operations
                SqlCommand deleteCommand = SqlHelper.CreateCommand(connection, deleteProcedure, deleteSourceColumns);

                try
                {
                    // Update the data source with the DataSet changes
                    SqlHelper.UpdateDataset(insertCommand, deleteCommand, updateCommand, ds, tableName);
                }
                catch (DBConcurrencyException)
                {
                    string errMessage = "A concurrency error has ocurred while trying to update the data source; The row wasn´t updated: ";
                    throw new Exception(errMessage);
                }
            }
            catch (Exception ex)
            {
                string errMessage = "";
                for (Exception tempException = ex; tempException != null; tempException = tempException.InnerException)
                {
                    errMessage += tempException.Message + Environment.NewLine + Environment.NewLine;
                }
                throw new Exception(errMessage);
            }
            finally
            {
                if (connection != null)
                    connection.Dispose();
            }
        }

        public sealed class EnvSettings
        {
            //SQL Connection String
            public static string ConnectionString = DecryptData(ConfigurationManager.AppSettings["CONNECTIONSTRING"]);
            
            //public static string ConnectionString = "aaa";
            public static string LOGCOMMONFOLDER = @"C:\LMSLog.txt";
            //public static string TRACEFOLDER = ConfigurationSettings.AppSettings["TRACEFOLDER"];
            public static string ERRORFOLDER = @"C:\LMS Revamp";
            //public static string APPTRACEFOLDER = ConfigurationSettings.AppSettings["APPLICATIONTRACEFOLDER"];
            public static bool ERRORLEVEL = (bool)(Convert.ToBoolean(true));
            public static string SMTPSERVER = ConfigurationManager.AppSettings["SMTPSERVER"];

            /// </method>
            public static string DecryptData(string data)
            {
                try
                {
                    //Have a Key
                    byte[] myKey = Encoding.ASCII.GetBytes("ActisKey");
                    //Have a vector required  to create a Crypto stream 
                    byte[] myVector = Encoding.ASCII.GetBytes("ActisKey");

                    //Create a Service Provider object
                    DESCryptoServiceProvider myCryptoProvider = new DESCryptoServiceProvider();
                    //Create a memory strem 
                    MemoryStream myMemoryStream = new MemoryStream(Convert.FromBase64String(data));
                    //Create a Cryptro memory stream
                    CryptoStream myCryptoStream = new CryptoStream(myMemoryStream, myCryptoProvider.CreateDecryptor(myKey, myVector), CryptoStreamMode.Read);
                    //HAve a reader
                    StreamReader myReader = new StreamReader(myCryptoStream);

                    return myReader.ReadToEnd();
                }
                catch (Exception ex)
                {
                    //Error.ReportException(ex, true);
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Get the scalar object
        /// </summary>
        /// <returns>Object</returns>
        public static object GetScalar(string storedProcedure, SqlParameter[] arParms)
        {
            // SqlConnection that will be used to execute the sql commands
            SqlConnection connection = null;

            try
            {
                //CodeTrace.Log("Method Start:", MethodBase.GetCurrentMethod());

                try
                {
                    connection = new SQLSettings().GetConnection();
                }
                catch (Exception ex)
                {
                    //Error.ReportErrorBrief("ERR0001", "SQL Connection Failure", "Application Error");
                    throw ex;
                }

                object obj = SqlHelper.ExecuteScalar(connection, CommandType.StoredProcedure, storedProcedure, arParms);

                //CodeTrace.Log("Method End:", MethodBase.GetCurrentMethod());
                return obj;
            }
            catch (Exception ex)
            {
                string errMessage = "";
                for (Exception tempException = ex; tempException != null; tempException = tempException.InnerException)
                {
                    errMessage += tempException.Message + Environment.NewLine + Environment.NewLine;
                }

                throw new Exception(errMessage);
            }
            finally
            {
                if (connection != null)
                    connection.Dispose();
            }
        }
    }
}
