using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Fire2.Areas.Dashboard.Models;
using Fire2.Models;
using Fire2.Areas.Dashboard.Helper;

namespace Fire2.Areas.Dashboard.Controllers
{
    public class AdminsController : Controller
    {
        private Model1 db = new Model1();

        // GET: Dashboard/Admins
        public ActionResult Index()
        {
            return View(db.Admins.ToList());
        }

        // GET: Dashboard/Admins/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admins admins = db.Admins.Find(id);
            if (admins == null)
            {
                return HttpNotFound();
            }
            return View(admins);
        }

        // GET: Dashboard/Admins/Create
        public ActionResult Create()
        {
            var tree = new TreeHelper();
            ViewBag.Tree = tree.GetTree();
            return View();
        }

        // POST: Dashboard/Admins/Create
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Admins admins)
        {
            if (ModelState.IsValid)
            {
                admins.Salt = UtilHelper.CreateSalt();
                admins.PasswordHash = UtilHelper.GenerateHashWithSalt(admins.PasswordHash, admins.Salt);
                admins.CreatedAt = DateTime.UtcNow;
                admins.UpdatedAt = DateTime.UtcNow;
                db.Admins.Add(admins);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(admins);
        }

        // GET: Dashboard/Admins/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admins admins = db.Admins.Find(id);
            if (admins == null)
            {
                return HttpNotFound();
            }

            var tree = new TreeHelper();
            ViewBag.Tree = tree.GetTree();
            return View(admins);
        }

        // POST: Dashboard/Admins/Edit/5
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Admins admins)
        {

            if (ModelState.IsValid)
            {
                admins.UpdatedAt = DateTime.UtcNow;
                db.Entry(admins).State = EntityState.Modified;
                db.Entry(admins).Property(x => x.CreatedAt).IsModified = false;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(admins);
        }

        // GET: Dashboard/Admins/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admins admins = db.Admins.Find(id);
            if (admins == null)
            {
                return HttpNotFound();
            }
            return View(admins);
        }

        // POST: Dashboard/Admins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Admins admins = db.Admins.Find(id);
            db.Admins.Remove(admins);
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
