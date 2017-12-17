// ***********************************************************************
// Assembly         : DuPont.API
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
using Microsoft.Practices.ServiceLocation;
using Ninject;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace DuPont.API.Infrastructure
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
            kernel.Bind<IMenu>().To<MenuRepository>();
            kernel.Bind<IMenu_Role>().To<Menu_RoleRepository>();
            kernel.Bind<IPublished_Demand>().To<Published_DemandRepository>();
            kernel.Bind<IFarmerRequirement>().To<FarmerRequirementRepository>();
            kernel.Bind<ISys_Dictionary>().To<Sys_DictionaryRepository>();
            kernel.Bind<IFarmerVerficationInfoRepository>().To<FarmerVerficationInfoRepository>();
            kernel.Bind<IOperatorInfoVerifciationRepository>().To<OperatorInfoVerificationRepository>();
            kernel.Bind<IBusinessVerificationRepository>().To<BusinessVerificationInfoRepository>();
            kernel.Bind<IBusiness>().To<BusinessRepository>();
            kernel.Bind<IArea>().To<AreaRepository>();
            kernel.Bind<IFileInfoRepository>().To<FileInfoRepository>();
            kernel.Bind<IOperator>().To<OperatorRepository>();
            kernel.Bind<ICommon>().To<CommonRepository>();
            kernel.Bind<ISms>().To<RongLianYunSmsRepository>();
            kernel.Bind<ICarouselRepository>().To<CarouselRepository>();
            kernel.Bind<ISysSetting>().To<SysSettingRepository>();
            kernel.Bind<IUser_Password_History>().To<User_Password_HistoryRepository>();
            kernel.Bind<ISmsMessage>().To<SmsMessageRepository>();
            kernel.Bind<ISysLog>().To<SysLogRepository>();
            kernel.Bind<IArticle>().To<ArticleRepository>();
            kernel.Bind<IArticleCategory>().To<ArticleCategoryRepository>();
            kernel.Bind<ILearningGardenCarousel>().To<LearningGardenCarouselRepository>();
            kernel.Bind<IAdminUser>().To<AdminUserRepository>();
            kernel.Bind<IRoleVerification>().To<RoleVerificationRepository>();
            kernel.Bind<IUser_Role>().To<User_RoleRepository>();
            kernel.Bind<IExpertPermission>().To<ExpertPermissionRepository>();
            kernel.Bind<IExpertQuestion>().To<ExpertQuestionRepository>();
            kernel.Bind<IExpertQuestionReply>().To<ExpertQuestionReplyRepository>();
            kernel.Bind<IFarm>().To<FarmRepository>();
            kernel.Bind<IFarmArea>().To<FarmAreaRepository>();
            kernel.Bind<IFarmBooking>().To<FarmBookingRepository>();
            kernel.Bind<IUserToken>().To<User_TokenRepository>();
            kernel.Bind<INotification>().To<NotificationRepository>();
            kernel.Bind<IPublicNotification>().To<PublicNotificationRepository>();
            kernel.Bind<IPersonalNotification>().To<PersonalNotificationRepository>();
            kernel.Bind<IVisitNotification>().To<VisitNotificationRepository>();

            kernel.Bind<IQQUser>().To<QQUserRepository>();
            kernel.Bind<IWeChatUser>().To<WeChatUserRepository>();
            kernel.Bind<ISendNotificationResult>().To<SendNotificationResultRepository>();
            kernel.Bind<IMachineDemandTypeRL>().To<MachineDemandTypeRLRepository>();
            kernel.Bind<IUserRoleDemandTypeLevelRL>().To<UserRoleDemandTypeLevelRLRepository>();
        }
    }
}