using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kvanto.Classes
{
    public class Check
    {
        public static bool ItsNumber(string str)
        {
            if (!str.Any(c => char.IsDigit(c)))
                return false;
            return true;
        }
        public static bool IsAllLetters(string s)
        {
            foreach (char c in s)
            {
                if (!Char.IsLetter(c))
                    return false;
            }
            return true;
        }
        public static bool ItsOnlyFIO(string str)
        {
            char[] fioChar = str.ToCharArray();
            if (str.Split(' ').Length != 3)
                return false;
            if (!str.Any(c => char.IsLetter(c)))
                return false;
            if (!char.IsUpper(fioChar[0]) && !char.IsUpper(fioChar[1]) && !char.IsUpper(fioChar[2]))
                return false;
            return true;
        }
        public static bool TextIsDate(string text)
        {
            var dateFormat = "dd-MM-yyyy";
            DateTime scheduleDate;
            if (DateTime.TryParseExact(text, dateFormat, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out scheduleDate))
            {
                return true;
            }
            return false;
        }
        public static bool ItsNumberTel(string str)
        {

            bool isflag = false;
            string isnum = "1234567890";
            if (str != "")
            {
                string write = str.ToLower();
                for (int i = 0; i < write.Length; i++)
                {
                    isflag = false;
                    for (int j = 0; j < isnum.Length; j++)
                    {
                        if (write[i] == isnum[j])
                        {
                            isflag = true;
                        }
                    }
                }
            }
            return isflag;
        }
    }
}
