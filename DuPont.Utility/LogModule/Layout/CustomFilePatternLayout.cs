using DuPont.Utility.LogModule.Converter;
using log4net.Layout;

namespace DuPont.Utility.LogModule.Layout
{
    public class CustomFilePatternLayout : PatternLayout
    {
        public CustomFilePatternLayout()
        {
            this.AddConverter("CreateTime", typeof(CreateTimePatternLayoutConverter));
            this.AddConverter("Level", typeof(LevelPatternLayoutConverter));
            this.AddConverter("Message", typeof(MessagePatternLayoutConverter));
            this.AddConverter("StackTrace", typeof(StackTracePatternLayoutConverter));
        }
    }
}
