using DuPont.Utility.LogModule.Model;
using log4net;
using log4net.Appender;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Utility.LogModule.Service
{
    public static class LogUtil<TLog> where TLog : class,ILogBase, new()
    {
        private static ILog log;

        public static void Initial()
        {
            log = LogManager.GetLogger("FF");
            Assembly assembly = Assembly.GetExecutingAssembly();
            System.IO.Stream stream = assembly.GetManifestResourceStream("DuPont.Utility.log4net.config");
            log4net.Config.XmlConfigurator.Configure(stream);
        }

        public static void Error(TLog logItem)
        {
            if (log == null)
            {
                Initial();
            }
            if (log.IsErrorEnabled)
            {
                logItem.Level = "Error";
                log.Error(logItem);
            }
        }

        public static void Debug(TLog logItem)
        {
            if (log == null)
            {
                Initial();
            }

            if (log.IsDebugEnabled)
            {
                logItem.Level = "Debug";
                log.Debug(logItem);
            }
        }

        public static void Fatal(TLog logItem)
        {
            if (log == null)
            {
                Initial();
            }
            if (log.IsFatalEnabled)
            {
                logItem.Level = "Fatal";
                log.Fatal(logItem);
            }
        }

        public static void Info(TLog logItem)
        {
            if (log == null)
            {
                Initial();
            }
            if (log.IsInfoEnabled)
            {
                logItem.Level = "Info";
                log.Info(logItem);
            }
        }

        public static void Warn(TLog logItem)
        {
            if (log == null)
            {
                Initial();
            }
            if (log.IsWarnEnabled)
            {
                logItem.Level = "Warn";
                log.Warn(logItem);
            }
        }
    }
}
