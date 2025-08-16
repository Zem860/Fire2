using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fire2.Areas.Front.Models
{
    public class Knowledge
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "編號")]
        public int Id { get; set; }
        [Required]
        [Column(TypeName="nvarchar")]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required]
        [Column(TypeName ="nvarchar")]

        [MaxLength(50)]
        public string SubTitle {  get; set; }


        [Column(TypeName = "nvarchar")]
        [MaxLength(300)]
        [Display(Name = "封面照片")]
        public string CoverPhoto { get; set; }

        [AllowHtml]
        [Column(TypeName = "nvarchar(max)")]
        [Display(Name = "內容")]
        public string Content { get; set; }

        [Column(TypeName = "DATETIME")]
        [Display(Name = "創建時間")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Column(TypeName = "DATETIME")]
        [Display(Name = "更新時間")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    }
}