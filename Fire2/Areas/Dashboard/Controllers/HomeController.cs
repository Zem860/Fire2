using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Fire2.Areas.Dashboard.Helper;
using Fire2.Models;

namespace Fire2.Areas.Dashboard.Controllers
{
    [DashboardAuthorize]
    public class HomeController : Controller
    {
        // GET: Dashboard/Dashboard
        public ActionResult Index()
        {
            // 取得目前的 FormsAuthentication Ticket
            var authCookie = System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie == null)
            {
                // 沒有 cookie，導回後台登入
                return RedirectToAction("Index", "Account", new { area = "Dashboard" });
            }

            try
            {
                // 解密 cookie
                var ticket = FormsAuthentication.Decrypt(authCookie.Value);
                var userName = ticket.Name; // 我假設 Name 存的是 Admin 的 Id
                var db = new Model1();

                // 查資料庫確認 Admin 是否存在
                var admin = db.Admins.FirstOrDefault(a => a.Account.ToString() == userName);
                if (admin == null)
                {
                    // 沒這個人，清除 cookie 並導回後台登入
                    FormsAuthentication.SignOut();
                    return RedirectToAction("Index", "Account", new { area = "Dashboard" });
                }

                // ✅ 有找到，允許進入
                return View();
            }
            catch
            {
                // 解密失敗或其他錯誤，清除 cookie 並導回後台登入
                FormsAuthentication.SignOut();
                return RedirectToAction("Index", "Account", new { area = "Dashboard" });
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}