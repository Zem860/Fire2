using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Fire2.Areas.Front.Models
{
    public enum Gender
    {
        男 = 1,
        女 = 2
    }
    public class ContactViewModel
    {
        [Required(ErrorMessage ="姓名必填")]
        public string Name { get; set; }

        [Required (ErrorMessage = "性別必填")]
        [Display(Name="性別")]
        public Gender? Gender { get; set; }  // 用 nullable 才會觸發 Required
        [Required(ErrorMessage = "電話必填")]
        [Display(Name="連絡電話")]
        [RegularExpression(@"^(\+?\d{1,3}[- ]?)?\d{9,10}$", ErrorMessage = "電話格式錯誤")]
        public string Phone {  get; set; }

        [Required(ErrorMessage = "Email必填")]
        [EmailAddress(ErrorMessage = "Email 格式錯誤")]
        [Display(Name = "E-mail")]
        public string EmailAddress { get; set; }
        [Required(ErrorMessage = "驗證碼必填")]
        [Display(Name = "驗證碼")]
        public string Captcha { get; set; }

    }
}