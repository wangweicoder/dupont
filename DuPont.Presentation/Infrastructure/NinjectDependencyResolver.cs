// ***********************************************************************
// Assembly         : DuPont.Presentation
// Author           : 毛文君
// Created          : 10-27-2015
// Tel              :15801270290
// QQ               :731314565
//
// Last Modified By : 毛文君
// Last Modified On : 12-07-2015
// ***********************************************************************
// <copyright file="NinjectDependencyResolver.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using DuPont.Interface;
using DuPont.Repository;
using DuPont.Utility.LogModule.Model;
using Microsoft.Practices.ServiceLocation;
using Ninject;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace DuPont.Presentation.Infrastructure
{
    /// <summary>
    /// 依赖解析器
    /// </summary>
    public class NinjectDependencyResolver : IDependencyResolver, IServiceLocator
    {
        private static IKernel kernel;
        public static IKernel Kernel
        {
            get { return NinjectDependencyResolver.kernel; }
            private set { NinjectDependencyResolver.kernel = value; }
        }

        public NinjectDependencyResolver()
        {
            kernel = new StandardKernel();
            AddBindings();
        }

        #region "解析对象实例"
        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        public IEnumerable<TService> GetAllInstances<TService>()
        {
            return kernel.GetAll<TService>();
        }

        public IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        public TService GetInstance<TService>(string key)
        {
            return kernel.TryGet<TService>(key);
        }

        public TService GetInstance<TService>()
        {
            return kernel.TryGet<TService>();
        }

        public object GetInstance(Type serviceType, string key)
        {
            return kernel.TryGet(serviceType, key);
        }

        public object GetInstance(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        } 
        #endregion

        #region "添加绑定"
        private void AddBindings()
        {
            kernel.Bind<ISms>().To<RongLianYunSmsRepository>();
            kernel.Bind<ISysLog>().To<SysLogRepository>();
        } 
        #endregion
    }
}