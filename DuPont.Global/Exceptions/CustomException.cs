using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Global.Exceptions
{
    public class CustomException : Exception
    {
        public CustomException(string message)
            : base(message)
        {

        }
    }
}
