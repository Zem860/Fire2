using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Fire2.Areas.Front.Models;
using Fire2.Models;

namespace Fire2.Areas.Front.Controllers
{

    public class DownloadController : Controller
    {
        private Model1 db = new Model1();

        public ActionResult Logout()
        {
            if (Request.Cookies[".FrontAuth"] != null)
            {
                var cookie = new HttpCookie(".FrontAuth")
                {
                    Expires = DateTime.Now.AddDays(-1)
                };
                Response.Cookies.Add(cookie);
            }

            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Member");
        }

        // GET: Front/Posts
        public ActionResult Index()
        {
            HttpCookie authCookie = this.HttpContext.Request.Cookies[".FrontAuth"];
            if (authCookie != null)
            {
                var posts = db.Posts.Include(p => p.Member).
                Include(p => p.Comments).
                OrderByDescending(p => p.CreatedAt).
                Select(p => new PostWithMetaViewModel
                {
                    PostId = p.Id,
                    Title = p.Title,
                    PostAuthor = p.Member.Account,
                    PostCreatedAt = p.CreatedAt,
                    LatestCommentAuthor = p.Comments.OrderByDescending(c => c.CreateDate).Select(c => c.Member.Account).FirstOrDefault(),
                    CommentDate = p.Comments.OrderByDescending(c => c.CreateDate).Select(c => c.CreateDate).FirstOrDefault(),
                    CommentCount = p.Comments.Count(),
                });


                return View(posts.ToList());
            }
            else
            {
                FormsAuthentication.SignOut();

                return RedirectToAction("Login", "Member", new { area = "Front" });
            }
            
        }

        // GET: Front/Posts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Posts posts = db.Posts.Include(p => p.Member).FirstOrDefault(p => p.Id == id);
            var comments = from c in db.Comments
                           join m in db.Members on c.MemberId equals m.Id
                           where c.PostId == id
                           orderby c.CreateDate descending
                           select new CommentViewModel
                           {
                               Id = c.Id,
                               CommentContent = c.CommentContent,
                               CreateDate = c.CreateDate,
                               MemberAccount = m.Account // 取得會員帳號
                           };
            if (posts == null)
            {
                return HttpNotFound();
            }
            ViewBag.Comments = comments.ToList();
            return View(posts);
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


        public ActionResult CreateRe(int? id)
        {
            Posts post = db.Posts
                .Include(p => p.Member)
                .FirstOrDefault(p => p.Id == id);
            if (post ==null)
            {
                return View(); 
            }

            var comments = new Comments();
            ViewBag.PostTitle = post.Title;
            ViewBag.PostId = post.Id; // 設定 PostId 以便在留言時使用

            return View("~/Areas/Front/Views/Download/CreateRe.cshtml", comments);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRe(int PostId, Comments comments)
        {
            if (ModelState.IsValid)
            {

                HttpCookie authCookie = this.HttpContext.Request.Cookies[".FrontAuth"];
                if (authCookie != null)
                {
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                    comments.MemberId = Convert.ToInt32(ticket.Name);
                    comments.PostId = PostId; // 設定留言的 PostId 為當前文章的 Id
                }
                else
                {
                    FormsAuthentication.SignOut();

                    return RedirectToAction("Login", "Member", new { area = "Front" });
                }

                db.Comments.Add(comments);
                db.SaveChanges();
                return RedirectToAction("Index", "Download", new { area = "Front" });
            }

            //ViewBag.MemberId = new SelectList(db.Members, "Id", "Account", posts.MemberId);
            return RedirectToAction("Index", "Download", new { area = "Front" });
        }


        // GET: Front/Posts/Create
        public ActionResult Create()
        {
            ViewBag.MemberId = new SelectList(db.Members, "Id", "Account");
            return View();
        }

        // POST: Front/Posts/Create
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Posts posts)
        {
            if (ModelState.IsValid)
            {

                HttpCookie authCookie = this.HttpContext.Request.Cookies[".FrontAuth"];
                if (authCookie != null)
                {
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                    posts.MemberId = Convert.ToInt32(ticket.Name);                  
                } else
                {
                    FormsAuthentication.SignOut();

                    return RedirectToAction("Login", "Member", new { area = "Front" }); 
                }

                db.Posts.Add(posts);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.MemberId = new SelectList(db.Members, "Id", "Account", posts.MemberId);
            return RedirectToAction("Index", "Download", new { area = "Front" });
        }


        // POST: Front/Posts/Edit/5
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,Title,MemberId,Content,CreatedAt,UpdatedAt")] Posts posts)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(posts).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.MemberId = new SelectList(db.Members, "Id", "Account", posts.MemberId);
        //    return View(posts);
        //}

        // GET: Front/Posts/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Posts posts = db.Posts.Find(id);
        //    if (posts == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(posts);
        //}

        // POST: Front/Posts/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Posts posts = db.Posts.Find(id);
        //    db.Posts.Remove(posts);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
