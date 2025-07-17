using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fire2.Areas.Front.Models
{
    public class PostWithMetaViewModel
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string PostAuthor { get; set; }
        public DateTime PostCreatedAt { get; set; }
        public string LatestCommentAuthor { get; set; }
        public DateTime CommentDate { get; set; }
        public int CommentCount { get; set; }
    }


}