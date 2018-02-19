using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Navigation;
using RoomMateExpress.Core.ViewModels.BaseViewModels;

namespace RoomMateExpress.Core.ViewModels
{
    public class ReportViewModel : MvxViewModel<BaseUserReportViewModel>
    {
        #region Private fields

        private readonly IMvxNavigationService _navigationService;
        private BaseUserReportViewModel _report;

        #endregion

        public ReportViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
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

        #endregion

        #region Commands

        public IMvxAsyncCommand CloseCommand => new MvxAsyncCommand(Close);

        public IMvxAsyncCommand IgnoreCommand => new MvxAsyncCommand(Ignore);

        public IMvxAsyncCommand ProcessReportCommand => new MvxAsyncCommand(ProcessReport);

        #endregion

        #region Methods

        private async Task Close()
        {
            await _navigationService.Close(this);
        }

        private async Task Ignore()
        {
            await _navigationService.Close(this);
        }

        private async Task ProcessReport()
        {
            await _navigationService.Navigate<ProcessReportViewModel, BaseUserReportViewModel>(Report);
        }

        #endregion
    }
}
