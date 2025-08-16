using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fire2.Models;
using MvcPaging;

namespace Fire2.Areas.Front.Controllers
{
    public class KnowledgeController : Controller
    {
        private Model1 db = new Model1();
        // GET: Front/Knowledge
        public ActionResult Index(int? page)
        {
            var knowledge = db.Knowledges.OrderByDescending(k=>k.CreatedAt).ToList();

            if (page.HasValue)
            {
                page--;
            }
            else
            {
                page = 0;
            }

            int pageSize = 3;


            return View(knowledge.ToPagedList(page.Value, pageSize));

            //return View(knowledge);
        }

        public ActionResult Detail(int id)
        {
            
            var knowledge = db.Knowledges.Find(id);
            if (knowledge == null)
            {
                return HttpNotFound();
            }

            return View(knowledge);
        }
    }
}