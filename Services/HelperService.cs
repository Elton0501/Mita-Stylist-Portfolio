using DataBase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class HelperService
    {
        #region singleton
        public static HelperService Instance
        {
            get
            {
                if (instance == null) instance = new HelperService();
                return instance;
            }
        }
        private static HelperService instance { get; set; }

        public HelperService()
        {
        }
        #endregion

        public string Encrypt(string clearText)
        {
            string EncryptionKey = "abc123";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        public string Decrypt(string cipherText)
        {
            string EncryptionKey = "abc123";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
        //Elton Encodeing or Decoding for urls and return in numbers
        public string ConvertStringToHex(string input, System.Text.Encoding encoding)
        {
            Byte[] stringBytes = encoding.GetBytes(input);
            StringBuilder sbBytes = new StringBuilder(stringBytes.Length * 2);
            foreach (byte b in stringBytes)
            {
                sbBytes.AppendFormat("{0:X2}", b);
            }
            return sbBytes.ToString();
        }
        public string ConvertHexToString(String hexInput, System.Text.Encoding encoding)
        {
            int numberChars = hexInput.Length;
            byte[] bytes = new byte[numberChars / 2];
            for (int i = 0; i < numberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hexInput.Substring(i, 2), 16);
            }
            return encoding.GetString(bytes);
        }

        public DateTime getCurrentIST()
        {
            DateTime today = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
            return today;
        }

        public void RemoveOldUserVist()
        {
            using (var context = new ApplicationDbContext())
            {
                var Webdata = context.WebVisitCounts.ToList();
                var Pagedata = context.PageVisitCounts.ToList();


                if (Webdata != null && Webdata.Count > 0)
                {
                    DateTime LatestWebDate = Webdata.Max(x => x.VisitDateTime);
                    DateTime OldWebDate = LatestWebDate.AddDays(-7);
                    var oldWebData = Webdata.Where(x => x.VisitDateTime < OldWebDate).ToList();
                    context.WebVisitCounts.RemoveRange(oldWebData);
                }

                if (Pagedata != null && Pagedata.Count > 0)
                {
                    DateTime LatestImageDate = Pagedata.Max(x => x.VisitDateTime);
                    DateTime OldImageDate = LatestImageDate.AddDays(-7);
                    var oldImageData = Pagedata.Where(x => x.VisitDateTime < OldImageDate).ToList();
                    context.PageVisitCounts.RemoveRange(oldImageData);
                }
                context.SaveChanges();
            }
        }

        public string VisitPageCount(string name)
        {
            using (var context = new ApplicationDbContext())
            {

                var visitpagecount = context.PageVisitCounts.ToList();
                var data = visitpagecount.Where(z => z.VisitPage == name).ToList();
                return data != null ? data.Count().ToString() : "0";
            }
        }
    }
}
