using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Models
{
    public class LogModel
    {
        public long Id { get; set; }
        public string Level { get; set; }
        public string StackTrace { get; set; }
        public string Message { get; set; }
        public DateTime CreateTime { get; set; }

    }
}
