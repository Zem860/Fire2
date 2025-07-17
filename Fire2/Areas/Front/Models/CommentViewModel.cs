using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fire2.Areas.Front.Models
{
    public class CommentViewModel
    {
        public int Id { get; set; }
        public string CommentContent { get; set; }
        public DateTime CreateDate { get; set; }
        public string MemberAccount { get; set; }
    }
}