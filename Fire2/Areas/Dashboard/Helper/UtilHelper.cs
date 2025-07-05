using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;

namespace Fire2.Areas.Dashboard.Helper
{
    public class UtilHelper
    {
        public const int DefaultSaltSize = 5;
        /// <summary>
        /// 產生Salt
        /// </summary>
        /// <returns>Salt</returns>
        public static string CreateSalt()
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buffer = new byte[DefaultSaltSize];
            rng.GetBytes(buffer);
            return Convert.ToBase64String(buffer);
        }

        /// <summary>
        /// 產生密碼Hash
        /// </summary>
        /// <param name="password">輸入密碼</param>
        /// <param name="salt">密碼鹽</param>
        /// <returns></returns>
        public static string GenerateHashWithSalt(string password, string salt)
        {
            // merge password and salt together
            string sHashWithSalt = password + salt;
            // convert this merged value to a byte array
            byte[] saltedHashBytes = Encoding.UTF8.GetBytes(sHashWithSalt);
            // use hash algorithm to compute the hash
            HashAlgorithm algorithm = new SHA256Managed();
            // convert merged bytes to a hash as byte array
            byte[] hash = algorithm.ComputeHash(saltedHashBytes);
            // return the has as a base 64 encoded string
            return Convert.ToBase64String(hash);
        }
        /// <summary>
        /// 驗證使用者
        /// </summary>
        /// <param name="account">輸入帳號</param>
        /// <param name="password">輸入密碼</param>
        /// <returns></returns>

        /// <summary>
        /// 將使用者資料寫入cookie,產生AuthenTicket
        /// </summary>
        /// <param name="userData">使用者資料</param>
        /// <param name="userId">UserAccount</param>
        public static void SetAuthenTicket(string userData, string userId)
        {
            //宣告一個驗證票
            FormsAuthenticationTicket ticket =
                new FormsAuthenticationTicket(1, userId, DateTime.Now, DateTime.Now.AddHours(3), false, userData);
            //加密驗證票
            string encryptedTicket = FormsAuthentication.Encrypt(ticket);
            //建立Cookie
            HttpCookie authenticationcookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            //將Cookie寫入回應

            HttpContext.Current.Response.Cookies.Add(authenticationcookie);

        }

        /// <summary>
        /// 擷取第一個 &lt;p&gt; 標籤中的 HTML 內容（保留格式）
        /// </summary>
        public static string ExtractFirstParagraph(string html)
        {
            if (string.IsNullOrEmpty(html)) return "";
            var match = Regex.Match(html, "<p.*?>(.*?)</p>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            return match.Success ? $"<p>{match.Groups[1].Value}</p>" : "";
        }

        /// <summary>
        /// 擷取第一個 &lt;p&gt; 標籤中的純文字（去除 HTML 標籤）
        /// </summary>
        public static string ExtractFirstCleanParagraph(string html)
        {
            if (string.IsNullOrEmpty(html)) return "";

            // 抓出所有 <p>...</p>
            var matches = Regex.Matches(html, @"<p[^>]*>(.*?)</p>", RegexOptions.IgnoreCase | RegexOptions.Singleline);

            foreach (Match match in matches)
            {
                var rawContent = match.Groups[1].Value;

                // 移除內部 HTML 標籤
                var textOnly = Regex.Replace(rawContent, "<.*?>", "", RegexOptions.Singleline).Trim();

                // 過濾掉空的或只有廣告/多媒體的段落
                if (!string.IsNullOrWhiteSpace(textOnly) && textOnly.Length > 5)
                {
                    // 如果字數過長，截斷
                    if (textOnly.Length > 100)
                        textOnly = textOnly.Substring(0, 50) + "...";

                    return textOnly;
                }
            }

            // 如果完全沒找到
            return "";
        }


    }
}