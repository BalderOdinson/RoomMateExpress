using System.Threading.Tasks;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using RoomMateExpress.Core.Helpers;
using RoomMateExpress.Core.Services;
using RoomMateExpress.Core.ViewModels.BaseViewModels;

namespace RoomMateExpress.Core.ViewModels
{
    public class AdminEditinfoViewModel : MvxViewModel<LoginViewModel>
    {
        #region Private fields

        private BaseAdminViewModel _admin;
        private readonly IMvxNavigationService _navigationService;
        private readonly IAdminService _adminService;
        private readonly IToastSerivce _toastSerivce;
        private bool _isBusy;

        #endregion

        public AdminEditinfoViewModel(IMvxNavigationService navigationService, IAdminService adminService, IToastSerivce toastSerivce)
        {
            _navigationService = navigationService;
            _adminService = adminService;
            _toastSerivce = toastSerivce;
        }

        #region Overrided methods

        public override void Prepare(LoginViewModel parameter)
        {
            _navigationService.Close(parameter);
            Admin = new BaseAdminViewModel();
        }

        #endregion

        #region Properties

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public BaseAdminViewModel Admin
        {
            get => _admin;
            set => SetProperty(ref _admin, value);
        }

        #endregion

        #region Commands

        public IMvxAsyncCommand SetInfoCommand => new MvxAsyncCommand(SetInfo);

        #endregion

        #region Methods

        private async Task SetInfo()
        {
            IsBusy = true;
            await ApiRequestHelper.HandleApiResult(async () => await _adminService.CreateAdmin(Admin), async model =>
             {
                 Settings.ApplicationData.CurrentAdminViewModel = model;
                 await _navigationService.Navigate<AdminMainViewModel>();
                 await _navigationService.Close(this);
             });
            IsBusy = false;
        }

        #endregion
    }
}
