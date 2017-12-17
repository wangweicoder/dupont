// ***********************************************************************
// Assembly         : DuPont.Utility
// Author           : 毛文君
// Created          : 11-18-2015
//
// Last Modified By : 毛文君
// Last Modified On : 11-18-2015
// ***********************************************************************
// <copyright file="JPushHelper.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary>极光推送Helper类</summary>
// ***********************************************************************
using cn.jpush.api;
using cn.jpush.api.common;
using cn.jpush.api.common.resp;
using cn.jpush.api.push.mode;
using cn.jpush.api.push.notification;
using DuPont.Models.Enum;
using DuPont.Utility.LogModule.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace DuPont.Utility
{
    public static class JPushHelper
    {
        private static object monitorlock = new object();
        private static JPushClient _pushClient;

        private static JPushClient PushClient
        {
            get
            {
                Initial();
                return _pushClient;
            }
        }

        /// <summary>
        /// JPushClient初始化
        /// </summary>
        private static void Initial()
        {
            if (_pushClient != null)
                return;

            try
            {


                Monitor.Enter(monitorlock);
                if (_pushClient == null)
                {
                    var app_key = ConfigHelper.GetAppSetting(DataKey.JPushAppKey);
                    var masterSecret = ConfigHelper.GetAppSetting(DataKey.JPushMasterSecret);
                    _pushClient = new JPushClient(app_key, masterSecret);
                }
            }
            finally
            {
                Monitor.Exit(monitorlock);
            }

        }

        private static bool SendPush(PushPayload pushPayLoad)
        {
            try
            {
                var messageResult = PushClient.SendPush(pushPayLoad);
                return true;
            }
            catch (APIRequestException e)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Error response from JPush server. Should review and fix it. ");
                sb.AppendLine("HTTP Status: " + e.Status);
                sb.AppendLine("Error Code: " + e.ErrorCode);
                sb.AppendLine("Error Message: " + e.ErrorMessage);

                //记录日志到系统文件
                var log = new DP_Log
                {
                    Message = sb.ToString(),
                    StackTrace = e.StackTrace,
                    CreateTime = Utility.TimeHelper.GetChinaLocalTime()
                };

                IOHelper.WriteLogToFile(log.ToString(), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs"));
                return false;
            }
            catch (APIConnectionException e)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Error Message: " + e.Message);

                //记录日志到系统文件
                var log = new DP_Log
                {
                    Message = sb.ToString(),
                    StackTrace = e.StackTrace,
                    CreateTime = Utility.TimeHelper.GetChinaLocalTime()
                };

                IOHelper.WriteLogToFile(log.ToString(), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs"));
                return false;
            }
        }


        public static bool PushObject_All_All_Alert(string alert)
        {
            PushPayload pushPayload = new PushPayload();
            pushPayload.platform = Platform.all();
            pushPayload.audience = Audience.all();
            pushPayload.notification = new Notification().setAlert(alert);

            return SendPush(pushPayload);
        }

        /// <summary>
        /// 推送消息给特定别名的用户
        /// </summary>
        /// <returns></returns>
        public static bool PushObject_all_alias_alert(string alias, string alert)
        {
            PushPayload pushPayload = new PushPayload();
            pushPayload.platform = Platform.all();
            pushPayload.audience = Audience.s_alias(alias);//这里推送给别名为alias1的用户
            pushPayload.notification = new Notification().setAlert(alert);
            return SendPush(pushPayload);
        }

        /// <summary>
        /// 推送消息给拥有特定“标签”的用户群
        /// </summary>
        /// <returns></returns>
        public static bool PushObject_Android_Tag_AlertWithTitle(string alert, string title, params string[] tags)
        {
            PushPayload pushPayload = new PushPayload();

            pushPayload.platform = Platform.android();
            pushPayload.audience = Audience.s_tag(tags);
            pushPayload.notification = Notification.android(alert, title);

            return SendPush(pushPayload);
        }

        /// <summary>
        /// 将消息推送给安卓和IOS，并指定拥有特定标签的人
        /// </summary>
        /// <returns></returns>
        public static bool PushObject_android_and_ios(string alert, string androidTitle, Dictionary<string, string> extraParameters, params string[] tags)
        {
            PushPayload pushPayload = new PushPayload();
            pushPayload.platform = Platform.android_ios();//发送目标：android和ios
            var audience = Audience.s_tag(tags);
            pushPayload.audience = audience;//设置要发送的目标
            var notification = new Notification().setAlert(alert);//设置要发送的内容

            //针对安卓设置
            notification.AndroidNotification = new AndroidNotification().setTitle(androidTitle);//设置针对安卓的通知栏标题

            //针对ios设置
            notification.IosNotification = new IosNotification();
            notification.IosNotification.incrBadge(1);//角标加1

            //添加额外的数据
            if (extraParameters != null && extraParameters.Count > 0)
            {
                foreach (var dicItem in extraParameters)
                {
                    notification.IosNotification.AddExtra(dicItem.Key, dicItem.Value);
                }
            }

            //检查参数是否有设置正确
            pushPayload.notification = notification.Check();

            return SendPush(pushPayload);
        }


        //将指定的消息以及额外添加的数据推送给安卓和IOS平台终端且满足具有指定标签的目标用户
        public static bool PushObject_ios_tagAnd_alertWithExtrasAndMessage(string alert, string message, Dictionary<string, string> extraParameters, params string[] tags)
        {
            PushPayload pushPayload = new PushPayload();

            pushPayload.platform = Platform.android_ios();//推送的平台为android和ios
            pushPayload.audience = Audience.s_tag_and(tags);//推送的具有tag1和tag2这两个标签的目标用户

            //构造Notification对象
            var notification = new Notification();
            notification.setAlert(alert);//设定要发送的消息内容
            notification.IosNotification = new IosNotification().incrBadge(1);
            pushPayload.notification = notification;

            //添加额外的数据
            if (extraParameters != null && extraParameters.Count > 0)
            {
                foreach (var dicItem in extraParameters)
                {
                    notification.IosNotification.AddExtra(dicItem.Key, dicItem.Value);
                }
            }

            pushPayload.message = Message.content(message);

            return SendPush(pushPayload);
        }


        public static bool PushObject_ios_audienceMore_messageWithExtras(string message, Dictionary<string, string> extraParameters, params string[] tags)
        {

            var pushPayload = new PushPayload();
            pushPayload.platform = Platform.android_ios();
            pushPayload.audience = Audience.s_tag(tags);
            pushPayload.message = Message.content(message);
            //添加额外的数据
            if (extraParameters != null && extraParameters.Count > 0)
            {
                foreach (var dicItem in extraParameters)
                {
                    pushPayload.notification.IosNotification.AddExtra(dicItem.Key, dicItem.Value);
                }
            }

            return SendPush(pushPayload);
        }
    }
}
