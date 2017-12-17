// ***********************************************************************
// Assembly         : DuPont.Models
// Author           : 毛文君
// Created          : 09-02-2015
//
// Last Modified By : 毛文君
// Last Modified On : 09-02-2015
// ***********************************************************************
// <copyright file="DataKey.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using DuPont.Entity.Enum;
using System;
namespace DuPont.Models.Enum
{
    public static class DataKey
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public const string UserId = "userId";

        /// <summary>
        /// 本地证书路径
        /// </summary>
        public const string CertificateUrl = "CertificateUrl";

        /// <summary>
        /// 证书密码
        /// </summary>
        public const string CertificatePwd = "CertificatePwd";

        /// <summary>
        /// APP接口服务的地址--Release模式使用
        /// </summary>
        public const string RemoteApiForRelease = "RemoteApi";

        /// <summary>
        /// APP接口服务的地址--Debug模式使用
        /// </summary>
        public const string RemoteApiForDebug = "RemoteApiForDebug";

        /// <summary>
        /// 远程后台接口地址
        /// </summary>
        public const string RemoteBackgroundApi = "RemoteAdminApi";

        /// <summary>
        /// APP端接口外网地址
        /// </summary>
        public const string AppApiNetAddr = "app_api_addr";

        /// <summary>
        /// 评价增加先锋币数配置编号
        /// </summary>
        public const string BonusDPointByCommentSettingID = "002";

        /// <summary>
        /// 用户信息
        /// </summary>
        public const string UserInfo = "UserInfo";

        /// <summary>
        /// 登录信息
        /// </summary>
        public const string LoginInfo = "LoginInfo";

        public const string RemoveSessionCookie = "RemoveSessionCookie";

        /// <summary>
        /// 极光推送应用 AppKey
        /// </summary>
        public const string JPushAppKey = "app_key";

        /// <summary>
        /// 极光推送应用 Master Secret
        /// </summary>
        public const string JPushMasterSecret = "masterSecret";

        /// <summary>
        /// 角色类型
        /// </summary>
        public const string RoleType = "RoleType";

        public const string ArticleStaticPageBasePath = "articleBasePath";

        /// <summary>
        /// Android轮询推送的频率
        /// </summary>、
        [Obsolete("已弃用")]
        public const string AndroidPollingFrequency = "pollingFrequency";

        /// <summary>
        /// APP有效登录保持时间（单位：天，0无限制）
        /// </summary>
        public const string SaveValidLoginDays = "SaveValidLoginDays";

        /// <summary>
        /// APP用户密码有效时间
        /// </summary>
        public const string SaveValidUserPasswordDays = "SaveValidUserPassword";

        /// <summary>
        /// 第三方登录图标显示状态配置编码
        /// </summary>
        public const string CAS_DisplayState_ID = "004";

        /// <summary>
        /// 推送轮询频率
        /// </summary>
        public const string NotificationPollingFrequency = "005";
        /// <summary>
        ///天气ApiURL
        /// </summary>
        /// <author>ww</author>
        public const string WeatherUrl = "WeatherUrl";
        /// <summary>
        ///百度地图开放平台根据经纬度查询城市名称地址
        /// </summary>
        /// <author>ww</author>
        public const string BaiduGeocoderUrl = "BaiduGeocoderUrl";
        /// <summary>
        ///百度地图开放平台AK
        /// </summary>
        /// <author>ww</author>
        public const string baiduAK = "baiduAK";
        /// <summary>
        /// 心知天气url地址
        /// </summary>
        public const string WeatherUrlNew = "WeatherUrlNew";
        /// <summary>
        /// 心知 5天天气 key
        /// </summary>
        public const string WeatherUrlKey = "WeatherUrlKey";
        /// <summary>
        /// 心知实时天气 key
        /// </summary>
        public const string WeatherUrlKey1 = "WeatherUrlKey1";
        /// <summary>
        /// Token有效登录保持时间（单位：分钟，0无限制）
        /// </summary>
        /// <author>ww</author>
        public const string ValidLoginToken = "ValidLoginToken";

    }
}
