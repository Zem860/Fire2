using System.Web.Mvc;

namespace Fire2.Areas.Front
{
    public class FrontAreaRegistration : AreaRegistration
    {
        public override string AreaName => "Front";

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                name: "Front_default",
                url: "Front/{controller}/{action}/{id}",
                defaults: new { action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute(
                name: "Front_root",
                url: "",
                defaults: new { controller = "Home", action = "Index" }
            );
        }
    }
}
