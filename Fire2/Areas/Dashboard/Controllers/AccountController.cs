using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;
using Fire2.Areas.Dashboard.Helper;
using Fire2.Areas.Dashboard.Models;
using Fire2.Areas.Front.Models;
using Fire2.Models;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;

namespace Fire2.Areas.Dashboard.Controllers
{
    public class AccountController : Controller
    {
        private Model1 db = new Model1();

        // GET: Dashboard/Account
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        //public ActionResult Logout()
        //{
        //    FormsAuthentication.SignOut();
        //    return RedirectToAction("Index");
        //}
        public ActionResult Logout()
        {
            if (Request.Cookies[".DashboardAuth"] != null)
            {
                var cookie = new HttpCookie(".DashboardAuth")
                {
                    Expires = DateTime.Now.AddDays(-1)
                };
                Response.Cookies.Add(cookie);
            }

            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Account");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginVm login)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", login);
            }

            Admins admin = ValidateUser(login.Account, login.PasswordHash);
            //只要是null都是失敗
            if (admin == null)
            {
                ViewBag.Message = "登入失敗";
                return RedirectToAction("Index", "Account", new { area = "Dashboard" });
            }

            //登入成功
            //驗鄭成功就做表單驗證
            string userData = JsonConvert.SerializeObject(admin);
            FormsAuthentication.SetAuthCookie(admin.Account, false);

            return RedirectToAction("Index", "Home");
        }

        private Admins ValidateUser(string account, string password)
        {

            //確認帳號是否存在

            Admins admins = db.Admins.FirstOrDefault(a => a.Account == account);
            if (admins == null)
            {
                return null;
            }
            //確認密碼是否正確
            //資料庫資料
            string dbPassword = admins.PasswordHash;
            string salt = admins.Salt;
            //產生雜湊密碼
            var hashPassword = UtilHelper.GenerateHashWithSalt(password, salt);
            if (hashPassword != dbPassword)
            {
                return null;
            }
            return admins;
        }

    }
}