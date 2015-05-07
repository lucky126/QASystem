using System.Web.Mvc;

namespace ChinaHCM.QA.Site.Web.Areas.Manager
{
    public class ManagerAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Manager";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Manager_default",
                "Manager/{controller}/{action}/{id}",
                new {controller="Board",  action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
