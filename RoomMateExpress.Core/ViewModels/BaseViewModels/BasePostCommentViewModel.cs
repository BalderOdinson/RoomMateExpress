using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using RoomMateExpress.Core.Models;

namespace RoomMateExpress.Core.ViewModels.BaseViewModels
{
    public class BasePostCommentViewModel : BaseViewModel
    {
        private BaseUserViewModel _user;
        private string _text;
        private DateTimeOffset _commentedAt;
        private BasePostViewModel _post;

        public BaseUserViewModel User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }

        public DateTimeOffset CommentedAt
        {
            get => _commentedAt;
            set => SetProperty(ref _commentedAt, value);
        }

        public BasePostViewModel Post
        {
            get => _post;
            set => SetProperty(ref _post, value);
        }

        public BasePostCommentViewModel()
        {

        }

        public BasePostCommentViewModel(PostComment commentForPost) : base(commentForPost.Id)
        {
            Mapper.Map(commentForPost, this);
        }
    }
}
