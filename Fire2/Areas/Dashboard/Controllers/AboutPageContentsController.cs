using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Fire2.Areas.Front.Models;
using Fire2.Models;

namespace Fire2.Areas.Dashboard.Controllers
{
    public class AboutPageContentsController : Controller
    {
        private Model1 db = new Model1();

        // GET: Dashboard/AboutPageContents
        public ActionResult Index()
        {
            return View(db.AboutPageContents.ToList());
        }

        // GET: Dashboard/AboutPageContents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AboutPageContent aboutPageContent = db.AboutPageContents.Find(id);
            if (aboutPageContent == null)
            {
                return HttpNotFound();
            }
            return View(aboutPageContent);
        }

        // GET: Dashboard/AboutPageContents/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Dashboard/AboutPageContents/Create
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,PageKey,Title, Link,Content,CreatedAt, UpdatedAt")] AboutPageContent aboutPageContent)
        {
            if (ModelState.IsValid)
            {
                aboutPageContent.CreatedAt = DateTime.UtcNow; // 設定創建時間為當前時間
                aboutPageContent.UpdatedAt = DateTime.UtcNow; // 設定更新時間為當前時間
                db.AboutPageContents.Add(aboutPageContent);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(aboutPageContent);
        }

        // GET: Dashboard/AboutPageContents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AboutPageContent aboutPageContent = db.AboutPageContents.Find(id);
            if (aboutPageContent == null)
            {
                return HttpNotFound();
            }
            return View(aboutPageContent);
        }

        // POST: Dashboard/AboutPageContents/Edit/5
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,PageKey,Title,Link,Content,CreatedAt,UpdatedAt")] AboutPageContent aboutPageContent)
        {
            if (ModelState.IsValid)
            {
                aboutPageContent.UpdatedAt = DateTime.UtcNow; // 更新時間設為當前時間
                db.Entry(aboutPageContent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(aboutPageContent);
        }

        // GET: Dashboard/AboutPageContents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AboutPageContent aboutPageContent = db.AboutPageContents.Find(id);
            if (aboutPageContent == null)
            {
                return HttpNotFound();
            }
            return View(aboutPageContent);
        }

        // POST: Dashboard/AboutPageContents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AboutPageContent aboutPageContent = db.AboutPageContents.Find(id);
            db.AboutPageContents.Remove(aboutPageContent);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
