using DuPont.Presentation.Attributes;
using System.Web;
using System.Web.Mvc;

namespace DuPont.Presentation
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new CustomHandleErrorWithErrorJsonAttribute());
        }
    }
}