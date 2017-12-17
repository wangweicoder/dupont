
using DuPont.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Interface
{
    public interface ILog : IRepository<T_SYS_LOG>
    {
        List<LogModel> GetLogListModel(int pageIndex, int pageSize, out long reocrdCount);
    }
}
