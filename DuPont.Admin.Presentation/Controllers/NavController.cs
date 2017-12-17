// ***********************************************************************
// Assembly         : DuPont
// Author           : 毛文君
// Created          : 08-06-2015
//
// Last Modified By : 毛文君
// Last Modified On : 08-06-2015
// ***********************************************************************
// <copyright file="NavController.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************



using DuPont.Interface;
using DuPont.Models.Enum;
using DuPont.Models.Models;
using DuPont.Utility;
using DuPont.Utility.LogModule.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DuPont.Admin.Presentation.Controllers
{
    public class NavController : BaseController
    {
        public PartialViewResult Menu()
        {
            using (var result = new ResponseResult<List<T_MENU>>())
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
                var reponseObj = RestSharpHelper.PostWithApplicationJson<ResponseResult<List<T_MENU>>>(GetCurrentUrl(this), postParas, GetCertificationFilePath(), GetCertificationPwd());
                var menuString = HttpAsynchronousTool.CustomHttpWebRequestPost(GetCurrentUrl(this), postParas, GetCertificationFilePath(), GetCertificationPwd());
                if (reponseObj != null && reponseObj.IsSuccess)
                {
                    return PartialView("MenuPartial", reponseObj.Entity);
                }

                return PartialView("MenuPartial");
            }
        }
    }
}