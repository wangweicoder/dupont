// ***********************************************************************
// Assembly         : DuPont.Models
// Author           : 毛文君
// Created          : 09-08-2015
//
// Last Modified By : 毛文君
// Last Modified On : 09-08-2015
// ***********************************************************************
// <copyright file="VersionUpgradeModel.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DuPont.Models.Models
{
    /// <summary>
    /// 检查APP版本升级的模型
    /// </summary>
    public class VersionUpgradeModel
    {
        public VersionUpgradeModel()
        {
                Platform =
                Version =
                ChangeLog =
                DownloadUrl = "";
        }
        //平台
        public string Platform { get; set; }
        //当前版本
        public string Version { get; set; }
        //版本号
        public int VersionCode { get; set; }
        //更改日志
        public string ChangeLog { get; set; }
        //下载地址
        public string DownloadUrl { get; set; }
        //升级开关
        public int IsOpen { get; set; }

    }
}
