using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Fire2.Areas.Dashboard.Helper
{
    public class DashboardAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase context)
        {
            // 從這次請求的 Cookie 拿後台登入那顆
            var dashboardCookie = context.Request.Cookies[".DashboardAuth"];
            if (dashboardCookie == null) return false;      // 沒拿到 → 視為未登入

            var ticket = FormsAuthentication.Decrypt(dashboardCookie.Value);
            return ticket != null && !ticket.Expired;       // 票存在且未過期 → 通過
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext fc)
        {
            var url = new UrlHelper(fc.RequestContext)
                .Action("Index", "Account", new { area = "Dashboard" });
            fc.Result = new RedirectResult(url);
        }
    }
}
