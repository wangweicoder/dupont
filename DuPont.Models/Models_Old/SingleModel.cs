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
// <copyright file="SingleModel.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using DuPont.Models.Enum;


namespace DuPont.Models.Models
{
    public class SingleModel<TEntity>  where TEntity : class,new()
    {
        public bool IsSuccess { get; set; }
        public ActionType ActionType { get; set; }
        public TEntity Data { get; set; }
    }
}
