using DuPont.Admin.Presentation.Filters;
using DuPont.Global.Filters.ActionFilters;
using DuPont.Interface;
// ***********************************************************************
// Assembly         : DuPont
// Author           : 毛文君
// Created          : 08-05-2015
//
// Last Modified By : 毛文君
// Last Modified On : 08-05-2015
// ***********************************************************************
// <copyright file="AdminController.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace DuPont.Admin.Presentation.Controllers
{

    [SysAuthAttribute]
    public class AdminController:Controller
    {

        public AdminController()
        {           
         
        }

        //
        // GET: /Admin/
        [Validate]
        public ActionResult Index()
        {           
            return View();
        }
	}
}