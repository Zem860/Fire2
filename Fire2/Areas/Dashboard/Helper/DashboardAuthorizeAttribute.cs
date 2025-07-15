using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fire2.Areas.Dashboard.Helper
{
    public class DashboardAuthorizeAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var url = new UrlHelper(filterContext.RequestContext);
            var loginUrl = url.Action("Index", "Account", new { area = "Dashboard" });
            filterContext.Result = new RedirectResult(
                loginUrl + "?ReturnUrl=" + HttpUtility.UrlEncode(filterContext.HttpContext.Request.RawUrl)
            );
        }
    }


}