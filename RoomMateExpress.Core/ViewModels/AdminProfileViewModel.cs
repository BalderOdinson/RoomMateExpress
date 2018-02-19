using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using RoomMateExpress.Core.Helpers;
using RoomMateExpress.Core.Services;
using RoomMateExpress.Core.ViewModels.BaseViewModels;

namespace RoomMateExpress.Core.ViewModels
{
    public class AdminProfileViewModel : MvxViewModel<BaseUserViewModel>
    {
        private BaseUserViewModel _user;
        private readonly IMvxNavigationService _navigationService;
        private readonly IUserService _userService;
        private bool _isBusy;

        public AdminProfileViewModel(IMvxNavigationService navigationService, IUserService userService)
        {
            _navigationService = navigationService;
            _userService = userService;
        }

        #region Overrided methods

        public override void Prepare(BaseUserViewModel parameter)
        {
            User = parameter;
        }

        public override async Task Initialize()
        {
            await base.Initialize();
            await LoadUser(User.Id);
        }

        #endregion

        #region Properties

        public BaseUserViewModel User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        #endregion

        #region Commands

        public IMvxAsyncCommand CloseCommand => new MvxAsyncCommand(async () => await Close());

        public IMvxAsyncCommand ViewPostsCommand => new MvxAsyncCommand(ViewPosts);

        public IMvxAsyncCommand ViewCommentsCommand => new MvxAsyncCommand(ViewComments);

        public IMvxAsyncCommand BanUserDialogCommand => new MvxAsyncCommand(BanUserDialog);

        #endregion

        #region Methods

        private async Task Close()
        {
            await _navigationService.Close(this);
        }

        private async Task ViewPosts()
        {
            await _navigationService.Navigate<AdminUserPostsViewModel, BaseUserViewModel>(User);
        }

        private async Task ViewComments()
        {
            await _navigationService.Navigate<UserCommentsViewModel, BaseUserViewModel>(User);
        }

        private async Task BanUserDialog()
        {
            await _navigationService.Navigate<BanUserViewModel, BaseUserViewModel>(User);
        }

        private async Task LoadUser(Guid id)
        {
            IsBusy = true;
            await ApiRequestHelper.HandleApiResult(() => _userService.GetUser(id), model => Mapper.Map(model, User));
            IsBusy = false;
        }

        #endregion




    }
}
