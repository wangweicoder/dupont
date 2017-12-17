using DuPont.API.Attributes;
using System.Web;
using System.Web.Mvc;

namespace DuPont.API
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new CustomHandleErrorWithErrorJsonAttribute());
        }
    }
}
