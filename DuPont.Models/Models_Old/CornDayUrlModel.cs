using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Models
{
    public class CornDayUrlModel
    {
        public List<dayurlmodel> dayurllist { get; set; }
       
    }
    public class dayurlmodel
    {
        public string title { get; set; }
        public string href { get; set; }
    }

}
