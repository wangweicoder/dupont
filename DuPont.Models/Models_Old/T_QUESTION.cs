using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Models
{
    public partial class T_QUESTION
    {
        public T_QUESTION()
        {
            this.CreateTime = DateTime.Now;
            this.LastModifiedTime = DateTime.Now;
        }
    }
}
