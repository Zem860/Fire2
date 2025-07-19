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
using MvcPaging;
using Fire2.Areas.Dashboard.Models;
using System.IO;
using Fire2.Areas.Dashboard.Filter;

namespace Fire2.Areas.Dashboard.Controllers
{
    [PermissionFilter]

    public class ExpertsController : Controller
    {
        private Model1 db = new Model1();

        // GET: Dashboard/Experts
        public ActionResult Index(int? page)
        {
            if (!page.HasValue)
            {
                page = 0;
                //套件邏輯規定第一頁=0
            }
            else
            {
                page--;
            }
            int pageSize = 3;
            var experts = db.Experts.OrderByDescending(x => x.CreatedAt).AsQueryable();
            //我到目前為止都只是指令還沒執行，還沒讓資料庫執行且實體化(toList就是去資料庫實際撈資料)，也就是可以不斷加上篩選條件
            //單位名稱關鍵字查詢
            if (Session["Name"] != null)
            {
                string name = Session["Name"].ToString();//Session物件要用.ToString才會變成字串
                experts = experts.Where(x => x.Name.Contains(name));
            }
            if (Session["Title"] != null)
            {
                string title = Session["Title"].ToString();
                experts = experts.Where(x => x.Title.Contains(title));
            }
            if (Session["Education"] != null)
            {
                string education = Session["Education"].ToString();
                experts = experts.Where(x => x.Education.Contains(education));
            }

            if (Session["CreatedAtStart"] != null)
            {
                DateTime start = (DateTime)Session["CreatedAtStart"];
                experts = experts.Where(x => x.CreatedAt > start);
            }
            if (Session["CreatedAtEnd"] != null)
            {
                DateTime end = (DateTime)Session["CreatedAtEnd"];
                experts = experts.Where(x => x.CreatedAt < end);
            }

            return View(experts.ToPagedList(page.Value, pageSize));
            //return View(db.Experts.ToList());
        }

        [HttpPost]
        public ActionResult UploadImage(HttpPostedFileBase upload, string CKEditorFuncNum)
        {
            if (upload != null && upload.ContentLength > 0)
            {
                var fileName = Path.GetFileName(upload.FileName);
                var filePath = Server.MapPath("~/Uploads/Experts/" + fileName);
                upload.SaveAs(filePath);

                var imageUrl = Url.Content("~/Uploads/Experts/" + fileName);

                // ⬇️ 回傳 CKEditor 4 需要的格式，才能插入圖片
                string script = $"<script>window.parent.CKEDITOR.tools.callFunction({CKEditorFuncNum}, '{imageUrl}', '圖片上傳成功');</script>";
                return Content(script, "text/html");
            }

            // ⬇️ 上傳失敗時的回傳格式
            return Content("<script>alert('圖片上傳失敗');</script>", "text/html");
        }


        [HttpPost]
        public ActionResult Index(ExpertSearchModel search)
        {
            Session["Name"] = search.Name;
            Session["Title"] = search.Title;
            Session["Education"] = search.Education;
            Session["CreatedAtStart"] = search.CreatedAtStart;
            Session["CreatedAtEnd"] = search.CreatedAtEnd;


            return RedirectToAction("Index");
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


            if (ModelState.IsValid)
            {
                if (ImgUrl == null || ImgUrl.ContentLength == 0)
                {
                    experts.ImgUrl = "/Uploads/Experts/default.jpg"; // 預設圖片
                } else
                {
                    string fileName = FileHelper.SaveUpImage(ImgUrl, "Experts");
                    experts.ImgUrl = $"/Uploads/Experts/{fileName}";
                }
                experts.CreatedAt = DateTime.UtcNow;
                experts.UpdatedAt = DateTime.UtcNow; // 更新時間設為當前時間


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
        public ActionResult Edit( Experts experts, HttpPostedFileBase ImgUrl)
        {
            if (ModelState.IsValid)
            {
                var expertInDb = db.Experts.Find(experts.Id);
                if (expertInDb == null)
                {
                    return HttpNotFound();
                }
                if (ImgUrl!=null && ImgUrl.ContentLength > 0)
                {
                    string fileName = FileHelper.SaveUpImage(ImgUrl,"Experts");
                    experts.ImgUrl = $"/Uploads/Experts/{fileName}";
                }
                else
                {
                    experts.ImgUrl = expertInDb.ImgUrl;
                }
                expertInDb.Name = experts.Name;
                expertInDb.ImgUrl = experts.ImgUrl;
                expertInDb.Title = experts.Title;
                expertInDb.Education = experts.Education;
                expertInDb.Introduction = experts.Introduction;
                expertInDb.Others = experts.Others;
                expertInDb.UpdatedAt = DateTime.UtcNow;
                expertInDb.CreatedAt = expertInDb.CreatedAt; // 保持創建時間不變

                //db.Entry(experts).State = EntityState.Modified;
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
