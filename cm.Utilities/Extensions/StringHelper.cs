using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace cm.Utilities.Extensions
{
    public static class StringHelper
    {
        public static string ToUnsignString(this string input)
        {
            input = input.Trim();
            for (int i = 0x20; i < 0x30; i++)
            {
                input = input.Replace(((char)i).ToString(), " ");
            }
            input = input.Replace(".", "-");
            input = input.Replace(" ", "-");
            input = input.Replace(",", "-");
            input = input.Replace(";", "-");
            input = input.Replace(":", "-");
            input = input.Replace("  ", "-");
            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
            string str = input.Normalize(NormalizationForm.FormD);
            string str2 = regex.Replace(str, string.Empty).Replace('đ', 'd').Replace('Đ', 'D');
            while (str2.IndexOf("?") >= 0)
            {
                str2 = str2.Remove(str2.IndexOf("?"), 1);
            }
            while (str2.Contains("--"))
            {
                str2 = str2.Replace("--", "-").ToLower();
            }
            return str2;
        }

        public static string EncryptPlainTextToCipherText(string PlainText, string SecurityKey)
        {
            byte[] toEncryptedArray = UTF8Encoding.UTF8.GetBytes(PlainText);

            SHA256Managed objMD5CryptoService = new SHA256Managed();
            byte[] securityKeyArray = objMD5CryptoService.ComputeHash(UTF8Encoding.UTF8.GetBytes(SecurityKey));
            objMD5CryptoService.Clear();

            var objTripleDESCryptoService = new AesCryptoServiceProvider();
            objTripleDESCryptoService.Key = securityKeyArray;
            objTripleDESCryptoService.Mode = CipherMode.ECB;
            objTripleDESCryptoService.Padding = PaddingMode.PKCS7;

            var objCrytpoTransform = objTripleDESCryptoService.CreateEncryptor();
            byte[] resultArray = objCrytpoTransform.TransformFinalBlock(toEncryptedArray, 0, toEncryptedArray.Length);
            objTripleDESCryptoService.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static string DecryptCipherTextToPlainText(string CipherText, string SecurityKey)
        {
            byte[] toEncryptArray = Convert.FromBase64String(CipherText);
            SHA256Managed objMD5CryptoService = new SHA256Managed();

            byte[] securityKeyArray = objMD5CryptoService.ComputeHash(UTF8Encoding.UTF8.GetBytes(SecurityKey));
            objMD5CryptoService.Clear();

            var objTripleDESCryptoService = new AesCryptoServiceProvider();
            objTripleDESCryptoService.Key = securityKeyArray;
            objTripleDESCryptoService.Mode = CipherMode.ECB;
            objTripleDESCryptoService.Padding = PaddingMode.PKCS7;

            var objCrytpoTransform = objTripleDESCryptoService.CreateDecryptor();
            byte[] resultArray = objCrytpoTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            objTripleDESCryptoService.Clear();

            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        private static Dictionary<string, string> _dayOfWeekDic = new Dictionary<string, string>
        {
            { "Monday", "Thứ 2"},
            { "Tuesday", "Thứ 3" },
            { "Wednesday", "Thứ 4"},
            { "Thursday", "Thứ 5"},
            { "Friday", "Thứ 6" },
            { "Saturday", "Thứ 7"},
            { "Sunday", "Chủ nhật"}
        };

        public static string GetLevel(string _dayOfWeekId)
        {
            return _dayOfWeekDic[_dayOfWeekId];
        }

        private static Dictionary<int, string> _monthInYear = new Dictionary<int, string>
        {
            { 1, "Tháng 1"},
            { 2, "Tháng 2" },
            { 3, "Tháng 3"},
            { 4, "Tháng 4"},
            { 5, "Tháng 5" },
            { 6, "Tháng 6"},
            { 7, "Tháng 7"},
            { 8, "Tháng 8"},
            { 9, "Tháng 9" },
            { 10, "Tháng 10"},
            { 11, "Tháng 11"},
            { 12, "Tháng 12"}
        };

        public static string GetMonth(int _monthInYearInt)
        {
            return _monthInYear[_monthInYearInt];
        }
    }
}