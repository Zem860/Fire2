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
        public static string ExtractFirstParagraphText(string html)
        {
            if (string.IsNullOrEmpty(html)) return "";
            var match = Regex.Match(html, "<p.*?>(.*?)</p>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            var content = match.Success ? match.Groups[1].Value : "";
            if (content.Length > 100)
            {
                content = content.Substring(0, 100) + "..."; // 取前 100 字並加省略號
            }
            return Regex.Replace(content, "<.*?>", ""); // 移除 HTML 標籤
        }
    }
}