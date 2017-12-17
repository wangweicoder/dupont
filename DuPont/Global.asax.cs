using DuPont.Global.JsonProvider;
using DuPont.Global.ValueProviderFactories;
using DuPont.Infrastructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Script.Serialization;

namespace DuPont
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //移除系统默认的JSON反序列化工具
            ValueProviderFactories.Factories.Remove(ValueProviderFactories.Factories.OfType<JsonValueProviderFactory>().FirstOrDefault());
            //添加自定义的JSON反序列化工具
            ValueProviderFactories.Factories.Add(new JsonNetValueProviderFactory());
            AutoMapperConfig.Configure();
            AreaRegistration.RegisterAllAreas();
            DependencyResolver.SetResolver(new NinjectDependencyResolver());

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
