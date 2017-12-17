using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Dtos.Background.User
{
    public class SearchUserListInputDto
    {
        private int? _pageIndex;
        private int? _pageSize;

        public int? pageIndex
        {
            get
            {
                return _pageIndex ?? 1;
            }
            set
            {
                if (value.HasValue && value.Value > 0)
                    _pageIndex = value;
            }
        }

        public int? pageSize
        {
            get
            {
                return _pageSize ?? 10;
            }
            set
            {
                if (value.HasValue && value.Value > 0)
                {
                    if (value.Value > 10000)
                        _pageSize = 10000;
                    else
                        _pageSize = value;
                }
            }
        }

        public string RoleId { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PhoneNumber { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }

        public int? UserTypeId { get; set; }
    }
}
