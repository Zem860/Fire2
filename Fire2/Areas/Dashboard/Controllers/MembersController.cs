using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Fire2.Areas.Dashboard.Filter;
using Fire2.Areas.Front.Models;
using Fire2.Models;

namespace Fire2.Areas.Dashboard.Controllers
{
    [PermissionFilter]

    public class MembersController : Controller
    {
        private Model1 db = new Model1();

        // GET: Dashboard/Members
        public ActionResult Index()
        {
            return View(db.Members.ToList());
        }

        // GET: Dashboard/Members/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Members members = db.Members.Find(id);
            if (members == null)
            {
                return HttpNotFound();
            }
            return View(members);
        }

        // GET: Dashboard/Members/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Dashboard/Members/Create
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Account,PasswordHash,Salt,Name,Gender,Birthday,MembershipType,Phone,Mobile,Address,Email,IsInternationalMember,InternationalCertificatePath,CurrentOrgnization,JobTitle,HighestEducation,IsVerified,TotalYears,TotalMonths,CreatedAt,UpdatedAt")] Members members)
        {
            if (ModelState.IsValid)
            {
                db.Members.Add(members);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(members);
        }

        public ActionResult RemoveCertificate(int?id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var member = db.Members.Find(id);



            if (!string.IsNullOrEmpty(member.InternationalCertificatePath)){ 
            
                string physicalPath = Server.MapPath("~/" + member.InternationalCertificatePath);
                if (System.IO.File.Exists(physicalPath))
                {
                    System.IO.File.Delete(physicalPath);
                }
                member.InternationalCertificatePath = null; // 清除路徑
            };
            //我在上傳檔案的時候應該要先亂碼並且儲存亂碼名稱
            db.SaveChanges();

            return RedirectToAction("Edit", new { id = id, area = "Dashboard" });
        }

        // GET: Dashboard/Members/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Members members = db.Members.Find(id);
            if (members == null)
            {
                return HttpNotFound();
            }
            return View(members);
        }

        // POST: Dashboard/Members/Edit/5
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Account,PasswordHash,Salt,Name,Gender,Birthday,MembershipType,Phone,Mobile,Address,Email,IsInternationalMember,InternationalCertificatePath,CurrentOrgnization,JobTitle,HighestEducation,IsVerified,TotalYears,TotalMonths,CreatedAt,UpdatedAt")] Members members)
        {
            if (ModelState.IsValid)
            {
                db.Entry(members).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(members);
        }

        // GET: Dashboard/Members/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Members members = db.Members.Find(id);
            if (members == null)
            {
                return HttpNotFound();
            }
            return View(members);
        }

        // POST: Dashboard/Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Members members = db.Members.Find(id);
            db.Members.Remove(members);
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
