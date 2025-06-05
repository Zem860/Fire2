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
using Fire2.Areas.Dashboard.Helper;

namespace Fire2.Areas.Dashboard.Controllers
{
    public class ExpertsController : Controller
    {
        private Model1 db = new Model1();

        // GET: Dashboard/Experts
        public ActionResult Index()
        {
            return View(db.Experts.ToList());
        }

        // GET: Dashboard/Experts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Experts experts = db.Experts.Find(id);
            if (experts == null)
            {
                return HttpNotFound();
            }
            return View(experts);
        }

        // GET: Dashboard/Experts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Dashboard/Experts/Create
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Experts experts, HttpPostedFileBase ImgUrl)
        {
            if (ImgUrl == null || ImgUrl.ContentLength == 0)
            {
                ModelState.AddModelError("ImgUrl", "請上傳專家照片");
            }

            if (ModelState.IsValid)
            {
                string fileName = FileHelper.SaveUpImage(ImgUrl);
                experts.ImgUrl = $"Uploads/Experts/{fileName}";

                db.Experts.Add(experts);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(experts);
        }

        // GET: Dashboard/Experts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Experts experts = db.Experts.Find(id);
            if (experts == null)
            {
                return HttpNotFound();
            }
            return View(experts);
        }

        // POST: Dashboard/Experts/Edit/5
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,ImgUrl,Title,Education,Introduction,Others,CreatedAt,UpdatedAt")] Experts experts)
        {
            if (ModelState.IsValid)
            {
                db.Entry(experts).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(experts);
        }

        // GET: Dashboard/Experts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Experts experts = db.Experts.Find(id);
            if (experts == null)
            {
                return HttpNotFound();
            }
            return View(experts);
        }

        // POST: Dashboard/Experts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Experts experts = db.Experts.Find(id);
            db.Experts.Remove(experts);
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
