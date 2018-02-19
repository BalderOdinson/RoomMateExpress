using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using RoomMateExpress.Core.Helpers;
using RoomMateExpress.Core.Helpers.Enums;
using RoomMateExpress.Core.Mocks;
using RoomMateExpress.Core.Models;

namespace RoomMateExpress.Core.ViewModels.BaseViewModels
{
    public class BasePostViewModel : BaseViewModel
    {
        private string _title;
        private string _description;
        private AccomodationOptions _accomodationOption;
        private PetOptions _petOptions;
        private byte _numberOfRoommates;
        private Gender _wantedGender;
        private bool _isSmokingAllowed;
        private AccomodationType _accomodationType;
        private bool _arePetsAllowed;
        private decimal _price;
        private BaseUserViewModel _user;
        private MvxObservableCollection<BasePostPictureViewModel> _postPictures;
        private MvxObservableCollection<BaseUserViewModel> _likes;
        private MvxObservableCollection<BasePostCommentViewModel> _commentsForPosts;
        private MvxObservableCollection<BaseNeighborhoodViewModel> _neighborhoods;
        private int _likesCount;
        private int _commentsCount;
        private bool _isLiked;
        private BaseCityViewModel _city;
        private DateTimeOffset _postDate;

        public BaseCityViewModel City
        {
            get => _city;
            set => SetProperty(ref _city, value);
        }

        public BaseUserViewModel User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        public PetOptions PetOptions
        {
            get => _petOptions;
            set => SetProperty(ref _petOptions, value);
        }

        public AccomodationOptions AccomodationOption
        {
            get => _accomodationOption;
            set => SetProperty(ref _accomodationOption, value);
        }

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public bool IsLiked
        {
            get => _isLiked;
            set => SetProperty(ref _isLiked, value);
        }

        public byte NumberOfRoommates
        {
            get => _numberOfRoommates;
            set => SetProperty(ref _numberOfRoommates, value);
        }

        public Gender WantedGender
        {
            get => _wantedGender;
            set => SetProperty(ref _wantedGender, value);
        }

        public bool IsSmokingAllowed
        {
            get => _isSmokingAllowed;
            set => SetProperty(ref _isSmokingAllowed, value);
        }

        public bool HasAccomodation => AccomodationOption == AccomodationOptions.With;

        public AccomodationType AccomodationType
        {
            get => _accomodationType;
            set => SetProperty(ref _accomodationType, value);
        }

        public bool ArePetsAllowed
        {
            get => _arePetsAllowed;
            set => SetProperty(ref _arePetsAllowed, value);
        }

        public decimal Price
        {
            get => _price;
            set => SetProperty(ref _price, value);
        }

        public int LikesCount
        {
            get => _likesCount;
            set => SetProperty(ref _likesCount, value);
        }

        public int CommentsCount
        {
            get => _commentsCount;
            set => SetProperty(ref _commentsCount, value);
        }

        public DateTimeOffset PostDate
        {
            get => _postDate;
            set => SetProperty(ref _postDate, value);
        }

        public MvxObservableCollection<BasePostPictureViewModel> PostPictures
        {
            get => _postPictures;
            set => SetProperty(ref _postPictures, value);
        }

        public MvxObservableCollection<BaseUserViewModel> Likes
        {
            get => _likes;
            set => SetProperty(ref _likes, value);
        }

        public MvxObservableCollection<BasePostCommentViewModel> Comments
        {
            get => _commentsForPosts;
            set => SetProperty(ref _commentsForPosts, value);
        }

        public MvxObservableCollection<BaseNeighborhoodViewModel> Neighborhoods
        {
            get => _neighborhoods;
            set => SetProperty(ref _neighborhoods, value);
        }

        public BasePostViewModel(Post post) : base(post.Id)
        {
            Mapper.Map(post, this);
        }

        public BasePostViewModel()
        {
            PostPictures = new MvxObservableCollection<BasePostPictureViewModel>();
            Likes = new MvxObservableCollection<BaseUserViewModel>();
            Neighborhoods = new MvxObservableCollection<BaseNeighborhoodViewModel>();
            Comments = new MvxObservableCollection<BasePostCommentViewModel>();
        }
    }
}
