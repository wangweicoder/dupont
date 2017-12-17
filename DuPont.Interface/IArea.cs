// ***********************************************************************
// Assembly         : DuPont.Interface
// Author           : 毛文君
// Created          : 08-18-2015
//
// Last Modified By : 毛文君
// Last Modified On : 08-18-2015
// ***********************************************************************
// <copyright file="IArea.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using DuPont.Models.Models;
using System.Collections.Generic;
namespace DuPont.Interface
{
    public interface IArea:IRepository<T_AREA>
    {
        List<AreaViewModel> GetAreaChilds(string parentId);

        /// <summary>
        /// 根据地区编码获取地区名称
        /// </summary>
        /// <param name="areaCodes">地区编码 ---- 多个编码用竖线分隔</param>
        /// <returns>所有地区连接到一起的字符串</returns>
        string GetAreaNamesBy(string areaCodes);

        /// <summary>
        /// 获取管理区域
        /// </summary>
        /// <param name="parentAid">为-1时将获取管理范围的所有级别的地区数据</param>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<T_AREA> GetManageArea(string parentAid, long userId);
    }
}
