using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Fire2.Areas.Front.Controllers
{
    public class CaptchaController : Controller
    {
        // GET: Front/Captcha
        public ActionResult Image()
        {
            string code = GenerateCapchaText(6);
            Session["CaptchaCode"] = code;
            Bitmap bitmap = new Bitmap(200, 50);
            Graphics graphics = Graphics.FromImage(bitmap);
            using (MemoryStream ms = new MemoryStream())
            {
                Font font = new Font("Arial", 24, FontStyle.Bold);
                Brush brush = new SolidBrush(Color.DarkGreen);
                graphics.Clear(Color.LightGray);
                graphics.DrawString(code, font, brush, 10,5);
                // 加干擾線
                Pen pen = new Pen(Color.Gray);
                Random rand = new Random();
                for (int i =0; i < 5; i++)
                {
                    graphics.DrawLine(pen, rand.Next(0, 120), rand.Next(0, 40), rand.Next(0, 120), rand.Next(0, 40));
                }

                bitmap.Save(ms, ImageFormat.Png);
                return File(ms.ToArray(), "image/png");

            }
        }

        private string GenerateCapchaText(int length)
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            char[] captchaText = new char[length];
            for (int i = 0; i < length; i++)
            {
                captchaText[i]=(chars[random.Next(chars.Length)]);

            }

            return new string(captchaText);
        }
    }
}