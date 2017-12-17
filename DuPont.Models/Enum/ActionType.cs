// ***********************************************************************
// Assembly         : DuPont.Models
// Author           : 毛文君
// Created          : 12-15-2015
// Tel              :15801270290
// QQ               :731314565
//
// Last Modified By : 毛文君
// Last Modified On : 12-15-2015
// ***********************************************************************
// <copyright file="ActionType.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.ComponentModel;
namespace DuPont.Models.Enum
{
    public enum ActionType
    {
        [Description("添加")]
        Add,
        [Description("修改")]
        Edit,
        [Description("详情")]
        Detail,
        [Description("删除")]
        Delete,
        [Description("列表")]
        List
    }
}
