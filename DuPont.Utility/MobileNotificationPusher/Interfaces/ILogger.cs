using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Utility.Core.MobileNotificationPusher.Interfaces
{
    public interface ILogger
    {
        void Debug(string format, params object[] objs);
        void Info(string format, params object[] objs);
        void Warning(string format, params object[] objs);
        void Error(string format, params object[] objs);
    }
}
