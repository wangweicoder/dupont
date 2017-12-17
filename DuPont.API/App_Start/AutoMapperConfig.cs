// ***********************************************************************
// Assembly         : DuPont.API
// Author           : 毛文君
// Created          : 12-09-2015
// Tel              :15801270290
// QQ               :731314565
//
// Last Modified By : 毛文君
// Last Modified On : 12-09-2015
// ***********************************************************************
// <copyright file="AutoMapperConfig.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using AutoMapper;
using DuPont.Utility;
using DuPont.Models.Dtos.Foreground.LearningWorld;
using DuPont.Models.Dtos.Foreground.Expert;
using DuPont.Models.Models;
using DuPont.Models.Dtos.Foreground.Account;
using DuPont.Models.Dtos.Foreground.Notification;

namespace DuPont.API
{
    public class AutoMapperConfig
    {
        public static void Configure()
        {
            //Mapper.CreateMap<T_QUESTION,ExpertQuestionSearchInput>()
            // .ForMember(target => target, opt => opt.MapFrom(source => source))

            Mapper.CreateMap<T_ARTICLE_CATEGORY, NavigateMenuItemOutput>()
                .ForMember(target => target.CatId, opt => opt.MapFrom(source => source.CategoryId))
                .ForMember(target => target.Title, opt => opt.MapFrom(source => source.Name));

            Mapper.CreateMap<T_LEARNING_GARDEN_CAROUSEL, CarouselPictureOutput>()
                .ForMember(target => target.ArticleId, opt => opt.MapFrom(source => source.ArticleId))
                .ForMember(target => target.ArticleTitle, opt => opt.MapFrom(source => source.T_ARTICLE.Title))
                .ForMember(target => target.ArticleUrl, opt => opt.MapFrom(source => source.T_ARTICLE.Url))
                .ForMember(target => target.PictureUrl, opt => opt.MapFrom(source => source.T_ARTICLE.Content));

            Mapper.CreateMap<T_ARTICLE, ArticleListSearchOutput>();

            Mapper.CreateMap<T_ARTICLE, CarouselPictureOutput>()
                .ForMember(target => target.ArticleId, opt => opt.MapFrom(source => source.Id))
                .ForMember(target => target.ArticleTitle, opt => opt.MapFrom(source => source.Title))
                .ForMember(target => target.ArticleUrl, opt => opt.MapFrom(source => source.Url));

            Mapper.CreateMap<ExpertQuestionInput, T_QUESTION>()
                .ForMember(target => target.Description, opt => opt.MapFrom(source => source.Description))
                .ForMember(target => target.PictureIds, opt => opt.MapFrom(source => source.PictureIds))
                .ForMember(target => target.RoleId, opt => opt.MapFrom(source => source.RoleId))
                .ForMember(target => target.Title, opt => opt.MapFrom(source => source.Title))
                .ForMember(target => target.UserId, opt => opt.MapFrom(source => source.CreateUserId))
                .ForMember(target => target.QuestionType, opt => opt.MapFrom(source => source.QuestionType.ToString()));

            Mapper.CreateMap<T_QUESTION, ExpertQuestionListOutput>()
                .ForMember(target => target.QuestionId, opt => opt.MapFrom(source => source.Id))
                .ForMember(target => target.CreateUserId, opt => opt.MapFrom(source => source.UserId))
                .ForMember(target => target.UserName, opt => opt.MapFrom(source => string.IsNullOrEmpty(source.User.UserName) ? source.User.LoginUserName : source.User.UserName))
                .ForMember(target => target.CreateTime, opt => opt.MapFrom(source => TimeHelper.GetMilliSeconds(source.CreateTime)));
            //增加是否采纳
            Mapper.CreateMap<T_QUESTION_REPLY, ExpertQuestionReplyListOutput>()
                .ForMember(target => target.ReplyId, opt => opt.MapFrom(source => source.Id))
                .ForMember(target => target.ReplyUserId, opt => opt.MapFrom(source => source.UserId))
                .ForMember(target=>target.IsAgree,opt=>opt.MapFrom(source=>source.IsAgree))
                .ForMember(target => target.ReplyTime, opt => opt.MapFrom(source => TimeHelper.GetMilliSeconds(source.CreateTime)));

            Mapper.CreateMap<T_QUESTION, ExpertMyQuestionListOutput>()
                .ForMember(target => target.QuestionId, opt => opt.MapFrom(source => source.Id))
                .ForMember(target => target.CreateTime, opt => opt.MapFrom(source => TimeHelper.GetMilliSeconds(source.CreateTime)));

            Mapper.CreateMap<T_QUESTION_REPLY, ExpertMyReplyListOutput>()
                .ForMember(target => target.ReplyTime, opt => opt.MapFrom(source => TimeHelper.GetMilliSeconds(source.CreateTime)));

            Mapper.CreateMap<ExpertQuestionReplyInput, T_QUESTION_REPLY>()
                .ForMember(target => target.UserId, opt => opt.MapFrom(source => source.ReplyUserId));

            Mapper.CreateMap<T_QUESTION, ExpertQuestionCarouselOutput>()
                .ForMember(target => target.QuestionId, opt => opt.MapFrom(source => source.Id))
                .ForMember(target => target.UserName, opt => opt.MapFrom(source => string.IsNullOrEmpty(source.User.UserName) ? source.User.LoginUserName : source.User.UserName));

            Mapper.CreateMap<T_DEMONSTRATION_FARM, FarmListOutput>()
                .ForMember(target => target.FarmId, opt => opt.MapFrom(source => source.Id));

            Mapper.CreateMap<T_DEMONSTRATION_FARM, FarmDetailOutput>()
                .ForMember(target => target.PlantAreaYield, opt => opt.MapFrom(source => source.PlantArea))
                .ForMember(target => target.OpenStartDate, opt => opt.MapFrom(source => source.OpenStartDate == null ? null : TimeHelper.GetMilliSeconds(source.OpenStartDate.Value)))
                .ForMember(target => target.OpenEndDate, opt => opt.MapFrom(source => source.OpenEndDate == null ? null : TimeHelper.GetMilliSeconds(source.OpenEndDate.Value)));

            Mapper.CreateMap<T_FARM_AREA, FarmArea>()
                .ForMember(target => target.ExhibitionAreaId, opt => opt.MapFrom(source => source.Id));

            Mapper.CreateMap<T_USER, UpdatePasswordOutput>();

            Mapper.CreateMap<T_NOTIFICATION, MessageItem>()
                .ForMember(target => target.Message, opt => opt.MapFrom(source => source.MsgContent))
                .ForMember(target => target.NotificationType, opt => opt.MapFrom(source => source.NotificationType))
                .ForMember(target => target.NotificationSource, opt => opt.MapFrom(source => source.NotificationSource))
                .ForMember(target=>target.NotificationSourceId,opt=>opt.MapFrom(source=>source.NotificationSourceId))
                .ForMember(target => target.IsOpen, opt => opt.MapFrom(source => source.IsOpen)); ;

        }
    }
}