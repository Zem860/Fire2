using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fire2.Models;

namespace Fire2.Areas.Front.Controllers
{
    public class ArticleController : Controller
    {
        // GET: Front/About
        private Model1 db = new Model1();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            var sidebarData = db.AboutMenus.OrderBy(m => m.CreatedAt).ToList();
            ViewBag.SidebarData = sidebarData;
            return View();
        }

        public ActionResult Organization()
        {
            var sidebarData = db.AboutMenus.OrderBy(m => m.CreatedAt).ToList();
            ViewBag.SidebarData = sidebarData;
            return View();
        }
        public ActionResult Expert()
        {
            var expertsData = db.Experts.OrderBy(e => e.Id).ToList();
            ViewBag.ExpertsData = expertsData;
            return View();
        }
        public ActionResult ExpertDetail(int id)
        {
            // 不需要 Convert.ToString(id)，因為 int 不能為 null，也不會是空字串
            var expert = db.Experts.FirstOrDefault(e => e.Id == id);

            if (expert == null)
            {
                return HttpNotFound();
            }

            return View(expert);
        }

    }
}