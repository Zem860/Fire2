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
            //在沒有controller的情況下不會有Url，所以自訂一個去模擬controller
            //但實際上我要導入到AccountController
            var loginUrl = url.Action("Index", "Account", new { area = "Dashboard" });
            //繼承了AuthorizeAttribute它本身就會幫你檢查HttpContext.User.Identity.IsAuthenticated如果是false本來會return401
            //但是使用override改寫後我把它倒回了
            filterContext.Result = new RedirectResult(
                loginUrl
            );
        }
    }


}