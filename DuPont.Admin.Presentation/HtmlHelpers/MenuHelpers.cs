// ***********************************************************************
// Assembly         : DuPont
// Author           : 毛文君
// Created          : 08-06-2015
//
// Last Modified By : 毛文君
// Last Modified On : 08-06-2015
// ***********************************************************************
// <copyright file="MenuHelpers.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Web.Mvc;
using DuPont.Models.Models;

namespace DuPont.Admin.Presentation.HtmlHelpers
{
    public static class MenuHelpers
    {
        public static MvcHtmlString GenerateMasterMenu(this HtmlHelper html, IEnumerable<T_MENU> menuList)
        {
            if (menuList != null)
            {
                StringBuilder sbResult = new StringBuilder();

                sbResult.AppendFormat("<ul class=\"sidebar-menu\">");
                FindSubMenu(menuList, 0, sbResult);
                sbResult.AppendFormat("</ul>");
                return MvcHtmlString.Create(sbResult.ToString());
            }

            return null;
        }

        private static IEnumerable<T_MENU> FindSubMenu(IEnumerable<T_MENU> menuList, int parentId, StringBuilder sbResult)
        {
            var child_MenuList = menuList.Where(m => m.ParentId == parentId).OrderBy(m => m.Order).ToList();
            
            if (child_MenuList != null && child_MenuList.Count() > 0)
            {
                if (parentId != 0)
                {
                    sbResult.Append("<ul class=\"treeview-menu\">");
                }
                foreach (var mnu in child_MenuList)
                {
                    //判断是否有子菜单
                    var hasChildMenu = menuList.Count(m => m.ParentId == mnu.Id) > 0;
                    var currentUrl = (string.IsNullOrEmpty(mnu.Url) ? "javascript:;" : mnu.Url);
                    var currentIcon = (!hasChildMenu && mnu.ParentId != 0) ? "fa-file" : "fa-th";
                    if (hasChildMenu)
                    {
                        sbResult.Append("<li class=\"treeview\">");
                        if (mnu.ParentId == 0)
                        {
                            sbResult.Append("<a href=\"" + currentUrl + "\" target=\"content_right\">");
                        }
                        else
                        {
                            sbResult.Append("<a href=\"" + mnu.Url + "\" target=\"content_right\">");
                        }
                        sbResult.Append("<i class=\"fa " + currentIcon + "\"></i> <span>" + mnu.MenuName + "</span> <i class=\"fa fa-angle-left pull-right\"></i>");
                        sbResult.Append("</a>");

                        FindSubMenu(menuList, mnu.Id, sbResult);

                        sbResult.Append("</li>");
                    }
                    else
                    {
                        sbResult.Append("<li>");
                        sbResult.Append("<a href=\"" + currentUrl + "\" target=\"content_right\">");
                        sbResult.Append("<i class=\"fa " + currentIcon + "\"></i>");
                        sbResult.Append("<span>" + mnu.MenuName + "</span></a>");
                        sbResult.Append("</li>");
                    }
                }

                if (parentId != 0)
                {
                    sbResult.Append("</ul>");
                }
            }

            return null;
        }
    }
}