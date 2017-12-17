// ***********************************************************************
// Assembly         : DuPont.Repository
// Author           : 毛文君
// Created          : 08-21-2015
//
// Last Modified By : 毛文君
// Last Modified On : 08-21-2015
// ***********************************************************************
// <copyright file="RongLianYunSmsRepository.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary>容联云通信短信接口实现</summary>
// ***********************************************************************
using DuPont.Interface;
using DuPont.Utility;
using System;
using System.Configuration;

namespace DuPont.Repository
{
    public class RongLianYunSmsRepository : ISms
    {
        public bool Send(string phoneNumber, string validateCode)
        {
            string ret = null;
            try
            {
                var api = new CCPRestSDK();
                //ip格式如下，不带https://
                string restAddress =ConfigHelper.GetAppSetting("restAddress");
                string restPort = ConfigHelper.GetAppSetting("restPort");
                string accountSid = ConfigHelper.GetAppSetting("accountSid");
                string accountToken = ConfigHelper.GetAppSetting("accountToken");
                string appId = ConfigHelper.GetAppSetting("appId");
                string smsTemplateId = ConfigHelper.GetAppSetting("smsTemplateId");
                int smsValidMinutes = Convert.ToInt32(ConfigHelper.GetAppSetting("smsValidMinutes"));
                var isInit = api.init(restAddress, restPort);
                api.setAccount(accountSid,accountToken);
              
                api.setAppId(appId);
                if (isInit)
                {
                    var retData = api.SendTemplateSMS(phoneNumber, smsTemplateId, new string[] { validateCode/*, smsValidMinutes.ToString()*/ });

                    var statusMsg = retData["statusMsg"].ToString();
                    if (statusMsg != "成功")
                    {
                        ret = statusMsg;
                    }
                }
                else
                {
                    ret = "初始化失败";
                }
            }
            catch (Exception exc)
            {
                ret = exc.Message;
            }

            return ret == null;
        }
    }
}
