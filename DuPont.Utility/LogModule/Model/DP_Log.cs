// ***********************************************************************
// Assembly         : DuPont.Utility
// Author           : 毛文君
// Created          : 09-24-2015
//
// Last Modified By : 毛文君
// Last Modified On : 09-24-2015
// ***********************************************************************
// <copyright file="DP_Log.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Text;
namespace DuPont.Utility.LogModule.Model
{
    public class DP_Log:ILogBase
    {
        public long Id { get; set; }
        public string StackTrace { get; set; }
        public string Message { get; set; }
        public System.DateTime CreateTime { get; set; }

        private string level;
        public string Level
        {
            get
            {
                return level;
            }
            set
            {
                level = value;
            }
        }

        public override string ToString()
        {
            StringBuilder sb_Log = new StringBuilder();
            sb_Log.AppendLine("【日志消息】:{0}");
            sb_Log.AppendLine("【日志详情】:{1}");
            sb_Log.AppendLine("【产生时间】:{2}");

            return string.Format(sb_Log.ToString(),this.Message,this.StackTrace,this.CreateTime.ToString("yyyy/MM/dd HH:mm:ss"));
        }
    }
}
