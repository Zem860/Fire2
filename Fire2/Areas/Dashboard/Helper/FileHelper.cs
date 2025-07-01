using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace Fire2.Areas.Dashboard.Helper
{
    public class FileHelper
    {
        public static string SaveUpImage(HttpPostedFileBase upfile, string pos)
        {
            string extenstion = Path.GetExtension(upfile.FileName);
            string fileName = $"{DateTime.Now:yyyyMMddHHmmssfff}{extenstion}";
            string path = HttpContext.Current.Server.MapPath($"~/Uploads/{pos}/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string savedPath = Path.Combine(path, fileName);
            upfile.SaveAs(savedPath);
            return fileName;
        }
    }
}