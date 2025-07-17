using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Fire2.Areas.Front.Models
{
    public class Comments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name ="編號")]
        public int Id { get; set; }

        [Display(Name = "會員Id")]
        public int MemberId { get; set; }
        public virtual Members Member { get; set; }

        [Display(Name = "Po文Id")]
        public int PostId { get; set; }
        public virtual Posts Post { get; set; }
        [Required]
        [AllowHtml]
        [Display(Name = "留言內容")]
        public string CommentContent { get; set; }

        [Display(Name = "建立時間")]
        public DateTime CreateDate { get; set; } = DateTime.Now;

        [Display(Name = "更新時間")]
        public DateTime? UpdateDate { get; set; }

    }
}