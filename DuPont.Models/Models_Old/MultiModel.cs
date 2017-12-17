// ***********************************************************************
// Assembly         : DuPont.Models
// Author           : 毛文君
// Created          : 12-15-2015
// Tel              :15801270290
// QQ               :731314565
//
// Last Modified By : 毛文君
// Last Modified On : 12-15-2015
// ***********************************************************************
// <copyright file="MultiModel.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections.Generic;
using Webdiyer.WebControls.Mvc;
namespace DuPont.Models.Models
{
    public class MultiModel<TEntity> : SingleModel<TEntity> where TEntity : class,new()
    {
        public MultiModel(bool success, int pageIndex, int pageSize, int totalCount, TEntity data)
        {
            this.IsSuccess = success;
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
            this.RecordCount = totalCount;
            this.Data = data;
            this.Pager = new PagedList<string>(new string[0], this.PageIndex, this.PageSize, this.RecordCount);
        }
        /// <summary>
        /// Mvc分页
        /// </summary>
        public PagedList<string> Pager { get; set; }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int RecordCount { get; set; }

    }
}
