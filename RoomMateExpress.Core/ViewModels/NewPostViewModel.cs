using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Navigation;
using System.Threading.Tasks;
using AutoMapper;
using MvvmCross.Plugins.Messenger;
using MvvmValidation;
using RoomMateExpress.Core.Extensions;
using RoomMateExpress.Core.Helpers;
using RoomMateExpress.Core.Helpers.Collections;
using RoomMateExpress.Core.Helpers.Enums;
using RoomMateExpress.Core.Helpers.MvxMessengerHelpers;
using RoomMateExpress.Core.Services;
using RoomMateExpress.Core.ViewModels.BaseViewModels;

namespace RoomMateExpress.Core.ViewModels
{
    public class NewPostViewModel : MvxViewModel<BasePostViewModel>
    {
        #region Private fields

        private readonly IMvxNavigationService _navigationService;
        private readonly IPostService _postService;
        private readonly ICityService _cityService;
        private readonly IToastSerivce _toastSerivce;
        private readonly ILocalizationService _localizationService;
        private readonly IPictureService _pictureService;
        private readonly IMvxMessenger _messenger;
        private MvxSubscriptionToken _subscriptionToken;
        private BaseCityViewModel _selectedCity;
        private BasePostViewModel _post;
        private MvxObservableCollection<BaseCityViewModel> _cities;
        private MvxObservableCollection<PostPictureItemViewModel> _postPictureItems;
        private MvxObservableCollection<NeighboorhoodItemViewModel> _neighborhoodItems;
        private ObservableDictionary<string, string> _errors;
        private BasePostViewModel _originalPost;
        private bool _isPosted = false;
        private bool _isBusy;

        #endregion

        public NewPostViewModel(IMvxNavigationService navigationService, IPostService postService,
            ICityService cityService, IToastSerivce toastSerivce, ILocalizationService localizationService,
            IMvxMessenger messenger, IPictureService pictureService)
        {
            _navigationService = navigationService;
            _postService = postService;
            _cityService = cityService;
            _toastSerivce = toastSerivce;
            _localizationService = localizationService;
            _messenger = messenger;
            _pictureService = pictureService;
            PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "SelectedCity" && SelectedCity != null)
                {
                    NeighborhoodItems = new MvxObservableCollection<NeighboorhoodItemViewModel>(
                        SelectedCity.Neighborhoods.Select(p => new NeighboorhoodItemViewModel(p, Post)));
                }
            };
        }

        #region Overrided methods

        public override async Task Initialize()
        {
            await base.Initialize();
            await ApiRequestHelper.HandleApiResult(() => _cityService.GetAllCities(),
                models =>
                {
                    Cities = new MvxObservableCollection<BaseCityViewModel>(models);
                    if (SelectedCity != null)
                        NeighborhoodItems =
                            new MvxObservableCollection<NeighboorhoodItemViewModel>(
                                SelectedCity.Neighborhoods.Select(n => new NeighboorhoodItemViewModel(n, Post)));
                    else
                        NeighborhoodItems = new MvxObservableCollection<NeighboorhoodItemViewModel>();
                    PostPictureItems = new MvxObservableCollection<PostPictureItemViewModel>();
                });
        }

        public override async void ViewDestroy(bool viewFinishing = true)
        {
            base.ViewDestroy(viewFinishing);
            if (_isPosted || !viewFinishing) return;
            foreach (var pictureViewModel in _post.PostPictures.Except(_originalPost.PostPictures))
            {
                await ApiRequestHelper.HandleApiResult(() =>
                    _pictureService.DeletePicture(
                        pictureViewModel.PictureUrl.Substring(pictureViewModel.PictureUrl.LastIndexOf("/") + 1)));
            }
        }

        public override void Prepare(BasePostViewModel parameter)
        {
            _originalPost = parameter;
            Post = Mapper.Map<BasePostViewModel>(parameter);
        }

        #endregion

        #region Properties

        public MvxObservableCollection<PostPictureItemViewModel> PostPictureItems
        {
            get => _postPictureItems;
            set => SetProperty(ref _postPictureItems, value);
        }

        public BaseCityViewModel SelectedCity
        {
            get => _selectedCity;
            set
            {
                SetProperty(ref _selectedCity, value);
                RaisePropertyChanged(nameof(SelectedCity.Neighborhoods));
            }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public MvxObservableCollection<BaseCityViewModel> Cities
        {
            get => _cities;
            set => SetProperty(ref _cities, value);
        }

        public BasePostViewModel Post
        {
            get => _post;
            set => SetProperty(ref _post, value);
        }

        public bool IsWithAccomodation
        {
            get => Post.AccomodationOption == AccomodationOptions.With;
            set
            {
                if (value)
                {
                    Post.AccomodationOption = AccomodationOptions.With;
                }
                RaisePropertyChanged(nameof(IsWithAccomodation));
                RaisePropertyChanged(nameof(IsWithoutAccomodation));
            }
        }

        public bool IsWithoutAccomodation
        {
            get => Post.AccomodationOption == AccomodationOptions.Without;
            set
            {
                if (value) Post.AccomodationOption = AccomodationOptions.Without;
                RaisePropertyChanged(nameof(IsWithoutAccomodation));
                RaisePropertyChanged(nameof(IsWithAccomodation));
            }
        }

        public bool WithApartment
        {
            get => Post.AccomodationType == AccomodationType.Apartment;
            set
            {
                if (value) Post.AccomodationType = AccomodationType.Apartment;
                RaisePropertyChanged(nameof(WithApartment));
            }
        }

        public bool WithHouse
        {
            get => Post.AccomodationType == AccomodationType.House;
            set
            {
                if (value) Post.AccomodationType = AccomodationType.House;
                RaisePropertyChanged(nameof(WithHouse));
            }
        }

        public bool IsMale
        {
            get => Post.WantedGender == Gender.Male;
            set
            {
                if (value) Post.WantedGender = Gender.Male;
                RaisePropertyChanged(nameof(IsMale));
            }
        }

        public bool IsFemale
        {
            get => Post.WantedGender == Gender.Female;
            set
            {
                if (value) Post.WantedGender = Gender.Female;
                RaisePropertyChanged(nameof(IsFemale));
            }
        }

        public bool IsAny
        {
            get => Post.WantedGender == Gender.Any;
            set
            {
                if (value) Post.WantedGender = Gender.Any;
                RaisePropertyChanged(nameof(IsAny));
            }
        }

        public bool HaveCat
        {
            get => (Post.PetOptions & PetOptions.Cat) == PetOptions.Cat;
            set
            {
                Post.PetOptions = value ? Post.PetOptions |= PetOptions.Cat : Post.PetOptions &= ~PetOptions.Cat;
                RaisePropertyChanged(nameof(HaveCat));
            }
        }

        public bool HaveReptile
        {
            get => (Post.PetOptions & PetOptions.Reptile) == PetOptions.Reptile;
            set
            {
                Post.PetOptions = value ? Post.PetOptions |= PetOptions.Reptile : Post.PetOptions &= ~PetOptions.Reptile;
                RaisePropertyChanged(nameof(HaveReptile));
            }
        }

        public bool HaveOther
        {
            get => (Post.PetOptions & PetOptions.Other) == PetOptions.Other;
            set
            {
                Post.PetOptions = value ? Post.PetOptions |= PetOptions.Other : Post.PetOptions &= ~PetOptions.Other;
                RaisePropertyChanged(nameof(HaveOther));
            }
        }

        public bool HaveBird
        {
            get => (Post.PetOptions & PetOptions.Bird) == PetOptions.Bird;
            set
            {
                Post.PetOptions = value ? Post.PetOptions |= PetOptions.Bird : Post.PetOptions &= ~PetOptions.Bird;
                RaisePropertyChanged(nameof(HaveBird));
            }
        }

        public bool HaveFish
        {
            get => (Post.PetOptions & PetOptions.Fish) == PetOptions.Fish;
            set
            {
                Post.PetOptions = value ? Post.PetOptions |= PetOptions.Fish : Post.PetOptions &= ~PetOptions.Fish;
                RaisePropertyChanged(nameof(HaveFish));
            }
        }

        public bool HaveDog
        {
            get => (Post.PetOptions & PetOptions.Dog) == PetOptions.Dog;
            set
            {
                Post.PetOptions = value ? Post.PetOptions |= PetOptions.Dog : Post.PetOptions &= ~PetOptions.Dog;
                RaisePropertyChanged(nameof(HaveDog));
            }
        }

        public bool HaveSmallAnimals
        {
            get => (Post.PetOptions & PetOptions.SmallAnimal) == PetOptions.SmallAnimal;
            set
            {
                Post.PetOptions = value ? Post.PetOptions |= PetOptions.SmallAnimal : Post.PetOptions &= ~PetOptions.SmallAnimal;
                RaisePropertyChanged(nameof(HaveSmallAnimals));
            }
        }

        public MvxObservableCollection<NeighboorhoodItemViewModel> NeighborhoodItems
        {
            get => _neighborhoodItems;
            set => SetProperty(ref _neighborhoodItems, value);
        }

        public ObservableDictionary<string, string> Errors
        {
            get => _errors;
            set => SetProperty(ref _errors, value);
        }
        #endregion

        #region Commands

        public IMvxAsyncCommand AddPostImageCommand => new MvxAsyncCommand(AddPostImage);

        public IMvxAsyncCommand CreatePostCommand => new MvxAsyncCommand(CreatePost);

        #endregion

        #region Methods

        private async Task AddPostImage()
        {
            var option = await _navigationService.Navigate<AddPictureViewModel, PictureOptions>();
            if (option == PictureOptions.None) return;
            _subscriptionToken = _messenger.Subscribe<PictureMessage>(SetPicture);
            await _pictureService.RequestPicture(1920, 80, option);
        }

        private async void SetPicture(PictureMessage pictureMessage)
        {
            if (pictureMessage.Success)
            {
                var picture = new BasePostPictureViewModel
                {
                    PictureUrl = pictureMessage.PictureUrl
                };
                Post.PostPictures.Add(picture);
                PostPictureItems.Add(new PostPictureItemViewModel(picture));
            }
            else if (!pictureMessage.Error.Equals(Constants.Errors.OperationCanceled))
            {
                if (pictureMessage.Error.Equals(Constants.Errors.LoginRequired))
                    await RequestLoginHelper.RequestLogin(AddPostImage);
                else
                    _toastSerivce.ShowByValue(pictureMessage.Error);
            }
            _subscriptionToken.Dispose();
            _subscriptionToken = null;
        }

        private async Task CreatePost()
        {
            if (!Validate())
                return;
            IsBusy = true;
            if (Post.Id.Equals(Guid.Empty))
                await ApiRequestHelper.HandleApiResult(() => _postService.CreatePost(Post), async model =>
                {
                    _isPosted = true;
                    Mapper.Map(model, _originalPost);
                    await _navigationService.Close(this);
                    _toastSerivce.ShowByResourceId("postCreated");
                });
            else
                await ApiRequestHelper.HandleApiResult(() => _postService.UpdatePost(Post), async model =>
                {
                    _isPosted = true;
                    foreach (var pictureViewModel in _originalPost.PostPictures.Except(model.PostPictures))
                    {
                        await ApiRequestHelper.HandleApiResult(() =>
                            _pictureService.DeletePicture(
                                pictureViewModel.PictureUrl.Substring(pictureViewModel.PictureUrl.LastIndexOf("/") + 1)));
                    }
                    Mapper.Map(model, _originalPost);
                    await _navigationService.Close(this);
                    _toastSerivce.ShowByResourceId("postCreated");
                });
            IsBusy = false;
        }

        private bool Validate()
        {
            var validator = new ValidationHelper();
            validator.AddRequiredRule(() => Post.Title,
                _localizationService.GetResourceString("postTitleRequired"));
            validator.AddRequiredRule(() => Post.Description,
                _localizationService.GetResourceString("postDescriptionRequired"));
            validator.AddRequiredRule(() => Post.NumberOfRoommates,
                _localizationService.GetResourceString("maxRoommatesRequired"));

            var result = validator.ValidateAll();

            Errors = result.AsObservableDictionary();

            return result.IsValid;
        }

        #endregion
    }
}
