using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fire2.Areas.Front.Models;
using Fire2.Areas.Front.Helper;
using Fire2.Models;
using System.Net;
using MvcPaging;


namespace Fire2.Areas.Front.Controllers
{
    public class NewsController : Controller
    {
        private Model1 db = new Model1();

        // GET: Front/News
        public ActionResult Index(int? page)
        {
            if (!page.HasValue)
            {
                page = 0;
            }
            else
            {
                page--;
            }
            int pageSize = 3;

            var newsData = db.News.OrderByDescending(e => e.CreatedAt).AsQueryable();
            ViewBag.NewsData = newsData;
            return View(newsData.ToPagedList(page.Value, pageSize));
        }

        public ActionResult News()
        {
            var newsData = db.News.OrderBy(e => e.Id).ToList();
            ViewBag.NewsData = newsData;
            return View();
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            News news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }


            return View(news);
        }
    }

}
