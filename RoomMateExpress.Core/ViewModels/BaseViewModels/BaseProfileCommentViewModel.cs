using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using RoomMateExpress.Core.Models;

namespace RoomMateExpress.Core.ViewModels.BaseViewModels
{
    public class BaseProfileCommentViewModel : BaseViewModel
    {
        private BaseUserViewModel _userCommentator;
        private string _text;
        private byte _grade;
        private DateTimeOffset _commentedAt;
        private BaseUserViewModel _userProfile;

        public BaseUserViewModel UserCommentator
        {
            get => _userCommentator;
            set => SetProperty(ref _userCommentator, value);
        }

        public BaseUserViewModel UserProfile
        {
            get => _userProfile;
            set => SetProperty(ref _userProfile, value);
        }

        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }

        public byte Grade
        {
            get => _grade;
            set => SetProperty(ref _grade, value);
        }

        public DateTimeOffset CommentedAt
        {
            get => _commentedAt;
            set => SetProperty(ref _commentedAt, value);
        }

        public BaseProfileCommentViewModel()
        {

        }

        public BaseProfileCommentViewModel(ProfileComment commentForProfile) : base(commentForProfile.Id)
        {
            Mapper.Map(commentForProfile, this);
        }
    }
}
