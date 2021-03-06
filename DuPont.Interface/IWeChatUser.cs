﻿using DuPont.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Interface
{
    public interface IWeChatUser : IRepositoryBase<WeChatUser>
    {
        /// <summary>
        /// 通过第三方Id获取用户数据
        /// </summary>
        /// <param name="socialId"></param>
        /// <returns></returns>
        WeChatUser GetUserBy(string socialId);
    }
}
