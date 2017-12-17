
// ***********************************************************************
// Assembly         : DuPont.Interface
// Author           : 曾普
// Created          : 09-28-2015
//
// Last Modified By : 曾普
// Last Modified On : 09-28-2015
// ***********************************************************************
// <copyright file="IUser_Password_History.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuPont.Models.Models;

namespace DuPont.Interface
{
    public interface IUser_Password_History: IRepository<T_USER_PASSWORD_HISTORY>
    {
        /// <summary>
        /// 根据用户ID获得密码历史记录
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        List<T_USER_PASSWORD_HISTORY> GetPasswordHistoryByUserId(long userId);

    }
}
