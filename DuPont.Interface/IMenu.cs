using DuPont.Models.Models;
using System;
// ***********************************************************************
// Assembly         : DuPont.Interface
// Author           : 毛文君
// Created          : 08-06-2015
//
// Last Modified By : 毛文君
// Last Modified On : 08-06-2015
// ***********************************************************************
// <copyright file="IMenu.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections.Generic;
namespace DuPont.Interface
{
    public interface IMenu : IRepository<T_MENU>
    {
        //IList<Entities.T_MENU> GetAll(System.Linq.Expressions.Expression<Func<Entities.T_MENU, bool>> predicate);
    }
}
