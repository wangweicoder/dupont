﻿using DuPont.Models;
using DuPont.Utility.LogModule.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Utility.LogModule.Converter
{
   public  class StackTracePatternLayoutConverter:LevelPatternLayoutConverter
    {
       protected override void Convert(System.IO.TextWriter writer, log4net.Core.LoggingEvent loggingEvent)
       {
           var logModel = loggingEvent.MessageObject as DP_Log;
           if (logModel != null)
           {
               writer.Write(logModel.StackTrace);
           }
       }
    }
}
