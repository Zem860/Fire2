using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Fire2.Areas.Front.Models
{
    public class ServiceHistoryViewModel
    {


        [Column(TypeName = "nvarchar")]
        [MaxLength(100)]
        public string ServiceUnit { get; set; }


        [Column(TypeName = "nvarchar")]
        [MaxLength(50)]
        [Display(Name = "職稱")]
        public string JobTitle { get; set; }

        [Range(1900, 2100, ErrorMessage = "年必填")]
        public int? StartYear { get; set; }

        [Range(1, 12, ErrorMessage = "月必填")]
        public int? StartMonth { get; set; }

        [Range(1900, 2100, ErrorMessage = "年必填")]
        public int? EndYear { get; set; }

        [Range(1, 12, ErrorMessage = "月必填")]
        public int? EndMonth { get; set; }
    }
}