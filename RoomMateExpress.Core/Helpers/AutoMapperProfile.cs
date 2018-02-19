using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using MvvmCross.Platform;
using RoomMateExpress.Core.Models;
using RoomMateExpress.Core.ViewModels;
using RoomMateExpress.Core.ViewModels.BaseViewModels;

namespace RoomMateExpress.Core.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<PostsFilterViewModel, PostsFilterViewModel>()
                .ConstructUsingServiceLocator();
            CreateMap<BaseUserViewModel, BaseUserViewModel>()
                .ConstructUsingServiceLocator();
            CreateMap<BasePostPictureViewModel, BasePostPictureViewModel>()
                .ConstructUsingServiceLocator();
            CreateMap<BasePostViewModel, BasePostViewModel>()
                .ConstructUsingServiceLocator();
            CreateMap<BaseAdminViewModel, BaseAdminViewModel>()
                .ConstructUsingServiceLocator();
            CreateMap<BaseChatViewModel, BaseChatViewModel>()
                .ConstructUsingServiceLocator();
            CreateMap<BasePostCommentViewModel, BasePostCommentViewModel>()
                .ConstructUsingServiceLocator();
            CreateMap<BaseProfileCommentViewModel, BaseProfileCommentViewModel>()
                .ConstructUsingServiceLocator();
            CreateMap<BaseCityViewModel, BaseCityViewModel>()
                .ConstructUsingServiceLocator();
            CreateMap<BaseMessageViewModel, BaseMessageViewModel>()
                .ConstructUsingServiceLocator();
            CreateMap<BaseNeighborhoodViewModel, BaseNeighborhoodViewModel>()
                .ConstructUsingServiceLocator();
            CreateMap<BaseUserReportViewModel, BaseUserReportViewModel>()
                .ConstructUsingServiceLocator();
            CreateMap<BaseAdminViewModel, Admin>()
                .ConstructUsingServiceLocator().ReverseMap();
            CreateMap<BaseChatViewModel, Chat>()
                .ConstructUsingServiceLocator().ReverseMap();
            CreateMap<BaseCityViewModel, City>()
                .ConstructUsingServiceLocator().ReverseMap();
            CreateMap<BasePostCommentViewModel, PostComment>()
                .ConstructUsingServiceLocator().ReverseMap();
            CreateMap<BaseProfileCommentViewModel, ProfileComment>()
                .ConstructUsingServiceLocator().ReverseMap();
            CreateMap<BaseMessageViewModel, Message>()
                .ConstructUsingServiceLocator().ReverseMap();
            CreateMap<BaseNeighborhoodViewModel, Neighborhood>()
                .ConstructUsingServiceLocator().ReverseMap();
            CreateMap<BasePostViewModel, Post>()
                .ConstructUsingServiceLocator().ReverseMap();
            CreateMap<BasePostPictureViewModel, PostPicture>()
                .ConstructUsingServiceLocator().ReverseMap();
            CreateMap<BaseUserViewModel, User>()
                .ConstructUsingServiceLocator().ReverseMap();
            CreateMap<BaseUserReportViewModel, UserReport>()
                .ConstructUsingServiceLocator().ReverseMap();
        }
    }
}
