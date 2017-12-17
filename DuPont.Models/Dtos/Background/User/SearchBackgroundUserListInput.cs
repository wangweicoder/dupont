using DuPont.Entity.Enum;
using DuPont.Models.DataAnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Dtos.Background.User
{
    public class SearchBackgroundUserListInput : PaginationInput
    {
        private DateTime? _regStartTime;
        private DateTime? _regEndTime;

        /// <summary>
        /// 省份
        /// </summary>
        [RegularExpression(@"\d+", ErrorMessage = "省份地址不正确!")]
        public string Province { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        [RegularExpression(@"\d+", ErrorMessage = "城市地址不正确!")]
        public string City { get; set; }

        /// <summary>
        /// 区县
        /// </summary>
         [RegularExpression(@"\d+", ErrorMessage = "区县地址不正确!")]
        public string Region { get; set; }

        /// <summary>
        /// 角色类型
        /// </summary>
        public RoleType RoleType { get; set; }

        /// <summary>
        /// 注册的开始时间
        /// </summary>
        [XSSJavaScript]
        public DateTime? RegStartTime
        {
            get
            {
                return _regStartTime;
            }
            set
            {
                if (value.HasValue) _regStartTime = value;
            }
        }

        /// <summary>
        /// 注册的截止时间
        /// </summary>
        [XSSJavaScript]
        public DateTime? RegEndTime
        {
            get
            {
                return _regEndTime;
            }
            set
            {
                if (value.HasValue)
                {
                    if (_regStartTime.HasValue)
                    {
                        if (value.Value >= _regStartTime.Value)
                        {
                            _regEndTime = value;
                        }
                    }
                    else
                    {
                        _regEndTime = value;
                    }
                }
            }
        }

        /// <summary>
        /// 是否锁定
        /// </summary>
        public bool? IsLock { get; set; }

        /// <summary>
        /// 登录账号名
        /// </summary>
        [SQLValidate]
        [XSSJavaScript]
        public string UserName { get; set; }
    }
}
