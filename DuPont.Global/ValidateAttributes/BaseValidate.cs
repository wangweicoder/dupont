using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Global.ValidateAttributes
{
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class BaseValidate : ValidationAttribute
    {

    }
}
