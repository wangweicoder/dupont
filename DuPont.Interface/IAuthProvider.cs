// ***********************************************************************
// Assembly         : DuPont.Interface
// Author           : 毛文君
// Created          : 08-10-2015
//
// Last Modified By : 毛文君
// Last Modified On : 08-10-2015
// ***********************************************************************
// <copyright file="IAuthProvider.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using DuPont.Models.Models;
using DuPont.Models.Dtos.Background;
namespace DuPont.Interface
{
    public interface IAuthProvider
    {
        /// <summary>
        /// 管理后台登录
        /// </summary>
        /// <param name="loginUserName"></param>
        /// <param name="loginPwd"></param>
        /// <returns></returns>
        AdminUserLoginInfo Authenticate(LoginInputDto input);
    }
}
