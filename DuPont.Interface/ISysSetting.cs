// ***********************************************************************
// Assembly         : DuPont.Interface
// Author           : 毛文君
// Created          : 09-10-2015
//
// Last Modified By : 毛文君
// Last Modified On : 09-10-2015
// ***********************************************************************
// <copyright file="ISysSetting.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using DuPont.Models.Models;
namespace DuPont.Interface
{
    public interface ISysSetting : IRepositoryBase<T_SYS_SETTING>
    {

        /// <summary>
        /// 获取系统配置信息
        /// </summary>
        /// <param name="settingId">Setting ID</param>
        /// <returns>T_SYS_SETTING.</returns>
        T_SYS_SETTING GetSetting(string settingId);
    }
}
