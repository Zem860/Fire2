using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Fire2.Areas.Dashboard.Filter;
using Fire2.Areas.Front.Models;
using Fire2.Models;
using Microsoft.Ajax.Utilities;
using MvcPaging;

namespace Fire2.Areas.Dashboard.Controllers
{
    [PermissionFilter]
    
    public class MembersController : Controller
    {
        private Model1 db = new Model1();

        // GET: Dashboard/Members
        public ActionResult Index(int? page)
        {
            var members = db.Members.OrderByDescending(m => m.CreatedAt).AsQueryable();
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

            // ✅ 重點：ToPagedList 產生 IPagedList 物件
            var pagedMembers = members.ToPagedList(page.Value, pageSize);
            return View(pagedMembers); // ✅ 型別正確：IPagedList<Members>
        }

        public ActionResult ServiceHistories(int id, int? page)
        {
            var sh = db.ServiceHistories.Where(s => s.MemberId == id).OrderByDescending(s => s.Id).AsQueryable();
            if (!page.HasValue)
            {
                page = 0;
            }
            else
            {
                page--;
            }
            int pageSize = 3;
            var pagedServiceHistory = sh.ToPagedList(page.Value, pageSize);

            return View(pagedServiceHistory);
        }
        public ActionResult ServiceHistoryEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceHistory servicehistory = db.ServiceHistories.Find(id);
            return View(servicehistory);
        }

        public ActionResult ServiceHistoryDelete(int? id)
        {
            var sh = db.ServiceHistories.Find(id);
            if (sh == null)
            {
                return HttpNotFound();
            }
            
            return View(sh);
        }

        [HttpPost, ActionName("ServiceHistoryDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult ServiceHistoryDeleteConfirm(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(400);

            var sh = db.ServiceHistories.Find(id);
            if (sh == null) return HttpNotFound();

            var memberId = sh.MemberId;   // 先存起來，刪掉後就拿不到了

            db.ServiceHistories.Remove(sh);
            db.SaveChanges();

            // 帶「Member 的 Id」回列表頁

            // 產生實際 URL（除錯時可看這個字串）
            var url = Url.Action("ServiceHistories", "Members",
                new { area = "Dashboard", id = memberId });

            return Redirect(url); // 或 RedirectToAction("ServiceHistories", new { area="Dashboard", id = memberId });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ServiceHistoryEdit(ServiceHistory serviceHistory)
        {
            var name = db.Members.FirstOrDefault()?.Name;

            if (!ModelState.IsValid)
            {
                ViewBag.name = name;
                return View(serviceHistory);
            }

            var entity = db.ServiceHistories.Find(serviceHistory.Id);
            if (entity == null) return HttpNotFound();

            // 只改欄位，不要 Add
            entity.JobTitle = serviceHistory.JobTitle;
            entity.ServiceUnit = serviceHistory.ServiceUnit;
            entity.StartYear = serviceHistory.StartYear;
            entity.StartMonth = serviceHistory.StartMonth;
            entity.EndYear = serviceHistory.EndYear;
            entity.EndMonth = serviceHistory.EndMonth;

            db.SaveChanges();
            return RedirectToAction("ServiceHistoryEdit", new { id = entity.Id }); // 建議帶 id
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

        public ActionResult RemoveCertificate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var member = db.Members.Find(id);

            if (!string.IsNullOrEmpty(member.InternationalCertificatePath))
            {

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

        ////POST: Dashboard/Members/Edit/5
        //// 若要免於大量指派(overposting) 攻擊，請啟用您要繫結的特定屬性，
        //// 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Members members, HttpPostedFileBase CertificateFile)
        {
            ModelState.Remove("PasswordHash"); // 不驗證
            ModelState.Remove("Salt");         // 同理（若有）
            var mms = db.Members.Find(members.Id);
            if (ModelState.IsValid)
            {
                mms.Account = members.Account;
                mms.Name = members.Name;
                mms.Gender = members.Gender;
                mms.Birthday = members.Birthday;
                mms.MembershipType = members.MembershipType;
                mms.Phone = members.Phone;
                mms.Mobile = members.Mobile;
                mms.Address = members.Address;
                mms.Email = members.Email;
                mms.IsInternationalMember = members.IsInternationalMember;
                if (CertificateFile !=null)
                {
                    var fileName = Path.GetFileName(CertificateFile.FileName);
                    var fileExtension = Path.GetExtension(CertificateFile.FileName);
                    var filePath = Server.MapPath("~/" + fileName);
                    if (!members.IsInternationalMember)
                    {

                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                        mms.InternationalCertificatePath = null; // 清除路徑
                                                                 //如果國際會員已經砍掉那證書也會砍掉
                    }
                    else
                    {

                        if (CertificateFile != null && CertificateFile.ContentLength > 0)
                        {
                            var physicalPath = Server.MapPath("~/Uploads/Certificates/" + fileName);
                            CertificateFile.SaveAs(physicalPath);
                        }
                        mms.InternationalCertificatePath = filePath; // 儲存檔案路徑
                    }
                }
                
                mms.CurrentOrgnization = members.CurrentOrgnization;
                mms.JobTitle = members.JobTitle;
                mms.TotalYears = members.TotalYears;
                mms.TotalMonths = members.TotalMonths;
                mms.UpdatedAt = DateTime.UtcNow;

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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var member = db.Members.Find(id);
            if (member == null) return HttpNotFound();

            // 先把這位會員的留言刪掉
            var comments = db.Comments.Where(c => c.MemberId == id).ToList();
            db.Comments.RemoveRange(comments);

            db.Members.Remove(member);
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
        private static string DumpModelStateErrors(ModelStateDictionary ms)
        {
            var sb = new StringBuilder();
            foreach (var kv in ms)
            {
                foreach (var err in kv.Value.Errors)
                {
                    var msg = string.IsNullOrWhiteSpace(err.ErrorMessage)
                        ? err.Exception?.Message
                        : err.ErrorMessage;
                    if (!string.IsNullOrWhiteSpace(msg))
                        sb.AppendLine($"{kv.Key}: {msg}");
                }
            }
            return sb.ToString();
        }
    }

}
