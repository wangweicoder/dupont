// ***********************************************************************
// Assembly         : DuPont.Models
// Author           : 毛文君
// Created          : 08-16-2015
//
// Last Modified By : 毛文君
// Last Modified On : 08-16-2015
// ***********************************************************************
// <copyright file="RoleType.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Entity.Enum
{
    /// <summary>
    /// 角色类型
    /// </summary>
    public enum RoleType
    {
        [Description("超级管理员")]
        SuperAdmin = 1,
        [Description("管理员")]
        Admin = 2,
        [Description("大农户")]
        Farmer = 3,
        [Description("农机手")]
        MachineryOperator = 4,
        [Description("产业商")]
        Business = 5,
        [Description("经销商")]
        Dealer = 6,
        [Description("未知")]
        Unknown = 0
    }

    /// <summary>
    /// 系统配置
    /// </summary>
    public enum SysCfg
    {
        [Description("系统用户编号")]
        SysUserId = 10000
    }


    /// <summary>
    /// 响应状态码
    /// 负数：错误消息码
    /// 正数：正确消息码
    /// </summary>
    public enum ResponseStatusCode
    {
        [Description("请求成功")]
        Success = 200,
        [Description("未登录")]
        NotLogin = -1000,
        [Description("账号在其它设备登录,已被迫下线!")]
        LoginoutByOtherDevice = -1001,
        [Description("非法请求")]
        BadRequest = -1002,
        [Description("拒绝访问")]
        AccessDenied = -1003,
        [Description("参数错误")]
        InvalidArgument = -1004,
        [Description("应用程序繁忙,请稍后再试")]
        ApplicationError = -1005,
        [Description("预期错误")]
        ExpectError = -1006,
        [Description("数据服务器繁忙,请稍后再试")]
        DbServerError = -1007,
        [Description("表单令牌错误")]
        AccessTokenError = -1008,
        [Description("对不起,账户已被锁定!")]
        UserIsLock = -1009,
        [Description("您太长时间未登录了,请重新登录!")]
        PleaseReLogin = -10010,
        [Description("您的密码已过期,请修改您的密码!")]
        PleaseUpdatePassword = -10011,
        [Description("您的Token已过期,请重新登录!")]
        PleaseUpdateToken = -10012
    }


    /// <summary>
    /// 专家咨询之提问类型
    /// </summary>
    public enum QuestionType
    {
        [Description("小麦")]
        Wheat = 101301,
        [Description("玉米")]
        Corn = 101302
    }

    /// <summary>
    /// 用户类型
    /// </summary>
    public enum UserTypes
    {
        [Description("手机注册用户")]
        PhoneUser = 0,
        [Description("QQ用户")]
        QQUser = 1,
        [Description("微信用户")]
        WeChatUser = 2
    }

    /// <summary>
    /// 发布状态
    /// </summary>
    public enum PublishState
    {
        [Description("待响应")]
        WaitForResponse = 100501,
        [Description("待评价")]
        WaitForComment = 100502,
        [Description("已评价")]
        CommentAlready = 100503,
        [Description("已取消")]
        CancelAlready = 100504,
        [Description("系统关闭")]
        SystemClosed = 100505,
        [Description("已关闭")]
        ClosedAlready = 100506,
        [Description("未知")]
        Unknown = 0
    }

    /// <summary>
    /// 产业商需求类型
    /// </summary>
    public enum BusinessDemandType
    {
        [Description("收粮")]
        Grain = 100201,
        [Description("收青贮")]
        HarvestSilage = 100202
    }

    /// <summary>
    /// 大农户需求类型
    /// </summary>
    public enum FarmerDemandType
    {
        [Description("播种")]
        Sow = 100101,
        [Description("打药")]
        SprayInsecticide = 100102,
        [Description("追肥")]
        TopApplication = 100103,
        [Description("浇水")]
        Watering = 100104,
        [Description("收获")]
        Harvest = 100105,
        [Description("整地")]
        SoilPreparation = 100106,
        [Description("烘干")]
        Dry = 100107,
        [Description("劳务")]
        LabourServices = 100108,
        [Description("卖粮")]
        SellGrain = 100801,
        [Description("卖青贮")]
        SellSilage = 100802
    }
    /// <summary>
    /// 用户来源
    /// </summary>
    public enum SourceType
    {
        [Description("先锋帮")]
        DuPont = 0,
        [Description("E田")]
        JeRei = 1
    }
}
