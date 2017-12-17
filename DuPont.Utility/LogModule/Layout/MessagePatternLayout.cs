using DuPont.Utility.LogModule.Converter;
using log4net.Layout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Utility.LogModule.Layout
{
    public class MessagePatternLayout : PatternLayout
    {
        public MessagePatternLayout()
        {
            this.AddConverter("Message", typeof(MessagePatternLayoutConverter));
        }
    }
}
