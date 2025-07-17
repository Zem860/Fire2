using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Fire2.Areas.Front.Helper;
using Fire2.Areas.Front.Models;
using Fire2.Models;
using Newtonsoft.Json;
using static System.Data.Entity.Infrastructure.Design.Executor;


namespace Fire2.Areas.Front.Controllers
{
    public class MemberController : Controller
    {
        private Model1 db = new Model1(); // 假設這是你的資料庫上下文

        // GET: Front/Member
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            if (Session["User"] != null)
            {
                return RedirectToAction("Download");
            }
            return View();
        }
        public ActionResult Register()
        {
            var ViewModel = new MemberRegisterViewModel
            {
                Member = new Members
                {
                    Gender = Gender.男,
                    MembershipType = Models.Membership.正式會員,
                },
                ServiceHistories = new List<ServiceHistory> { new ServiceHistory() },
                ServiceHistoriesViewModel = new List<ServiceHistoryViewModel> { new ServiceHistoryViewModel(), new ServiceHistoryViewModel() }
            };

            return View(ViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel login)
        {
            if (!ModelState.IsValid)
            {
                return View("Download", login);
            }

            Members member = ValidateUser(login.Account, login.PasswordHash);
            //只要是null都是失敗
            if (member == null)
            {
                ViewBag.Message = "登入失敗";
                return RedirectToAction("Login", "Member", login);
            }

            //登入成功
            //驗鄭成功就做表單驗證
            var simpleMember = new
            {
                Id = member.Id,
                Name = member.Account,
            };
            string userData = JsonConvert.SerializeObject(simpleMember);
            SetAuthenTicket(userData, member.Id.ToString(), this.HttpContext);

            return RedirectToAction("Download", "Member");
        }

        private Members ValidateUser(string account, string password)
        {
            //改寫成member的另外一個加密雜湊
            //確認帳號是否存在

            Members member = db.Members.FirstOrDefault(a => a.Account == account && a.IsVerified);
            if (member == null)
            {
                return null;
            }
            //確認密碼是否正確
            //資料庫資料
            string dbPassword = member.PasswordHash;
            byte[] salt = Convert.FromBase64String(member.Salt);

            // 雜湊處理
            var userHash = new HashPasswordHelper();

            var hashedPwd = Convert.ToBase64String(userHash.HashPassword(password, salt));

            if (hashedPwd != dbPassword)
            {
                return null;
            }
            return member;
        }

        public static void SetAuthenTicket(string userData, string userId, HttpContextBase context)
        {
            // 宣告一個驗證票
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                1,                // 版本
                userId,           // 使用者名稱
                DateTime.Now,     // 建立時間
                DateTime.Now.AddHours(3), // 到期時間
                false,            // 是否持久性 (Remember Me)
                userData          // 使用者資料
            );

            // 加密驗證票
            string encryptedTicket = FormsAuthentication.Encrypt(ticket);

            // 建立前台專用 Cookie
            HttpCookie authCookie = new HttpCookie(".FrontAuth", encryptedTicket);

            // 將 Cookie 寫入回應
            context.Response.Cookies.Add(authCookie);
        }


        [HttpPost]
        public ActionResult SubmitRegister(MemberRegisterViewModel model, string Captcha, HttpPostedFileBase CertificateFile)
        {
            if (!ModelState.IsValid)
            {
                if (model.ServiceHistories == null)
                {
                    model.ServiceHistories = new List<ServiceHistory>();
                }

                if (model.ServiceHistoriesViewModel == null)
                {
                    model.ServiceHistoriesViewModel = new List<ServiceHistoryViewModel>
            {
                new ServiceHistoryViewModel(),
                new ServiceHistoryViewModel()
            };
                }

                return View("Register", model);
            }
            // 驗證碼檢查
            var correctCode = Session["CaptchaCode"] as string;
            if (string.IsNullOrEmpty(Captcha) || !string.Equals(correctCode, Captcha))
            {
                return View("Register", model); // 還是回傳 View 顯示錯誤
            }

            // 雜湊處理
            var userHash = new HashPasswordHelper();
            var salt = userHash.CreateSalt();
            var hashedPwd = userHash.HashPassword(model.Member.PasswordHash, salt);
            model.Member.PasswordHash = Convert.ToBase64String(hashedPwd);
            model.Member.Salt = Convert.ToBase64String(salt);
            model.Member.IsVerified = false;
            if (CertificateFile!=null && CertificateFile.ContentLength > 0)
            {
                var fileName = Path.GetFileName(CertificateFile.FileName);
                var filePath = Server.MapPath("~/Uploads/Certificates/" + fileName);
                CertificateFile.SaveAs(filePath);
                var fileUrl = Url.Content("~/Uploads/Certificates/" + fileName);
                model.Member.InternationalCertificatePath = fileUrl; // 儲存檔案路徑
            }
            db.Members.Add(model.Member);
            db.SaveChanges(); // 拿到 Member.Id

            foreach (var sh in model.ServiceHistories)
            {
                sh.MemberId = model.Member.Id;
                db.ServiceHistories.Add(sh);
            }

            foreach (var svm in model.ServiceHistoriesViewModel)
            {
                //檢查這裡的historyviewmodel是否完整，若完整就將資料轉成model存進資料庫
                SaveSvmIfComplete(svm, model.Member.Id);
            }

            db.SaveChanges();

            TempData["Success"] = "會員資料已送出審核!";
            Session.Remove("CaptchaCode");

            return RedirectToAction("Register");
        }
        public void SaveSvmIfComplete(ServiceHistoryViewModel svm, int modelId)
        {
            bool isComplete =
                !string.IsNullOrWhiteSpace(svm.ServiceUnit) &&
                !string.IsNullOrWhiteSpace(svm.JobTitle) &&
                svm.StartYear.HasValue &&
                svm.StartMonth.HasValue &&
                svm.EndYear.HasValue &&
                svm.EndMonth.HasValue;
            if (!isComplete)
            {
                return; // 不完整 → 不存
            }

            var entity = new ServiceHistory
            {
                ServiceUnit = svm.ServiceUnit,
                JobTitle = svm.JobTitle,
                StartYear = svm.StartYear,
                StartMonth = svm.StartMonth,
                EndYear = svm.EndYear,
                EndMonth = svm.EndMonth,
                MemberId = modelId
            };

            db.ServiceHistories.Add(entity);
        }


        [Authorize]
        public ActionResult Download()
        {
            // 取得目前的 FormsAuthentication Ticket
            var authCookie = System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie == null)
            {
                // 沒有 cookie，導回後台登入
                return RedirectToAction("Index", "Account", new { area = "Dashboard" });
            }

            try
            {
                // 解密 cookie
                var ticket = FormsAuthentication.Decrypt(authCookie.Value);
                var userId = ticket.Name; // 我假設 Name 存的是 Admin 的 Id
                var db = new Model1();

                // 查資料庫確認 Admin 是否存在
                var admin = db.Members.FirstOrDefault(m => m.Id.ToString() == userId);
                if (admin == null)
                {
                    // 沒這個人，清除 cookie 並導回後台登入
                    FormsAuthentication.SignOut();
                    return RedirectToAction("Login", "Member", new { area = "Front" });
                }

                // ✅ 有找到，允許進入
                return RedirectToAction("Index", "Download", new { area = "Front" });
            }
            catch
            {
                // 解密失敗或其他錯誤，清除 cookie 並導回後台登入
                FormsAuthentication.SignOut();
                return RedirectToAction("Index", "Account", new { area = "Dashboard" });
            }       
        }

        public ActionResult DownloadMemberFile(string fileName)
        {
            var path = Server.MapPath("~/" + fileName);
            string contentType = MimeMapping.GetMimeMapping(path);
            return File(path, contentType, fileName);
        }

        public ActionResult Edit()
        {
            var authCookie = System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie == null)
            {
                return RedirectToAction("Login", "Member", new { area = "Front" }); // 沒有 cookie，導回前台登入
            }

            var ticket = FormsAuthentication.Decrypt(authCookie.Value);
            var userId = ticket.Name; // 我假設 Name 存的是 Admin 的 Id
            var member = db.Members.Include("ServiceHistories")
                .FirstOrDefault(m => m.Id.ToString() == userId);
            if (member ==null)
            {
                //看看之後要不要多一個標籤改已刪除
                return HttpNotFound();
            }
            var serviceHistoriesCount = member.ServiceHistories.Count;
            List<ServiceHistoryViewModel> sh = new List<ServiceHistoryViewModel>() { };
            if (serviceHistoriesCount < 3)
            {
                for (int i = 0; i < (3 - serviceHistoriesCount);i++)
                {
                    sh.Add(new ServiceHistoryViewModel());
                }
            }

            var vm = new MemberRegisterViewModel
            {
                Member = member,
                ServiceHistories = member.ServiceHistories.ToList(),
                ServiceHistoriesViewModel = sh,
                Captcha="",
            };


            return View(vm);
        }

        [HttpPost]
        public ActionResult Edit(MemberRegisterViewModel model, HttpPostedFileBase CertificateFile)
        {
            var member = db.Members.Find(model.Member.Id);
            if (member == null)
            {
                return HttpNotFound();
            }
            ModelState.Remove("Captcha");

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Where(x => x.Value.Errors.Count > 0)
                       .Select(x => new { x.Key, x.Value.Errors })
                       .ToList();

                return HttpNotFound();
            }

                member.Name = model.Member.Name;
            member.Email = model.Member.Email;
            member.Phone = model.Member.Phone;
            member.Mobile = model.Member.Mobile;
            member.Account = model.Member.Account;
            member.Address = model.Member.Address;
            member.Gender = model.Member.Gender;
            member.Birthday = model.Member.Birthday;
            member.MembershipType = model.Member.MembershipType;

            member.IsInternationalMember = model.Member.IsInternationalMember;
            member.CurrentOrgnization = model.Member.CurrentOrgnization;
            member.JobTitle = model.Member.JobTitle;
            member.HighestEducation = model.Member.HighestEducation;
            member.TotalYears=model.Member.TotalYears;
            member.TotalMonths = model.Member.TotalMonths;
            member.UpdatedAt = DateTime.UtcNow;

            if (CertificateFile != null && CertificateFile.ContentLength > 0)
            {
                var fileName = Path.GetFileName(CertificateFile.FileName);
                var filePath = Server.MapPath("~/Uploads/Certificates/" + fileName);
                CertificateFile.SaveAs(filePath);
                var fileUrl = Url.Content("~/Uploads/Certificates/" + fileName);
                member.InternationalCertificatePath = fileUrl; // 儲存檔案路徑
            }


            var serviceHistories = db.ServiceHistories.Where(sh => sh.MemberId == model.Member.Id).ToList();
            
            for (int i = 0; i < serviceHistories.Count(); i ++){
                var existing = serviceHistories[i];
                var updating = model.ServiceHistories[i];
                existing.ServiceUnit = updating.ServiceUnit;
                existing.JobTitle = updating.JobTitle;
                existing.StartYear = updating.StartYear;
                existing.StartMonth = updating.StartMonth;
                existing.EndYear = updating.EndYear;
                existing.EndMonth = updating.EndMonth;

            }

            foreach (var svm in model.ServiceHistoriesViewModel)
            {
                //檢查這裡的historyviewmodel是否完整，若完整就將資料轉成model存進資料庫
                SaveSvmIfComplete(svm, model.Member.Id);
            }

            db.SaveChanges();

            return RedirectToAction("Logout", "Download");
        }
    }
}