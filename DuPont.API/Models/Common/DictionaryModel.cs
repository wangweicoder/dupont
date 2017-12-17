using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DuPont.API.Models.Common
{
    public class DictionaryModel
    {
        public int Code { get; set; }
        public string DisplayName { get; set; }

        public int Order { get; set; }
    }
}