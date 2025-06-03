using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fire2.Areas.Front.Models
{
    public class Experts
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [Display(Name = "編號")]
        public int Id { get; set; }

        [AllowHtml]

        [Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(50)]
        [Display(Name = "姓名")]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(300)]
        [Display(Name = "專家照片")]
        public string ImgUrl { get; set; }

        [Display(Name = "職稱")]
        public string Title { get; set; }

        [AllowHtml]
        [Column(TypeName = "nvarchar(max)")] // 或 "ntext" / "text"，但 nvarchar(max) 比較現代
        public string Education { get; set; }

        [AllowHtml]
        [Column(TypeName = "nvarchar(max)")] // 或 "ntext" / "text"，但 nvarchar(max) 比較現代
        public string Introduction { get; set; }

        [AllowHtml]
        [Column(TypeName = "nvarchar(max)")] // 或 "ntext" / "text"，但 nvarchar(max) 比較現代
        public string Others { get; set; }

        [Column(TypeName = "DATETIME")]
        [Display(Name = "創建時間")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Column(TypeName = "DATETIME")]
        [Display(Name = "更新時間")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}