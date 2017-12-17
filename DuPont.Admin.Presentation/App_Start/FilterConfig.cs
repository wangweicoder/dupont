using DuPont.Admin.Attributes;
using DuPont.Admin.Presentation.Filters;
using DuPont.Interface;
using DuPont.Utility.LogModule.Model;
using System.Web;
using System.Web.Mvc;

namespace DuPont.Admin.Presentation
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new CustomHandleErrorWithLogOnlyAttribute());
        }
    }
}