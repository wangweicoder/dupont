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
using DuPont.Utility.LogModule.Model;
using Microsoft.Practices.ServiceLocation;
using Ninject;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace DuPont.Infrastructure
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
            kernel.Bind<IUser>().To<UserRepository>();
            kernel.Bind<IAdminUser>().To<AdminUserRepository>();
            kernel.Bind<IUser_Role>().To<User_RoleRepository>();
            kernel.Bind<IMenu>().To<MenuRepository>();
            kernel.Bind<IMenu_Role>().To<Menu_RoleRepository>();
            kernel.Bind<IAuthProvider>().To<FormsAuthProvider>();
            kernel.Bind<IRoleVerification>().To<RoleVerificationRepository>();
            kernel.Bind<IFarmerVerficationInfoRepository>().To<FarmerVerficationInfoRepository>();
            kernel.Bind<IOperatorInfoVerifciationRepository>().To<OperatorInfoVerificationRepository>();
            kernel.Bind<IBusinessVerificationRepository>().To<BusinessVerificationInfoRepository>();
            kernel.Bind<IArea>().To<AreaRepository>();
            kernel.Bind<IFileInfoRepository>().To<FileInfoRepository>();
            kernel.Bind<ISys_Dictionary>().To<Sys_DictionaryRepository>();
            kernel.Bind<ICommon>().To<CommonRepository>();
            kernel.Bind<ISuppliers_Sarea>().To<Suppliers_SareaRepository>();
            kernel.Bind<ISysSetting>().To<SysSettingRepository>();
            kernel.Bind<IUser_Password_History>().To<User_Password_HistoryRepository>();
            kernel.Bind<ISysLog>().To<SysLogRepository>();
            kernel.Bind<IApp_Version>().To<APP_VersionRepository>();
            kernel.Bind<IPermission>().To<PermissionRepository>();
            kernel.Bind<IPermissionProvider>().To<AdminPermissionProvider>();
            kernel.Bind<IFarmerRequirement>().To<FarmerRequirementRepository>();
            kernel.Bind<IBusiness>().To<BusinessRepository>();
            kernel.Bind<IArticle>().To<ArticleRepository>();
            kernel.Bind<IArticleCategory>().To<ArticleCategoryRepository>();
            kernel.Bind<IExpertPermission>().To<ExpertPermissionRepository>();
            kernel.Bind<IExpertQuestion>().To<ExpertQuestionRepository>();
            kernel.Bind<IExpertQuestionReply>().To<ExpertQuestionReplyRepository>();
            kernel.Bind<IFarm>().To<FarmRepository>();
            kernel.Bind<IFarmArea>().To<FarmAreaRepository>();
            kernel.Bind<INotification>().To<NotificationRepository>();
            kernel.Bind<IPublicNotification>().To<PublicNotificationRepository>();
            kernel.Bind<IPersonalNotification>().To<PersonalNotificationRepository>();
            kernel.Bind<IFarmBooking>().To<FarmBookingRepository>();
            kernel.Bind<IUserRoleDemandTypeLevelRL>().To<UserRoleDemandTypeLevelRLRepository>();
            kernel.Bind<IMachineDemandTypeRL>().To<MachineDemandTypeRLRepository>();
            kernel.Bind<IBusinessDemand_Response>().To<BusinessDemand_ResponseRepository>();
            kernel.Bind<IFarmerDemandResponse>().To<FarmerDemandResponseRepository>();
        }
    }
}