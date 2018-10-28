using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityClass
{
    public static class CodeHelper
    {
        // Calculate Total Pages
        public static int CalculateTotalPages(long numberOfRecords, Int32 pageSize)
        {
            long result;
            int totalPages;
            Math.DivRem(numberOfRecords, pageSize, out result);
            if (result > 0)
                totalPages = (int)((numberOfRecords / pageSize)) + 1;
            else
                totalPages = (int)(numberOfRecords / pageSize);
            return totalPages;
        }

        // Generate Random Number
        public static int GenerateRandomNumber(int max)
        {
            Random rnd = new Random();
            int randomNumber = rnd.Next(max);
            return randomNumber;
        }

        // Make first letter of every word to Uppercase
        public static string UppercaseFirstLetter(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            StringBuilder output = new StringBuilder();
            string[] words = s.Split(' ');
            foreach (string word in words)
            {
                char[] a = word.ToCharArray();
                a[0] = char.ToUpper(a[0]);
                string b = new string(a);
                output.Append(b + " ");
            }
            return output.ToString().Trim();
        }

        public static int ConvertToInt(object input)
        {
            if (input != null && input != DBNull.Value && input != string.Empty)
            {
                return Convert.ToInt32(input);
            }
            return 0;
        }
        public static Int64 ConvertToInt64(object input)
        {
            if (input != null && input != DBNull.Value && input != string.Empty)
            {
                return Convert.ToInt64(input);
            }
            return 0;
        }
        public static double ConvertToDouble(object input)
        {
            if (input != null && input != DBNull.Value && input != string.Empty)
            {
                return Convert.ToDouble(input);
            }
            return 0.0;
        }

        public static bool ConvertToBool(object input)
        {
            if (input != null && input != DBNull.Value && input != string.Empty)
            {
                return Convert.ToBoolean(input);
            }
            return false;
        }

        public static DateTime ConvertToDateTime(object input)
        {
            if (input != null && input != DBNull.Value && input != string.Empty)
            {
                return Convert.ToDateTime(input);
            }
            return DateTime.MinValue;
        }

        public static decimal RoundUp(decimal input)
        {
            int RoundUpto = 5;
            return Math.Round(input, RoundUpto);
        }
        public static double RoundUp(double input)
        {
            int RoundUpto = 5;
            return Math.Round(input, RoundUpto);
        }
        public static double RoundUp(double input, int RoundUpto)
        {
            return Math.Round(input, RoundUpto);
        }
        public static decimal RoundUp(decimal input, int RoundUpto)
        {
            return Math.Round(input, RoundUpto);
        }

        public static decimal ConvertToDecimal(object input)
        {
            if (input != null && input != DBNull.Value && input != string.Empty)
            {
                return Convert.ToDecimal(input);
            }
            return 0.0M;
        }
    }
}
