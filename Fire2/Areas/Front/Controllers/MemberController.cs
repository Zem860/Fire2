using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fire2.Areas.Front.Helper;
using Fire2.Areas.Front.Models;

namespace Fire2.Areas.Front.Controllers
{
    public class MemberController : Controller
    {
        // GET: Front/Member
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            if (Session["User"] != null)
            {
                return RedirectToAction("Download");
            }
            return View();
        }
        public ActionResult Register()
        {

            var ViewModel = new MemberRegisterViewModel();
            ViewModel.Member = new Members(); // 初始化 Members
            ViewModel.Member.Gender = Gender.男;
            ViewModel.Member.MembershipType = Membership.正式會員;
            ViewModel.ServiceHistories = new List<ServiceHistory> { new ServiceHistory(),    new ServiceHistory(),
            new ServiceHistory() }; // 初始化 ServiceHistories 為空列表


            return View(ViewModel); // ✅ 傳進 View
        }


        [HttpPost]

        public ActionResult SubmitRegister(MemberRegisterViewModel model, string Captcha)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }
                var correctCode = Session["CaptchaCodeMember"] as string;
                if (string.IsNullOrEmpty(Captcha) || !string.Equals(correctCode, Captcha))
                {
                    return View("Register");
                }
                Session.Remove("CaptchaCode"); // ✅ 驗證完清除
                TempData["Success"] = "您的意見已經送出！";
                return RedirectToAction("Register");
            
        }
   

        public ActionResult Download()
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login");

            }
            else
            {
                return View();
            }

        }
    }
}