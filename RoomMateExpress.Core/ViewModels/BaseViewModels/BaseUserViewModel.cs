using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MvvmCross.Core.ViewModels;
using RoomMateExpress.Core.Helpers;
using RoomMateExpress.Core.Helpers.Enums;
using RoomMateExpress.Core.Models;

namespace RoomMateExpress.Core.ViewModels.BaseViewModels
{
    public class BaseUserViewModel : BaseViewModel
    {
        private string _firstName;
        private string _lastName;
        private decimal _avrageGrade;
        private string _profilePictureUrl;
        private byte _age;
        private bool _hasFaculty;
        private string _descriptionOfStudyOrWork;
        private bool _isSmoker;
        private Gender _gender;
        private int _commentsOnProfileCount;
        private MvxObservableCollection<BaseProfileCommentViewModel> _commentOnProfile;
        private MvxObservableCollection<BasePostViewModel> _posts;
        private MvxObservableCollection<BasePostViewModel> _likes;
        private MvxObservableCollection<BaseUserViewModel> _rommates;
        private DateTimeOffset _birthDate;
        private DateTimeOffset _creationDate;

        public string FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }

        public string LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }

        public decimal AverageGrade
        {
            get => _avrageGrade;
            set => SetProperty(ref _avrageGrade, value);
        }

        public string ProfilePictureUrl
        {
            get => _profilePictureUrl;
            set => SetProperty(ref _profilePictureUrl, value);
        }

        public byte Age
        {
            get => _age;
            set => SetProperty(ref _age, value);
        }

        public DateTimeOffset BirthDate
        {
            get => _birthDate;
            set => SetProperty(ref _birthDate, value);
        }

        public DateTimeOffset CreationDate
        {
            get => _creationDate;
            set => SetProperty(ref _creationDate, value);
        }

        public bool HasFaculty
        {
            get => _hasFaculty;
            set => SetProperty(ref _hasFaculty, value);
        }

        public string DescriptionOfStudyOrWork
        {
            get => _descriptionOfStudyOrWork;
            set => SetProperty(ref _descriptionOfStudyOrWork, value);
        }

        public bool IsSmoker
        {
            get => _isSmoker;
            set => SetProperty(ref _isSmoker, value);
        }

        public Gender Gender
        {
            get => _gender;
            set => SetProperty(ref _gender, value);
        }

        public int CommentsOnProfileCount
        {
            get => _commentsOnProfileCount;
            set => SetProperty(ref _commentsOnProfileCount, value);
        }

        public MvxObservableCollection<BaseProfileCommentViewModel> CommentsOnProfile
        {
            get => _commentOnProfile;
            set => SetProperty(ref _commentOnProfile, value);
        }

        public MvxObservableCollection<BasePostViewModel> Posts
        {
            get => _posts;
            set => SetProperty(ref _posts, value);
        }

        public MvxObservableCollection<BasePostViewModel> Likes
        {
            get => _likes;
            set => SetProperty(ref _likes, value);
        }

        public MvxObservableCollection<BaseUserViewModel> Roommates
        {
            get => _rommates;
            set => SetProperty(ref _rommates, value);
        }

        public BaseUserViewModel(User user) : base(user.Id)
        {
            Mapper.Map(user, this);
        }

        public BaseUserViewModel()
        {
            CommentsOnProfile = new MvxObservableCollection<BaseProfileCommentViewModel>();
            Posts = new MvxObservableCollection<BasePostViewModel>();
            Likes =  new MvxObservableCollection<BasePostViewModel>();
        }
    }
}
