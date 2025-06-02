using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
    }
}