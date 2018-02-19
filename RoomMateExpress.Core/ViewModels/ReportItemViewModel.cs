using System;
using System.Collections.Generic;
using System.Text;
using RoomMateExpress.Core.ViewModels.BaseViewModels;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Navigation;
using System.Threading.Tasks;

namespace RoomMateExpress.Core.ViewModels
{
    public class ReportItemViewModel : MvxViewModel
    {
        #region Private fields

        private readonly IMvxNavigationService _navigationService;
        private BaseUserReportViewModel _report;

        #endregion

        public ReportItemViewModel(IMvxNavigationService navigationService, BaseUserReportViewModel report)
        {
            _navigationService = navigationService;
            Report = report;
        }

        #region Properties

        public BaseUserReportViewModel Report
        {
            get => _report;
            set => SetProperty(ref _report, value);
        }

        #endregion

        #region Commands

        public IMvxAsyncCommand OpenReportCommand => new MvxAsyncCommand(OpenReport);

        #endregion

        #region Methods

        private async Task OpenReport()
        {
            await _navigationService.Navigate<ReportViewModel, BaseUserReportViewModel>(Report);
        }

        #endregion

    }
}
