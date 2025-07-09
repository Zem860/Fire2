using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Fire2.Areas.Front.Helper
{
    public class UtilHelper
    {
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