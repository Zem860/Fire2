using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fire2.Models;

namespace Fire2.Areas.Front.Controllers
{
    public class HomeController : Controller
    {
        private Model1 db = new Model1();
        // GET: Front/Home
        public ActionResult Index()
        {
            var aboutMenu = db.AboutMenus.OrderBy(m=>m.CreatedAt).ToList();
            ViewBag.AboutMenu = aboutMenu;
            return View();
        }
    }
}