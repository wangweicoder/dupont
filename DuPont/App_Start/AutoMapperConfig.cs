using AutoMapper;
using DuPont.Models.Dtos.Background.Config;
using DuPont.Models.Dtos.Background.LearningWorld;
using DuPont.Models.Dtos.Background.Notification;
using DuPont.Models.Dtos.Background.Question;
using DuPont.Models.Dtos.Background.User;
using DuPont.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DuPont
{
    public class AutoMapperConfig
    {
        public static void Configure()
        {
            Mapper.CreateMap<T_ARTICLE_CATEGORY, ArticleCategory>()
                .ForMember(des => des.CatId, opt => opt.MapFrom(source => source.CategoryId))
                .ForMember(des => des.CatName, opt => opt.MapFrom(source => source.Name));

            Mapper.CreateMap<ArticleCategory, T_ARTICLE_CATEGORY>()
                .ForMember(des => des.CategoryId, opt => opt.MapFrom(source => source.CatId))
                .ForMember(des => des.Name, opt => opt.MapFrom(source => source.CatName));

            Mapper.CreateMap<T_ARTICLE, ArticleListSearchOutput>();

            Mapper.CreateMap<ArticleInput, T_ARTICLE>();

            Mapper.CreateMap<T_ARTICLE, Article>()
                .ForMember(des => des.CatId, opt => opt.MapFrom(source => source.T_ARTICLE_CATEGORY.CategoryId))
                .ForMember(des => des.CatName, opt => opt.MapFrom(source => source.T_ARTICLE_CATEGORY.Name));

            Mapper.CreateMap<T_USER, SearchExpertListOutput>()
                .ForMember(des => des.UserId, opt => opt.MapFrom(source => source.Id));

            Mapper.CreateMap<T_USER, SearchUserListWithStateOutput>()
                .ForMember(des => des.UserId, opt => opt.MapFrom(source => source.Id));

            Mapper.CreateMap<T_QUESTION, SearchQutionOutput>();

            Mapper.CreateMap<T_QUESTION_REPLY, QuestionReply>()
                .ForMember(des => des.ReplyId, opt => opt.MapFrom(source => source.Id))
                .ForMember(des => des.ReplyTime, opt => opt.MapFrom(source => source.CreateTime));

            Mapper.CreateMap<T_DEMONSTRATION_FARM, FarmListOutput>()
                .ForMember(des => des.Province, opt => opt.MapFrom(source => source.ProvinceAid))
                .ForMember(des => des.City, opt => opt.MapFrom(source => source.CityAid))
                .ForMember(des => des.Region, opt => opt.MapFrom(source => source.RegionAid));

            Mapper.CreateMap<T_AREA, AreaViewModel>();
            Mapper.CreateMap<T_NOTIFICATION, CreatePublicNotificationTaskOutput>();
            Mapper.CreateMap<T_NOTIFICATION, PersonalNotificationListOutput>()
                .ForMember(des => des.PhoneNumber, opt => opt.MapFrom(source => source.TargetUser.LoginUserName ?? source.TargetUser.PhoneNumber))
                .ForMember(des => des.IosDeviceToken, opt => opt.MapFrom(source => source.TargetUser.IosDeviceToken));

            Mapper.CreateMap<T_USER, PublicNotificationUserListOutput>()
                .ForMember(des => des.UserId, opt => opt.MapFrom(source => source.Id))
                .ForMember(des => des.PhoneNumber, opt => opt.MapFrom(source => source.LoginUserName ?? source.PhoneNumber));

            Mapper.CreateMap<T_FARM_BOOKING, FarmBookItem>()
                .ForMember(des => des.BookDate, opt => opt.MapFrom(source => source.CreateTime))
                .ForMember(des => des.PhoneNumber, opt => opt.MapFrom(source => source.ReservedUser.PhoneNumber))
                .ForMember(des => des.Visitor, opt => opt.MapFrom(source => source.ReservedUser.UserName ?? source.ReservedUser.LoginUserName));

            Mapper.CreateMap<T_FARM_AREA, ExhibitionAreaListOutput>()
                .ForMember(des => des.ExhibitionAreaId, opt => opt.MapFrom(source => source.Id))
                .ForMember(des => des.ExhibitionAreaName, opt => opt.MapFrom(source => source.Name));

            Mapper.CreateMap<BackgroundUserModel, SearchBackgroundUserListOutput>()
                .ForMember(des => des.LoginUserName, opt => opt.MapFrom(source => source.UserName))
                .ForMember(des => des.ProvinceCode, opt => opt.MapFrom(source => source.Province))
                .ForMember(des => des.CityCode, opt => opt.MapFrom(source => source.City))
                .ForMember(des => des.RegionCode, opt => opt.MapFrom(source => source.Region))
                .ForMember(des => des.IsLocked, opt => opt.MapFrom(source => source.IsLock))
                .ForMember(des => des.RegisterTime, opt => opt.MapFrom(source => source.CreateTime));

            Mapper.CreateMap<T_DEMONSTRATION_FARM, DemonstateFarmDetailViewModel>();

            Mapper.CreateMap<T_SYS_SETTING, SystemSettingViewModel>();
        }
    }
}