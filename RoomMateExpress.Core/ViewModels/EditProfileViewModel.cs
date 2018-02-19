using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using MvvmValidation;
using RoomMateExpress.Core.Extensions;
using RoomMateExpress.Core.Helpers;
using RoomMateExpress.Core.Helpers.Collections;
using RoomMateExpress.Core.Helpers.Enums;
using RoomMateExpress.Core.Helpers.MvxMessengerHelpers;
using RoomMateExpress.Core.Services;
using RoomMateExpress.Core.Settings;
using RoomMateExpress.Core.ViewModels.BaseViewModels;

namespace RoomMateExpress.Core.ViewModels
{
    public class EditProfileViewModel : MvxViewModel<LoginViewModel>
    {
        #region Private fields

        private readonly IMvxNavigationService _navigationService;
        private readonly IUserService _userService;
        private BaseUserViewModel _user;
        private bool _exitToMain;
        private readonly IToastSerivce _toastSerivce;
        private readonly IMvxMessenger _messenger;
        private MvxSubscriptionToken _subscriptionToken;
        private readonly IPictureService _pictureService;
        private readonly ILocalizationService _localizationService;
        private ObservableDictionary<string, string> _errors;
        private bool _isBusy;

        #endregion

        public EditProfileViewModel(IMvxNavigationService navigationService, IUserService userService, IToastSerivce toastSerivce, IMvxMessenger messenger, IPictureService pictureService, ILocalizationService localizationService)
        {
            _navigationService = navigationService;
            _userService = userService;
            _toastSerivce = toastSerivce;
            _messenger = messenger;
            _pictureService = pictureService;
            _localizationService = localizationService;
        }

        #region Overrided methods

        public override void Prepare()
        {
            base.Prepare();
            User = Mapper.Map<BaseUserViewModel>(ApplicationData.CurrentUserViewModel);
        }

        public override void Prepare(LoginViewModel parameter)
        {
            if (parameter != null)
            {
                _exitToMain = true;
                _navigationService.Close(parameter);
                User = new BaseUserViewModel
                {
                    BirthDate = default(DateTimeOffset)
                };
            }
        }

        #endregion

        #region Properties

        public ObservableDictionary<string, string> Errors
        {
            get => _errors;
            set => SetProperty(ref _errors, value);
        }

        public BaseUserViewModel User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        public bool IsMale
        {
            get => User.Gender == Gender.Male;
            set
            {
                if (value)
                {
                    User.Gender = Gender.Male;
                }
                RaisePropertyChanged(nameof(IsMale));
            }
        }

        public bool IsFemale
        {
            get => User.Gender == Gender.Female;
            set
            {
                if (value)
                {
                    User.Gender = Gender.Female;
                }
                RaisePropertyChanged(nameof(IsFemale));
            }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public List<ITransformation> Transformations => new List<ITransformation>
        {
            new CircleTransformation()
        };

        #endregion

        #region Commands

        public IMvxAsyncCommand ChangeDateCommand => new MvxAsyncCommand(ChangeDate);

        public IMvxAsyncCommand SaveChangesCommand => new MvxAsyncCommand(SaveChanges);

        public IMvxAsyncCommand ChangeProfilePictureCommand => new MvxAsyncCommand(ChangeProfilePicture);

        #endregion

        #region Methods

        private async Task ChangeDate()
        {
            var result = await _navigationService.Navigate<DatePickerViewModel, DatePickerOptions, DateTime>(
                new DatePickerOptions
                {
                    MinDate = DateTimeOffset.Now.Subtract(new TimeSpan(365 * 100, 0, 0, 0)).LocalDateTime,
                    MaxDate = DateTimeOffset.Now.Subtract(new TimeSpan(365 * 14, 0, 0, 0)).LocalDateTime,
                    SelectedDate = User.BirthDate == default(DateTimeOffset)
                        ? DateTimeOffset.Now.Subtract(new TimeSpan(365 * 14, 0, 0, 0)).LocalDateTime
                        : User.BirthDate.LocalDateTime
                });
            User.BirthDate = result == default(DateTime) ? User.BirthDate : result;
        }

        private async Task SaveChanges()
        {
            if (!Validate())
                return;
            IsBusy = true;
            await ApiRequestHelper.HandleApiResult(() => _userService.CreateOrUpdateUser(User), async model =>
            {
                ApplicationData.CurrentUserViewModel = Mapper.Map<BaseUserViewModel>(model);
                if (_exitToMain)
                {
                    await _navigationService.Navigate<MainViewModel>();
                    await _navigationService.Close(this);
                    return;
                }
                await _navigationService.Close(this);
            });
            IsBusy = false;
        }

        private async Task ChangeProfilePicture()
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
                if (!string.IsNullOrWhiteSpace(User.ProfilePictureUrl))
                    await ApiRequestHelper.HandleApiResult(() =>
                        _pictureService.DeletePicture(
                            User.ProfilePictureUrl.Substring(User.ProfilePictureUrl.LastIndexOf("/") + 1)));
                User.ProfilePictureUrl = pictureMessage.PictureUrl;
            }
            else if (!pictureMessage.Error.Equals(Constants.Errors.OperationCanceled))
            {
                if (pictureMessage.Error.Equals(Constants.Errors.LoginRequired))
                    await RequestLoginHelper.RequestLogin(ChangeProfilePicture);
                else
                    _toastSerivce.ShowByValue(pictureMessage.Error);
            }
            _subscriptionToken.Dispose();
            _subscriptionToken = null;
        }

        private bool Validate()
        {
            var validator = new ValidationHelper();
            validator.AddRequiredRule(() => User.FirstName, _localizationService.GetResourceString("requiredField"));
            validator.AddRequiredRule(() => User.LastName, _localizationService.GetResourceString("requiredField"));
            validator.AddRequiredRule(() => User.DescriptionOfStudyOrWork, _localizationService.GetResourceString("requiredField"));
            validator.AddRequiredRule(() => User.ProfilePictureUrl, _localizationService.GetResourceString("requiredImage"));
            validator.AddRule(nameof(User.BirthDate), () => RuleResult.Assert(User.BirthDate < DateTimeOffset.Now, _localizationService.GetResourceString("dateInvalid")));

            var result = validator.ValidateAll();

            Errors = result.AsObservableDictionary();

            return result.IsValid;
        }

        #endregion
    }
}
