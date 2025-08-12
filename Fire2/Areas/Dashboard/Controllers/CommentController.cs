using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fire2.Areas.Dashboard.Filter;
using Fire2.Areas.Dashboard.Helper;
using Fire2.Areas.Front.Models;
using Fire2.Migrations;
using Fire2.Models;

namespace Fire2.Areas.Dashboard.Controllers
{
    [PermissionFilter]
    public class CommentController : Controller
    {

        private Model1 db = new Model1();
        // GET: Dashboard/Comment
        public ActionResult Index()
        {
            var comments = db.Comments
                .Include("Member")
                .Include("Post").OrderByDescending(c=>c.Post.CreatedAt)
                .ThenByDescending(c=>c.Post.Title).ToList();
            return View(comments);
        }

        public ActionResult Edit(int id)
        {
            var comment = db.Comments.Find(id);
            return View(comment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind(Include = "Id,CommentContent")] Comments form)
        {
            if (!ModelState.IsValid) return View(form);

            var c = db.Comments.Find(id);
            if (c == null) return HttpNotFound();

            c.CommentContent = form.CommentContent;   // 只更新這個
            c.UpdateDate = DateTime.UtcNow;                                         // 不動 c.UpdateDate / c.CreateDate  => 自然就忽略
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult UploadImage(HttpPostedFileBase upload, string CKEditorFuncNum)
        {
            if (upload != null && upload.ContentLength > 0)
            {
                var fileName = Path.GetFileName(upload.FileName);
                var filePath = Server.MapPath("~/Uploads/CommentPosts/" + fileName);
                upload.SaveAs(filePath);

                var imageUrl = Url.Content("~/Uploads/CommentPosts/" + fileName);

                // ⬇️ 回傳 CKEditor 4 需要的格式，才能插入圖片
                string script = $"<script>window.parent.CKEDITOR.tools.callFunction({CKEditorFuncNum}, '{imageUrl}', '圖片上傳成功');</script>";
                return Content(script, "text/html");
            }

            // ⬇️ 上傳失敗時的回傳格式
            return Content("<script>alert('圖片上傳失敗');</script>", "text/html");
        }
    }
}