using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Dtos
{
    /// <summary>
    /// 分页输入
    /// </summary>
    public class PaginationInput
    {
        private int? _pageIndex;
        private int? _pageSize;
        public int? PageIndex
        {
            get
            {
                if (_pageIndex == null) return 1;

                return _pageIndex;
            }
            set
            {
                if (value.HasValue && value.Value > 0)
                    _pageIndex = value;
            }
        }

        public int? PageSize
        {
            get
            {
                if (_pageSize == null) return 10;

                return _pageSize;
            }
            set
            {
                if (value.HasValue && value.Value > 0)
                    _pageSize = value;
            }
        }
    }
}
