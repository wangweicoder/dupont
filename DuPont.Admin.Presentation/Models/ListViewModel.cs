// ***********************************************************************
// Assembly         : DuPont.Admin.Presentation
// Author           : 毛文君
// Created          : 12-13-2015
// Tel              :15801270290
// QQ               :731314565
//
// Last Modified By : 毛文君
// Last Modified On : 12-13-2015
// ***********************************************************************
// <copyright file="ListViewModel.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections.Generic;
using Webdiyer.WebControls.Mvc;
namespace DuPont.Admin.Presentation.Models
{
    public class ListViewModel<TEntity> where TEntity : class,new()
    {
        public ListViewModel(bool success, int pageIndex, int pageSize, long totalCount, List<TEntity> data)
        {
            this.Success = success;
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
            this.TotalCount = totalCount;
            this.Data = data;
            this.Pager = new PagedList<string>(new string[0], this.PageIndex, this.PageSize, (int)this.TotalCount);
        }
        public bool Success { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public long TotalCount { get; set; }
        public List<TEntity> Data { get; set; }
        public PagedList<string> Pager { get; set; }
    }
}