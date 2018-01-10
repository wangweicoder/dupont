using DuPont.Global.ActionResults;
using DuPont.Interface;
using DuPont.Models.Dtos.Foreground.Common;
using DuPont.Models.Models;
// ***********************************************************************
// Assembly         : DuPont
// Author           : 毛文君
// Created          : 08-05-2015
//
// Last Modified By : 毛文君
// Last Modified On : 08-05-2015
// ***********************************************************************
// <copyright file="AdminController.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DuPont.Controllers
{
    public class AdminController:Controller
    {
        private IMenu menuRepository;
        private IMenu_Role menuRoleRepository;
        
        public AdminController(IMenu menu,IMenu_Role menurole)
        {
            this.menuRepository = menu;
            this.menuRoleRepository = menurole;
        }

        //
        // GET: /Admin/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult FindWithPager(MenuInput input)
        {
            using (ResponseResult<List<T_MENU>> result = new ResponseResult<List<T_MENU>>())
            {
                IList<T_MENU> menuList = null;
                long recordCount;                
                menuList = this.menuRepository.GetMenuList(menu => menu.Visible,input.PageIndex ,input.PageSize,out recordCount);
                result.IsSuccess = true;
                result.Entity = menuList.ToList();
                result.TotalNums = recordCount;
                return new JsonResultEx(result);
            }
        }
        /// <summary>
        /// 查询子菜单列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ActionResult GetChildrenMenu(MenuInput input)
        {
            using (ResponseResult<List<T_MENU>> result = new ResponseResult<List<T_MENU>>())
            {
                IList<T_MENU> menuList = null;
                menuList = this.menuRepository.GetAll(menu => menu.Visible&& menu.ParentId==input.ParentId);
                result.IsSuccess = true;
                result.Entity = menuList.ToList();
                result.TotalNums = menuList.ToList().Count;
                return new JsonResultEx(result);
            }
        }
	}
}