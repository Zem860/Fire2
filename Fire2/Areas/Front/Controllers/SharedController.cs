using System.Linq;
using System.Web.Mvc;
using Fire2.Models; // 根據你實際 DbContext 命名
using Fire2.Areas.Front.Models; // 根據 NavbarItems 所在命名空間

namespace Fire2.Areas.Front.Controllers
{
    public class SharedController : Controller
    {
        private Model1 db = new Model1();

        [ChildActionOnly]
        public ActionResult Navbar()
        {
            // 抓出所有主選單（ParentId 為 NULL）
            var topLevelMenus = db.NavbarItems
                .Where(n => n.ParentId == null)
                .OrderBy(n => n.DisplayOrder)
                .ToList();

            // 預載每個主選單的子選單（一次查完）
            foreach (var menu in topLevelMenus)
            {

                menu.Children = db.NavbarItems
                    .Where(c => c.ParentId == menu.Id )
                    .OrderBy(c => c.DisplayOrder)
                    .ToList();
            }

            return PartialView("_NavbarPartial", topLevelMenus);
        }
    }
}
