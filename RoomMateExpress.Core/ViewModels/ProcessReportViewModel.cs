using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using RoomMateExpress.Core.Helpers;
using RoomMateExpress.Core.Services;
using RoomMateExpress.Core.ViewModels.BaseViewModels;

namespace RoomMateExpress.Core.ViewModels
{
    public class ProcessReportViewModel : MvxViewModel<BaseUserReportViewModel>
    {
        #region Private fields

        private BaseUserReportViewModel _report;
        private readonly IToastSerivce _toastSerivce;
        private readonly IMvxNavigationService _navigationService;
        private readonly IUserReportService _reportService;
        private bool _isBusy;

        #endregion

        public ProcessReportViewModel(IUserReportService reportService, IMvxNavigationService navigationService, IToastSerivce toastSerivce)
        {
            _reportService = reportService;
            _navigationService = navigationService;
            _toastSerivce = toastSerivce;
        }

        #region Overrided methods

        public override void Prepare(BaseUserReportViewModel parameter)
        {
            Report = parameter;
        }

        #endregion

        #region Properties

        public BaseUserReportViewModel Report
        {
            get => _report;
            set => SetProperty(ref _report, value);
        }

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        #endregion

        #region Commands

        public IMvxAsyncCommand BanUserCommand => new MvxAsyncCommand(BanUser);

        public IMvxAsyncCommand IgnoreCommand => new MvxAsyncCommand(Ignore);

        #endregion

        #region Methods

        private async Task BanUser()
        {
            IsBusy = true;
            if (await _navigationService.Navigate<BanUserViewModel, BaseUserViewModel, bool>(Report.UserReported))
            {
                await ApiRequestHelper.HandleApiResult(() => _reportService.ProcessReport(Report), async model =>
                {
                    await _navigationService.Close(this);
                    _toastSerivce.ShowByResourceId("reportProcessed");
                });
            }

            IsBusy = false;
        }

        private async Task Ignore()
        {
            IsBusy = true;
            await ApiRequestHelper.HandleApiResult(() => _reportService.ProcessReport(Report), async model =>
            {
                await _navigationService.Close(this);
                _toastSerivce.ShowByResourceId("reportProcessed");
            });
            IsBusy = false;
        }

        #endregion
    }
}
