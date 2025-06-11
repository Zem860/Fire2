using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Fire2.Areas.Front.Models
{
    public class MemberRegisterViewModel
    {

        public enum Gender
        {
            男 = 1,
            女 = 2
        }

        public enum Membership
        {
            正式會員 = 0,
            準會員 = 1,
            個人贊助會員 = 2,
            學生會員 = 3,
        }


        [Required]
        public Members Member { get; set; }

        [Required]
        public List<ServiceHistory> ServiceHistories { get; set; } = new List<ServiceHistory>();

        // 若你有需要上傳檔案的話，也可以放這個欄位
        public HttpPostedFileBase CertificateFile { get; set; }
        [Display(Name = "相關年資合計(年)")]
        [Required(ErrorMessage = "最高學歷為必填")]
        public int TotalYears { get; set; }

        [Display(Name = "相關年資合計(月)")]
        [Range(1, 12, ErrorMessage = "請輸入 1~12 的月份")]
        public int TotalMonths { get; set; }

        [Required(ErrorMessage = "驗證碼必填")]
        [Display(Name = "驗證碼")]
        public string Captcha { get; set; }
    }
}