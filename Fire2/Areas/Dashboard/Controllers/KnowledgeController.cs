using Fire2.Areas.Dashboard.Filter;
using Fire2.Areas.Dashboard.Helper;
using Fire2.Areas.Front.Models;
using Fire2.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Fire2.Areas.Dashboard.Controllers
{
    [PermissionFilter]
    public class KnowledgeController : Controller
    {


        private Model1 db = new Model1();
        // GET: Dashboard/Knowledge
        public ActionResult Index()
        {
            var knowledge = db.Knowledges.OrderByDescending(k=>k.CreatedAt).ToList();

            return View(knowledge);
        }

        public ActionResult Detail(int id)
        {
            var knowledge = db.Knowledges.FirstOrDefault(k => k.Id == id);
            return View(knowledge);
        }

        public ActionResult Edit(int? id)
        {
            var knowledge = db.Knowledges.Find(id);
            if (knowledge == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            

            return View(knowledge);
        }
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]

        [ValidateAntiForgeryToken]
        public ActionResult Create(Knowledge knowledge, HttpPostedFileBase CoverPhoto) {

            if (ModelState.IsValid)
            {
                if (CoverPhoto == null || CoverPhoto.ContentLength == 0)
                {
                    knowledge.CoverPhoto = "/Uploads/Knowledge/default.jpg"; // 預設圖片
                }
                else
                {
                    string fileName = FileHelper.SaveUpImage(CoverPhoto, "Knowledges");
                    knowledge.CoverPhoto = "https://upload.wikimedia.org/wikipedia/commons/d/d6/Nophoto.jpg";
                }
                knowledge.CreatedAt = DateTime.UtcNow;
                knowledge.UpdatedAt = DateTime.UtcNow; // 更新時間設為當前時間


                db.Knowledges.Add(knowledge);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Knowledge knowledge, HttpPostedFileBase CoverPhoto)
        {
            if (ModelState.IsValid)
            {
                var originalKnowledge = db.Knowledges.FirstOrDefault(k => k.Id == knowledge.Id);
                if (originalKnowledge == null)
                {
                    return HttpNotFound();
                }
                if (CoverPhoto != null && CoverPhoto.ContentLength>0)
                {
                    string fileName = FileHelper.SaveUpImage(CoverPhoto, "Knowledges");
                    knowledge.CoverPhoto = $"/Uploads/Knowledges/{fileName}";
                } else
                {
                    knowledge.CoverPhoto = knowledge.CoverPhoto;
                }
                originalKnowledge.Title = knowledge.Title;
                originalKnowledge.SubTitle = knowledge.SubTitle;
                originalKnowledge.CoverPhoto = knowledge.CoverPhoto;
                originalKnowledge.Content = knowledge.Content;
                originalKnowledge.UpdatedAt = DateTime.UtcNow;
                db.SaveChanges();
            }
            return View(knowledge);
        }

        [HttpPost]
        public ActionResult UploadImage(HttpPostedFileBase upload, string CKEditorFuncNum)
        {
            if (upload != null && upload.ContentLength > 0)
            {
                var fileName = Path.GetFileName(upload.FileName);
                var filePath = Server.MapPath("~/Uploads/Knowledges/" + fileName);
                upload.SaveAs(filePath);

                var imageUrl = Url.Content("~/Uploads/Knowledges/" + fileName);

                // ⬇️ 回傳 CKEditor 4 需要的格式，才能插入圖片
                string script = $"<script>window.parent.CKEDITOR.tools.callFunction({CKEditorFuncNum}, '{imageUrl}', '圖片上傳成功');</script>";
                return Content(script, "text/html");
            }

            // ⬇️ 上傳失敗時的回傳格式
            return Content("<script>alert('圖片上傳失敗');</script>", "text/html");
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Knowledge knowledge = db.Knowledges.Find(id);
            if (knowledge == null)
            {
                return HttpNotFound();
            }
            return View(knowledge);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var knowledge = db.Knowledges.Find(id);
            if (knowledge == null) return HttpNotFound();

            db.Knowledges.Remove(knowledge);

            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}