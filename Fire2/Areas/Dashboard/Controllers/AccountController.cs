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
using static Fire2.Areas.Dashboard.Helper.UtilHelper;

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

        [HttpPost, ValidateAntiForgeryToken, AllowAnonymous]
        public ActionResult Login(LoginVm m)
        {
            var admin = ValidateUser(m.Account, m.PasswordHash);   // 用明碼 Password
            if (admin == null) return View("Index", m);

            var json = JsonConvert.SerializeObject(new { Id = admin.Id, Account = admin.Account });
            var ticket = new FormsAuthenticationTicket(
                1, admin.Name, DateTime.Now, DateTime.Now.AddHours(3), false, json);

            Response.Cookies.Add(new HttpCookie(".DashboardAuth", FormsAuthentication.Encrypt(ticket))
            {
                HttpOnly = true,
                Path = "/Dashboard"
            });

            return RedirectToAction("Index", "Home", new { area = "Dashboard" });
        }

        [AllowAnonymous]
        public ActionResult Logout()
        {
            Response.Cookies.Add(new HttpCookie(".DashboardAuth") { Expires = DateTime.Now.AddDays(-1), Path = "/Dashboard" });
            FormsAuthentication.SignOut(); // 清前台那顆（若有）
            return RedirectToAction("Index", "Account", new { area = "Dashboard" });
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