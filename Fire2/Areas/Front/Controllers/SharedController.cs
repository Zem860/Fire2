using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Fire2.Models;

namespace Fire2.Areas.Front.Controllers
{
    public class SharedController : Controller
    {
        // GET: Front/Shared
        private Model1 db = new Model1();
        [ChildActionOnly]

        public ActionResult Navbar()
        {
            var AboutMenu = db.AboutMenus.OrderBy(m => m.CreatedAt).ToList();
            return PartialView("_NavbarPartial", AboutMenu);
        }
    }
}