using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fire2.Areas.Front.Models
{
    public class AboutPageContent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "關於頁內容ID")]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "頁面鍵")]
        public string PageKey { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "頁面標題")]

        public string Title { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(30)]
        [Display(Name = "連結")]
        public string Link { get; set; }

        [AllowHtml]
        [Display(Name = "頁面內容")]
        public string Content { get; set; }

        [Column(TypeName = "DATETIME")]
        [Display(Name = "創建時間")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Column(TypeName = "DATETIME")]
        [Display(Name = "更新時間")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}