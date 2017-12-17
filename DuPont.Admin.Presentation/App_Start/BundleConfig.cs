// ***********************************************************************
// Assembly         : DuPont
// Author           : 毛文君
// Created          : 08-04-2015
//
// Last Modified By : 毛文君
// Last Modified On : 08-05-2015
// ***********************************************************************
// <copyright file="BundleConfig.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Web;
using System.Web.Optimization;

namespace DuPont.Admin.Presentation
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css")); 
            //----------------以上为系统默认添加，下面为扩展添加----------------------
            bundles.Add(new StyleBundle("~/Content/ueditor").Include(
                      "~/Content/Styles/plugins/ueditor/themes/default/css/umeditor.min.css"));
            bundles.Add(new ScriptBundle("~/bundles/ueditor").Include(
                      "~/Content/Styles/plugins/ueditor/third-party/jquery.min.js",
                      "~/Content/Styles/plugins/ueditor/umeditor.config.js",
                      "~/Content/Styles/plugins/ueditor/umeditor.min.js",
                      "~/Content/Styles/plugins/ueditor/lang/zh-cn/zh-cn.js"
                ));

        }
    }
}
