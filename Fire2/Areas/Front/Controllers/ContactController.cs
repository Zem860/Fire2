using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fire2.Areas.Front.Models;

namespace Fire2.Areas.Front.Controllers
{
    public class ContactController : Controller
    {
        // GET: Front/Contact
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SubmitContact(ContactViewModel model, string Captcha)
        {
            // 這裡可以處理表單提交的邏輯
            var correctCode = Session["CaptchaCode"] as string;
            if (string.IsNullOrEmpty(Captcha) ||!string.Equals(correctCode, Captcha))
            {
                return View("Contact");
            }
            Session.Remove("CaptchaCode"); // ✅ 驗證完清除
            TempData["Success"] = "您的意見已經送出！";
            return RedirectToAction("Contact");
        }

        public static string Txt2Html(string fstr)
        {
            return string.IsNullOrEmpty(fstr)
                ? ""
                : HttpUtility.HtmlEncode(fstr).Replace("\n", "<br>");
        }

    }
}
