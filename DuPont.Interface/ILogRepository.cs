using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Interface
{
    public interface ILogRepository<TLog> where TLog : class,new()
    {
        void Info(TLog log);
        void Warn(TLog log);
        void Debug(TLog log);
        void Error(TLog log);
        void Fatal(TLog log);
    }
}
