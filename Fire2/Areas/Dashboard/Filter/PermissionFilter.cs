using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;
using Fire2.Areas.Dashboard.Models;
using Fire2.Models;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
namespace Fire2.Areas.Dashboard.Filter
{
    public class PermissionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Model1 db = new Model1();
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                filterContext.Controller.ViewBag.Side = "";
                return;
            }
            string userData = ((FormsIdentity)(HttpContext.Current.User.Identity)).Name;
            var userDataObj = JsonConvert.DeserializeObject<Admins>(userData);
            Admins admin = db.Admins.Find(Convert.ToInt32(userDataObj.Id));
            if (admin == null)
            {
                FormsAuthentication.SignOut();
                return;
            }
            //取得使用者的權限
            string userPermission = admin.Permission;
            var permissions = db.Permissions.Where(a => userPermission.Contains(a.Code)).ToList();
            string controllerName = filterContext.RouteData.Values["controller"].ToString();
            if (controllerName != "Home")
            {
                if (!permissions.Any(x => x.ControllerName == controllerName))
                {
                    FormsAuthentication.SignOut();
                    return;
                }
            }




            StringBuilder sb = new StringBuilder();
            var firstPs = permissions.Where(x => x.ParentId == null).ToList();
            foreach (var p in firstPs)
            {
                if (p.ChildPermissions.Count > 0)
                {
                    sb.Append("<li class=\"nav-item\">");
                    sb.Append($"<a href=\"#collapse-{p.Id}\" data-bs-toggle=\"collapse\" role=\"button\" class=\"nav-link\">{p.Subject} ▼</a>");
                    sb.Append($"<ul class=\"collapse list-unstyled ps-3\" id=\"collapse-{p.Id}\">");
                    sb.Append(GetSub(p.ChildPermissions, userPermission));           
                    sb.Append("</ul>");
                    sb.Append("</li>");


                }
                else
                {
                    sb.Append("<li class=\"nav-item\">");
                    sb.Append($"<a href=\"{p.Url}\" class=\"nav-link\">{p.Subject}</a>");
                    sb.Append("</li>");
                }
            }
            filterContext.Controller.ViewBag.Side = sb.ToString();

        }
        public string GetSub(ICollection<Permission> collection, string userPermission)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var child in collection)
            {

                if (child.ChildPermissions.Count > 0)
                {
                    sb.Append("<li class=\"nav-item\">");

                    //子單位權限與使用者是否相符
                    if (userPermission.Contains(child.Code))
                    {
                        sb.Append($"<a href=\"#collapse-{child.Id}\" data-bs-toggle=\"collapse\" role=\"button\" class=\"nav-link\">{child.Subject} ▼</a>");
                        sb.Append($"<ul class=\"collapse list-unstyled ps-3\" id=\"collapse-{child.Id}\">");
                        sb.Append(GetSub(child.ChildPermissions, userPermission));
                        sb.Append("</ul>");
                    }
                }
                else
                {
                    sb.Append($"<a href=\"{child.Url}\" class=\"nav-link\">{child.Subject}</a>");
                }

                sb.Append("</li>");

            }
            return sb.ToString();
        }

    }
}