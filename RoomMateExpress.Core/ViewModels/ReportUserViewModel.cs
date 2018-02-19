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
    public class ReportUserViewModel : MvxViewModel<BaseUserViewModel>
    {
        #region Private fields

        private readonly IMvxNavigationService _navigationService;
        private readonly IUserReportService _reportService;
        private BaseUserReportViewModel _userReport;
        private BaseUserViewModel _user;
        private bool _isBusy;

        #endregion

        public ReportUserViewModel(IMvxNavigationService navigationService, IUserReportService reportService)
        {
            _navigationService = navigationService;
            _reportService = reportService;
            _userReport = new BaseUserReportViewModel
            {
                UserReporting = Settings.ApplicationData.CurrentUserViewModel
            };
        }

        #region Overrided methods

        public override void Prepare(BaseUserViewModel parameter)
        {
            _user = parameter;
        }

        #endregion

        #region Properties

        public BaseUserReportViewModel UserReport
        {
            get => _userReport;
            set => SetProperty(ref _userReport, value);
        }

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        #endregion

        #region Commands

        public IMvxAsyncCommand ReportUserCommand => new MvxAsyncCommand(async () => await ReportUser());

        #endregion

        #region Methods

        private async Task ReportUser()
        {
            _userReport.UserReported = _user;
            IsBusy = true;
            await ApiRequestHelper.HandleApiResult(() => _reportService.ReportUser(UserReport),
                async () => await _navigationService.Close(this));
            IsBusy = false;
        }

        #endregion
    }
}
