using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Fire2.Areas.Front.Helper.NavbarHelper;

namespace Fire2.Areas.Front.Models
{
    public class NavbarItems
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "編號")]
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(30)]
        [Display(Name = "導覽列項目")]
        public string NavbarItem { get; set; }

        [Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(70)]
        [Display(Name = "導覽列項目連結")]
        public string Link { get; set; }

        [Display(Name ="上層選單")]
        public int? ParentId { get; set; }

        [ForeignKey("ParentId")]
        public virtual NavbarItems Parent {  get; set; }
        public virtual ICollection<NavbarItems> Children { get; set; }

        [Required]
        [Display(Name = "顯示順序")]
        public int DisplayOrder { get; set; } = 0;

        [Display(Name = "顯示區域")]
        public NavbarPosition Position { get; set; } = NavbarPosition.Top;

        [Required]
        [Display(Name = "是否根據登入顯示")]
        public bool ShowForLoggedInUsers { get; set; }

        [Column(TypeName = "DATETIME")]

        [Display(Name = "創建時間")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "DATETIME")]

        [Display(Name = "更新時間")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}