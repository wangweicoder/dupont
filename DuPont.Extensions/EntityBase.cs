using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Extensions
{
    public class EntityBase
    {
        public int Id { get; set; }
        public DateTime AddDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
