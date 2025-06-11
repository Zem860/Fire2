using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Fire2.Areas.Front.Models
{
    public class ServiceHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "編號")]
        public int Id { get; set; }

        public int MemberId { get; set; } // 外鍵欄位

        [ForeignKey("MemberId")]
        [JsonIgnore]
        public virtual Members Member { get; set; } // 導覽屬性

        [Required(ErrorMessage ="服務單位為必填")]
        [Column(TypeName = "nvarchar")]
        [MaxLength(100)]
        public string ServiceUnit { get; set; }


        [Required(ErrorMessage = "職稱為必填")]
        [Column(TypeName = "nvarchar")]
        [MaxLength(50)]
        [Display(Name = "職稱")]
        public string JobTitle { get; set; }

        [Required(ErrorMessage = "年必填")]
        [Range(1900, 2100, ErrorMessage = "年必填")]
        public int? StartYear { get; set; }
        [Required(ErrorMessage = "月必填")]

        [Range(1, 12, ErrorMessage = "月必填")]
        public int? StartMonth { get; set; }
        [Required(ErrorMessage = "年必填")]

        [Range(1900, 2100, ErrorMessage = "年必填")]
        public int? EndYear { get; set; }
        [Required(ErrorMessage = "月必填")]

        [Range(1, 12, ErrorMessage = "月必填")]
        public int? EndMonth { get; set; }
    }
}