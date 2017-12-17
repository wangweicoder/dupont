using System.Web.Mvc;

namespace DuPont.Presentation.Areas.Expert
{
    public class ExpertAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Expert";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Expert_default",
                "Expert/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
