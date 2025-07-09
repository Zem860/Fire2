using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Fire2.Areas.Front.Models;

namespace Fire2.Areas.Dashboard.Models
{
    public enum GenderEnum
    {
        男 = 0,
        女 = 1
    }
    public class Admins
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
        [Display(Name = "密碼鹽")]
        public string Salt { get; set; }

        [Required(ErrorMessage = "姓名為必填")]
        [Column(TypeName = "nvarchar")]
        [MaxLength(100)]
        [Display(Name = "姓名")]
        public string Name { get; set; }

        [Required(ErrorMessage = "性別為必填")]
        [Display(Name = "性別")]
        public Gender? Gender { get; set; }  // 用 nullable 才會觸發 Required

        [Required(ErrorMessage = "Email為必填")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Email格式錯誤")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Column(TypeName = "DATETIME")]
        [Display(Name = "創建時間")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Column(TypeName = "DATETIME")]
        [Display(Name = "更新時間")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [MaxLength(100)]
        [Display(Name = "權限")]
        public string Permission { get; set; }
    }
}