using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Fire2.Areas.Front.Models
{
    public enum Membership
    {
        正式會員 = 0,
        準會員 = 1,
        個人贊助會員 = 2,
        學生會員=3,
    }

    public enum GenderEnum
    {
        男 = 0,
        女 = 1
    }

    public class Members
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "編號")]
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(50)]
        [Display(Name = "帳號")]
        public string Account { get; set; }
        [Required(ErrorMessage = "密碼為必填項")]
        [Column(TypeName = "nvarchar")]
        [MaxLength(100)]
        [Display(Name = "密碼")]
        public string PasswordHash { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(100)]
        [Display(Name ="密碼鹽")]
        public string Salt { get; set; }

        [Required(ErrorMessage ="姓名為必填")]
        [Column(TypeName = "nvarchar")]
        [MaxLength(100)]
        [Display(Name = "姓名")]
        public string Name { get; set; }

        [Required(ErrorMessage = "性別為必填")]
        [Display(Name = "性別")]
        public Gender? Gender { get; set; }  // 用 nullable 才會觸發 Required

        [Required(ErrorMessage = "生日為必填")]
        [Display(Name = "生日")]
        [Column(TypeName = "DATETIME")]
        public DateTime Birthday { get; set; }

        [Required(ErrorMessage = "會員類型為必填")]
        [Display(Name = "會員類型")]
        public Membership MembershipType { get; set; }


        [Required(ErrorMessage = "電話為必填")]
        [RegularExpression(@"^(\d{2,4}-)?\d{6,8}$", ErrorMessage = "電話格式錯誤")]
        [Display(Name = "連絡電話(公)")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "手機為必填")]
        [RegularExpression(@"^09\d{8}$", ErrorMessage = "手機格式錯誤")]
        [Display(Name = "連絡電話(私)")]
        public string Mobile { get; set; }
        [Required(ErrorMessage = "地址為必填")]
        [Column(TypeName = "nvarchar")]
        [MaxLength(200)]
        [Display(Name = "地址")]
        public string Address { get; set; }


        [Required(ErrorMessage = "Email為必填")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Email格式錯誤")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        // 是否為國際會員
        [Display(Name = "是否為國際會員")]
        public bool IsInternationalMember { get; set; }

        // 國際會員證明上傳的檔案名稱（僅儲存檔名或路徑）
        [Display(Name = "國際會員證明")]
        public string InternationalCertificatePath { get; set; }

        [Required(ErrorMessage = "現職單位為必填")]
        [Column(TypeName = "nvarchar")]
        [MaxLength(50)]
        [Display(Name = "現職單位")]
        public string CurrentOrgnization { get; set; }
        [Required(ErrorMessage = "職稱為必填")]

        [Column(TypeName = "nvarchar")]
        [MaxLength(50)]
        [Display(Name = "職稱")]


        public string JobTitle { get; set; }

        [Required(ErrorMessage = "最高學歷為必填")]
        [Column(TypeName = "nvarchar")]
        [MaxLength(50)]
        [Display(Name = "最高學歷")]
        public string HighestEducation { get; set; }

        public virtual List<ServiceHistory> ServiceHistories { get; set; } = new List<ServiceHistory>();

        [Required]
        [Display(Name = "已驗證")]
        [Column(TypeName = "bit")]
        public bool IsVerified { get; set; } = false;


        [Required(ErrorMessage = "合計年資(年)為必填")]
        [Display(Name = "合計年資(年)")]
        [Range(0,100, ErrorMessage = "合計年資(年)請輸入0~100之間")]  
        public int? TotalYears { get; set; }
        [Required (ErrorMessage="合計年資(月)為必填")]
        [Display(Name="合計年資(月)")]
        [Range(0, 12, ErrorMessage = "合計年資(月)請輸入0~12之間")]
        public int? TotalMonths { get; set; }

        [Column(TypeName = "DATETIME")]
        [Display(Name = "創建時間")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Column(TypeName = "DATETIME")]
        [Display(Name = "更新時間")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public virtual ICollection<Posts> Posts { get; set; }
        public virtual ICollection<Comments> Comments { get; set; }


    }
}