using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fire2.Areas.Front.Models;
using MvcPaging;

namespace Fire2.Areas.Dashboard.Models
{
    public class ExpertSearchModel
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Introduction { get; set; }
        public string Education { get; set; }
        public string Others { get; set; }

        public DateTime? CreatedAtStart { get; set; }
        public DateTime? CreatedAtEnd { get; set; }

        public IPagedList <Experts> Results { get; set; }
    }
}