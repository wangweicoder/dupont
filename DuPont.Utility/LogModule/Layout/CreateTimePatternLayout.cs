﻿using DuPont.Utility.LogModule.Converter;
using log4net.Layout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Utility.LogModule.Layout
{
    public class CreateTimePatternLayout : PatternLayout
    {
       public CreateTimePatternLayout()
        {
            this.AddConverter("CreateTime", typeof(CreateTimePatternLayoutConverter));
        }
    }
}
