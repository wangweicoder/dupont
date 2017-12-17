// ***********************************************************************
// Assembly         : DuPont
// Author           : 毛文君
// Created          : 08-05-2015
//
// Last Modified By : 毛文君
// Last Modified On : 08-05-2015
// ***********************************************************************
// <copyright file="NinjectDependencyResolver.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using DuPont.Interface;
using DuPont.Repository;
using DuPont.Utility.LogModule;
using DuPont.Utility.LogModule.Model;
using DuPont.Utility.LogModule.Service;
using Microsoft.Practices.ServiceLocation;
using Ninject;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace DuPont.Admin.Presentation.Infrastructure
{
    /// <summary>
    /// 依赖解析器
    /// </summary>
    public class NinjectDependencyResolver : IDependencyResolver, IServiceLocator
    {
        private IKernel kernel;
        public NinjectDependencyResolver()
        {
            kernel = new StandardKernel();
            AddBindings();
        }
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

        private void AddBindings()
        {
            kernel.Bind<DuPont.Interface.IUser>().To<UserRepository>();
            kernel.Bind<DuPont.Interface.IUser_Role>().To<User_RoleRepository>();
            kernel.Bind<DuPont.Interface.IMenu>().To<MenuRepository>();
            kernel.Bind<DuPont.Interface.IMenu_Role>().To<Menu_RoleRepository>();
            //kernel.Bind<IAuthProvider>().To<FormsAuthProvider>();
            kernel.Bind<IRoleVerification>().To<RoleVerificationRepository>();
            kernel.Bind<IFarmerVerficationInfoRepository>().To<FarmerVerficationInfoRepository>();
            kernel.Bind<IOperatorInfoVerifciationRepository>().To<OperatorInfoVerificationRepository>();
            kernel.Bind<IBusinessVerificationRepository>().To<BusinessVerificationInfoRepository>();
            kernel.Bind<IArea>().To<AreaRepository>();
            kernel.Bind<IFileInfoRepository>().To<FileInfoRepository>();
            kernel.Bind<ISys_Dictionary>().To<Sys_DictionaryRepository>();
            kernel.Bind<ICommon>().To<CommonRepository>();
            kernel.Bind<ISuppliers_Sarea>().To<Suppliers_SareaRepository>();
            kernel.Bind<ILogRepository<DP_Log>>().To<LogRepository>();
            kernel.Bind<ISysLog>().To<SysLogRepository>().WithPropertyValue("sysLogRepository", new SysLogRepository());
        }
    }
}