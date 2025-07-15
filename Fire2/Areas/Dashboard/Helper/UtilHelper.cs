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
        //public static void SetAuthenTicket(string userData, string userId)
        //{
        //    //宣告一個驗證票
        //    FormsAuthenticationTicket ticket =
        //        new FormsAuthenticationTicket(1, userId, DateTime.Now, DateTime.Now.AddHours(3), false, userData);
        //    //加密驗證票
        //    string encryptedTicket = FormsAuthentication.Encrypt(ticket);
        //    //建立Cookie
        //    HttpCookie authenticationcookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
        //    //將Cookie寫入回應

        //    HttpContext.Current.Response.Cookies.Add(authenticationcookie);

        //}
        public static void SetAuthenTicket(string userData, string userId, string cookieName = null)
        {
            // 如果沒有指定，用區域判斷
            if (string.IsNullOrEmpty(cookieName))
            {
                if (HttpContext.Current.Request.Url.AbsolutePath.StartsWith("/Dashboard", StringComparison.OrdinalIgnoreCase))
                {
                    cookieName = ".DashboardAuth";
                }
                else
                {
                    cookieName = FormsAuthentication.FormsCookieName; // 預設 .ASPXAUTH
                }
            }

            var ticket = new FormsAuthenticationTicket(
                1, userId, DateTime.Now, DateTime.Now.AddHours(3), false, userData);

            string encryptedTicket = FormsAuthentication.Encrypt(ticket);
            var cookie = new HttpCookie(cookieName, encryptedTicket);
            HttpContext.Current.Response.Cookies.Add(cookie);
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
            if (string.IsNullOrWhiteSpace(html)) return "[無內容]";

            // 優先抓 <p>
            var matches = Regex.Matches(html, @"<p[^>]*>(.*?)</p>", RegexOptions.IgnoreCase | RegexOptions.Singleline);

            // 沒 <p> → 抓 <li> 或 <div>
            if (matches.Count == 0)
            {
                matches = Regex.Matches(html, @"<(li|div)[^>]*>(.*?)</\1>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            }

            foreach (Match match in matches)
            {
                var rawContent = match.Groups[1].Success ? match.Groups[2].Value : match.Groups[1].Value;

                // 過濾不需要的 tag
                if (Regex.IsMatch(rawContent, @"<(img|video|script|style|iframe)[^>]*>", RegexOptions.IgnoreCase))
                    continue;

                var textOnly = Regex.Replace(rawContent, "<.*?>", "", RegexOptions.Singleline).Trim();

                // 過濾空字串與亂碼
                if (!string.IsNullOrWhiteSpace(textOnly) && textOnly.Length > 3 && !Regex.IsMatch(textOnly, @"^[\W_]+$"))
                {
                    return textOnly.Length > 50 ? textOnly.Substring(0, 50) + "..." : textOnly;
                }
            }

            // fallback：整段清掉 tag
            var fallback = Regex.Replace(html, "<.*?>", "", RegexOptions.Singleline).Trim();
            if (!string.IsNullOrWhiteSpace(fallback) && fallback.Length > 3)
            {
                return fallback.Length > 50 ? fallback.Substring(0, 50) + "..." : fallback;
            }

            return "[無內容]";
        }


    }
}