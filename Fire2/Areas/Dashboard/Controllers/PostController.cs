using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fire2.Areas.Dashboard.Filter;
using Fire2.Areas.Dashboard.Helper;
using Fire2.Areas.Front.Models;
using Fire2.Models;
using Microsoft.Ajax.Utilities;
using MvcPaging;

namespace Fire2.Areas.Dashboard.Controllers
{
    [DashboardAuthorize]
    [PermissionFilter]

    public class PostController : Controller
    {
        private Model1 db = new Model1();
        // GET: Dashboard/Post
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

            var post = db.Posts.Include("Member").ToList();

            var pagedMembers = post.ToPagedList(page.Value, pageSize);
            return View(pagedMembers); // ✅ 型別正確：IPagedList<Members>
        }

        public ActionResult Edit(int id)
        {
            var post = db.Posts.FirstOrDefault(p=>p.Id == id);
            return View(post);
        }
        [HttpPost]
        public ActionResult Edit(int id, Posts post)
        {
            var originPost = db.Posts.First(p=>p.Id == id);
            originPost.Title = post.Title;
            originPost.UpdatedAt = DateTime.UtcNow;
            db.SaveChanges();
            return View(post);
        }

        public ActionResult Delete(int id) {

            var post = db.Posts.FirstOrDefault(p => p.Id == id);

            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult DeleteConfirm(int id)
        {
            var post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }

            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}