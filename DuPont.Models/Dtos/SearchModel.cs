using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webdiyer.WebControls.Mvc;

namespace DuPont.Models.Dtos
{
    public class SearchModel<TSearchInput, TSearchResult>
        where TSearchInput : PaginationInput
        where TSearchResult : ICollection
    {
        public SearchModel()
        {

        }
        public SearchModel(bool isSuccess, TSearchInput searchInput, TSearchResult searchResult, int totalNums)
        {
            IsSuccess = isSuccess;
            SearchCondition = searchInput;
            SearchResult = searchResult;
            Pagination = new PagedList<string>(new string[0], searchInput.PageIndex.Value, searchInput.PageSize.Value, totalNums);
            RecordCount = totalNums;
            if (isSuccess)
            {
                if (searchResult.Count == 0)
                    SuccMessage = "未获取到匹配的数据!";
            }
            else
            {
                ErrorMessage = "获取数据过程中出现异常,请联系技术支持!";
            }

        }

        /// <summary>
        /// 搜索条件的模型类型
        /// </summary>
        public TSearchInput SearchCondition { get; set; }

        /// <summary>
        /// 返回结果的结果集类型
        /// </summary>
        public TSearchResult SearchResult { get; set; }

        /// <summary>
        /// 返回的总记录条数
        /// </summary>
        public int RecordCount { get; set; }

        public IPagedList Pagination { get; set; }

        /// <summary>
        /// 请求是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 成功消息
        /// </summary>
        public string SuccMessage { get; set; }

        /// <summary>
        /// 失败消息
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}
