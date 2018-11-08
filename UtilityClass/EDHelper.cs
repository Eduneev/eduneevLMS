using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace UtilityClass
{
    public class EDHelper
    {
        public static String sKey = "2616508101191";
        
        public static string DecryptData(string STRING)
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
                MemoryStream myMemoryStream = new MemoryStream(Convert.FromBase64String(STRING));
                //Create a Cryptro memory stream
                CryptoStream myCryptoStream = new CryptoStream(myMemoryStream, myCryptoProvider.CreateDecryptor(myKey, myVector), CryptoStreamMode.Read);
                //HAve a reader
                StreamReader myReader = new StreamReader(myCryptoStream);
                return myReader.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string nEncryptData(string STRING)
        {
            int Key = 1;
            byte[] byteArr = Encoding.ASCII.GetBytes(STRING);
            string encodedStr = "";
            // right circular shift
            for (int i = 0; i < byteArr.Length; i++)
            {
                byteArr[i] = (byte)(byteArr[i] >> Key | byteArr[i] << (8 - Key));
            }
            encodedStr = Convert.ToBase64String(byteArr);
            return encodedStr;
        }

        public static string nDecryptData(string STRING)
        {
            int Key = 1;
            byte[] byteArr = Convert.FromBase64String(STRING);
            string decodedStr = "";
            // left circular shift
            for (int i = 0; i < byteArr.Length; i++)
            {
                byteArr[i] = (byte)(byteArr[i] << Key | byteArr[i] >> (8 - Key));
            }
            decodedStr = Encoding.ASCII.GetString(byteArr);
            decodedStr = decodedStr.Trim();
            return decodedStr;
        }

        public static string EncryptTripleDES(String sIn)
        {
            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider hashMD5 = new MD5CryptoServiceProvider();
            
            if (sIn != null)
            {
                if (sIn.Trim() != "")
                {
                    des.Key = hashMD5.ComputeHash(UTF8Encoding.UTF8.GetBytes(sKey));
                    des.Mode = CipherMode.ECB;
                    ICryptoTransform DESEncrypt = des.CreateEncryptor();
                    Byte[] buffer = UTF8Encoding.UTF8.GetBytes(sIn);
                    return Convert.ToBase64String(DESEncrypt.TransformFinalBlock(buffer, 0, buffer.Length));
                }
                else
                    return sIn;
            }
            else
            {
                return "";
            }
        }

        public static string DecryptTripleDES(String sOut)
        {
            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider hashMD5 = new MD5CryptoServiceProvider();

            if (sOut != null)
            {
                if (sOut.Trim() != "")
                {
                    des.Key = hashMD5.ComputeHash(UTF8Encoding.UTF8.GetBytes(sKey));
                    des.Mode = CipherMode.ECB;
                    ICryptoTransform DESDecrypt = des.CreateDecryptor();
                    Byte[] buffer = Convert.FromBase64String(sOut);
                    return UTF8Encoding.UTF8.GetString(DESDecrypt.TransformFinalBlock(buffer, 0, buffer.Length));
                }
                else
                    return sOut;
            }
            else
            {
                return "";
            }
        }
    }
}
