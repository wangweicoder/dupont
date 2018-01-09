﻿using DuPont.Admin.Presentation.Filters;
using DuPont.Global.Filters.ActionFilters;
using DuPont.Interface;
using DuPont.Models.Dtos.Foreground.Common;
using DuPont.Models.Enum;
using DuPont.Models.Models;
using DuPont.Utility;
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


namespace DuPont.Admin.Presentation.Controllers
{

    [SysAuthAttribute]
    public class AdminController:BaseController
    {

        public AdminController()
        {           
         
        }

        //
        // GET: /Admin/
        [Validate]
        public ActionResult Index()
        {           
            return View();
        }
        /// <summary>
        /// 菜单管理
        /// </summary>
        /// <returns></returns>
        public ActionResult MenuManager()
        {
            return View();
        }
        public ActionResult FindWithPager(MenuInput input)
        {
            //获取用户的角色信息
            var userId = GetLoginInfo().User.Id;
            //请求的参数
            var postParas = new Dictionary<string, string>(){
                        {"userId",userId.ToString()}
                    };

            if (postParas.ContainsKey(DataKey.UserId) == false)
            {
                postParas.Add(DataKey.UserId, GetLoginInfo().User.Id.ToString());
            }
            var result = RestSharpHelper.PostWithApplicationJson<ResponseResult<List<T_MENU>>>(GetCurrentUrl(this), postParas, GetCertificationFilePath(), GetCertificationPwd());
            var model = new MultiModel<List<T_MENU>>(result.IsSuccess, input.PageIndex, input.PageSize, (int)result.TotalNums, result.Entity);
            return View(model);            
        }
	}
}