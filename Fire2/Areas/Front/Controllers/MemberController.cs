using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fire2.Areas.Front.Helper;
using Fire2.Areas.Front.Models;
using Fire2.Models;

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
                    MembershipType = Membership.正式會員
                },
                ServiceHistories = new List<ServiceHistory> { new ServiceHistory() },
                ServiceHistoriesViewModel = new List<ServiceHistoryViewModel> { new ServiceHistoryViewModel(), new ServiceHistoryViewModel() }
            };

            return View(ViewModel);
        }

        [HttpPost]
        public ActionResult SubmitRegister(MemberRegisterViewModel model, string Captcha)
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
            db.Members.Add(model.Member);
            db.SaveChanges(); // 拿到 Member.Id

            foreach (var sh in model.ServiceHistories)
            {
                sh.MemberId = model.Member.Id;
                db.ServiceHistories.Add(sh);
            }

            foreach (var svm in model.ServiceHistoriesViewModel)
            {
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

        public ActionResult Download()
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login");

            }
            else
            {
                return View();
            }
        }
    }
}