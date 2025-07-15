using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
namespace Fire2.Areas.Front.Models
{
    public class News
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "編號")]
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(100)]
        [Display(Name = "標題")]
        
        public string Title { get; set; }
        [Column(TypeName = "nvarchar")]
        [MaxLength(300)]
        [Display(Name = "封面照片")]
        public string CoverPhoto { get; set; }

        [AllowHtml]
        [Column(TypeName = "nvarchar(max)")]
        [Display(Name = "內容")]
        public string Content { get; set; }

        [Column(TypeName = "DATETIME")]
        [Display(Name = "建立時間")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [Column(TypeName = "DATETIME")]
        [Display(Name = "更新時間")]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

    }
}