using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DuPont.API.Models.Account
{
    public class ProductInfo
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Acreage { get; set; }
    }
}