// ***********************************************************************
// Assembly         : DuPont.Repository
// Author           : 毛文君
// Created          : 09-28-2015
//
// Last Modified By : 毛文君
// Last Modified On : 09-28-2015
// ***********************************************************************
// <copyright file="LogRepository.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using DuPont.Interface;
using DuPont.Utility.LogModule;
using DuPont.Utility.LogModule.Model;
using DuPont.Utility.LogModule.Service;

namespace DuPont.Repository
{
    public class LogRepository : ILogRepository<DP_Log>
    {
        public void Info(DP_Log log)
        {
            LogUtil<DP_Log>.Info(log);
        }

        public void Warn(DP_Log log)
        {
            LogUtil<DP_Log>.Warn(log);
        }
        public void Debug(DP_Log log)
        {
            LogUtil<DP_Log>.Debug(log);
        }

        public void Error(DP_Log log)
        {
            LogUtil<DP_Log>.Error(log);
        }

        public void Fatal(DP_Log log)
        {
            LogUtil<DP_Log>.Fatal(log);
        }
    }
}
