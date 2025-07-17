using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Fire2.Areas.Front.Models
{
    public class Posts
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "標題")]
        public string Title { get; set; }


        [Display(Name = "會員Id")]
        public int MemberId { get; set; }

        public virtual Members Member { get; set; }

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
        public virtual ICollection<Comments> Comments { get; set; }


    }
}