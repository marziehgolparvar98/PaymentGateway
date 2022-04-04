using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

using static System.String;

namespace PaymentGateway.Common
{
    public static class CaptchaHelper
    {
        public static bool Validate(CaptchaItem captcha)
        {
            if (captcha == null) return false;
            if (IsNullOrEmpty(captcha.Value) ||
                IsNullOrEmpty(captcha.Value) ||
                IsNullOrEmpty(captcha.Value))
                return false;

            using (var sha256 = SHA256.Create())
            {
                var @now = DateTime.Now;
                var timeString = Format("{0}-{1}-{2} {3}:{4}",
                                    @now.Year,
                                    @now.Month,
                                    @now.Day,
                                    now.Minute < 10 ? captcha.Hash.Substring(0, 2) : now.Hour.ToString("00"),//hour
                                    now.Minute % 10 == 0 ? captcha.Hash.Substring(3, 2) : (now.Minute - now.Minute % 10).ToString("00"));//minute

                var saltedValue = Encoding.UTF8
                                          .GetBytes(Safe(captcha.Value))
                                          .Concat(Encoding.UTF8.GetBytes(captcha.Salt + timeString))
                                          .ToArray();
                // Send a simple text to hash.  
                var firstHashedBytes = sha256.ComputeHash(saltedValue);
                var secondHashedBytes = sha256.ComputeHash(firstHashedBytes);
                var thirdHashedBytes = sha256.ComputeHash(secondHashedBytes);
                // Get the hashed string.  
                return BitConverter.ToString(thirdHashedBytes) == captcha.Hash.Substring(6);
            }
        }
        private static string Safe(string str)
        {
            if (IsNullOrEmpty(str))
                return Empty;
            var regex = new Regex("[۰۱۲۳۴۵۶۷۸۹]");
            var dictNumbers = new Dictionary<string, string>{
                {"۰", "0" },
                {"۱", "1" },
                {"۲", "2" },
                {"۳", "3" },
                {"۴", "4" },
                {"۵", "5" },
                {"۶", "6" },
                {"۷", "7" },
                {"۸", "8" },
                {"۹", "9" },

            };
            str = regex.Replace(str, (m) =>
            {
                return dictNumbers[m.Value];
            });
            return str.ReplaceYK().Trim();
        }

        public static string ReplaceYK(this string obj, bool trim = true)
        {


            const char ARABI_Y1 = (char)1609;//'ى'
            const char ARABI_Y2 = (char)1610;//'ي'
            const char FARSI_Y = (char)1740;//'ی'
            const char ARABI_K = (char)1603;//'ك'
            const char FARSI_K = (char)1705;//'ک'
            var retVal = IsNullOrEmpty(obj) ? obj : obj.Replace(ARABI_Y1, FARSI_Y).Replace(ARABI_Y2, FARSI_Y).Replace(ARABI_K, FARSI_K);
            if (trim && !IsNullOrEmpty(retVal))
            {
                retVal = retVal.Trim();
            }
            return retVal;
        }
    }
}
