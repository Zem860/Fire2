using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Fire2.Areas.Dashboard.Helper;
using Fire2.Areas.Front.Models;
using Fire2.Migrations;
using MvcPaging;
using Fire2.Models;
using Fire2.Areas.Dashboard.Models;
using Fire2.Areas.Dashboard.Filter;

namespace Fire2.Areas.Dashboard.Controllers
{
    [PermissionFilter]

    public class NewsController : Controller
    {
        private Model1 db = new Model1();
        private string pos = "News";


        // GET: Dashboard/News
        public ActionResult Index(int? page)
        {
            if (!page.HasValue)
            {
                page = 0;
            } else
            {
                page--;
            }
            int pageSize = 3;
            var news = db.News.OrderByDescending(x => x.CreatedAt).AsQueryable();

            if (Session["NewsTitle"] != null)
            {
                string title = Session["NewsTitle"].ToString();
                news = news.Where(x=>x.Title.Contains(title));
            }

            if (Session["NewsContent"] != null)
            {
                string content = Session["NewsContent"].ToString();
                news = news.Where(x => x.Content.Contains(content));
            }
            if (Session["NewsCreatedAtStart"] != null)
            {
                DateTime start = (DateTime)Session["NewsCreatedAtStart"];
                news = news.Where(x=>x.CreatedAt > start);

            }
            if (Session["NewsCreatedAtEnd"] != null)
            {
                DateTime end = (DateTime)Session["NewsCreatedAtEnd"];
                news = news.Where(x => x.CreatedAt < end);
            }

            return View(news.ToPagedList(page.Value, pageSize));

            //return View(db.News.ToList());
        }

        [HttpPost]
        public ActionResult Index(NewsSearchModel search)
        {
            Session["NewsTitle"] = search.Title;
            Session["NewsContent"] = search.Content;
            Session["NewsCreatedAtStart"] = search.CreatedAtStart;
            Session["NewsCreatedAtEnd"] = search.CreatedAtEnd;


            return RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult UploadImage(HttpPostedFileBase upload, string CKEditorFuncNum)
        {
            if (upload != null && upload.ContentLength > 0)
            {
                var fileName = Path.GetFileName(upload.FileName);
                var filePath = Server.MapPath("~/Uploads/News/" + fileName);
                upload.SaveAs(filePath);

                var imageUrl = Url.Content("~/Uploads/News/" + fileName);

                // ⬇️ 回傳 CKEditor 4 需要的格式，才能插入圖片
                string script = $"<script>window.parent.CKEDITOR.tools.callFunction({CKEditorFuncNum}, '{imageUrl}', '圖片上傳成功');</script>";
                return Content(script, "text/html");
            }

            // ⬇️ 上傳失敗時的回傳格式
            return Content("<script>alert('圖片上傳失敗');</script>", "text/html");
        }


        // GET: Dashboard/News/Details/5
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

        // GET: Dashboard/News/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(News news, HttpPostedFileBase CoverPhoto)
        {

            if (ModelState.IsValid)
            {
                if (CoverPhoto == null || CoverPhoto.ContentLength == 0)
                {
                    news.CoverPhoto = news.CoverPhoto; // 預設圖片
                }
                else
                {
                    string fileName = FileHelper.SaveUpImage(CoverPhoto, pos);
                    news.CoverPhoto = $"/Uploads/{pos}/{fileName}";
                }


                db.News.Add(news);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(news);
        }

        // POST: Dashboard/News/Create
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,Title,CoverPhoto,Content")] News news)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.News.Add(news);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(news);
        //}

        // GET: Dashboard/News/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: Dashboard/News/Edit/5
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(News news, HttpPostedFileBase CoverPhoto)
        {

            if (ModelState.IsValid)
            {
                db.Entry(news).State = EntityState.Modified;
                var newsPhotoPath = news.CoverPhoto;
                if (CoverPhoto == null || CoverPhoto.ContentLength == 0)
                {
                    news.CoverPhoto = $"/Uploads/{pos}/default.png"; // 預設圖片
                }
                else
                {
                    string fileName = FileHelper.SaveUpImage(CoverPhoto, pos);
                    news.CoverPhoto = $"/Uploads/{pos}/{fileName}";
                }
                news.UpdatedAt = DateTime.UtcNow;
                db.Entry(news).Property(x => x.CreatedAt).IsModified = false;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(news);
        }

        // GET: Dashboard/News/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Dashboard/News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            News news = db.News.Find(id);
            db.News.Remove(news);
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
