using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using RoomMateExpress.Core.Extensions;
using RoomMateExpress.Core.Helpers;
using RoomMateExpress.Core.ViewModels.BaseViewModels;
using RoomMateExpress.Core.Mocks;
using RoomMateExpress.Core.Services;

namespace RoomMateExpress.Core.ViewModels
{
    public class AdminReportsViewModel : MvxViewModel
    {
        #region Private fields

        private MvxObservableCollection<ReportItemViewModel> _reportItemViewModels;
        private readonly IMvxNavigationService _navigationService;
        private bool _isRefreshing;
        private readonly IUserReportService _userReportService;
        private bool _isLoading;
        private bool _areAllElementsLoaded;

        #endregion

        public AdminReportsViewModel(IMvxNavigationService navigationService, IUserReportService userReportService)
        {
            _navigationService = navigationService;
            _userReportService = userReportService;
            ReportItemViewModels = new MvxObservableCollection<ReportItemViewModel>();
        }

        #region Overrided methods

        public override async Task Initialize()
        {
            await base.Initialize();
            await Reload();
        }

        #endregion

        #region Properties

        public MvxObservableCollection<ReportItemViewModel> ReportItemViewModels
        {
            get => _reportItemViewModels;
            set => SetProperty(ref _reportItemViewModels, value);
        }

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public bool AreAllElementsLoaded
        {
            get => _areAllElementsLoaded;
            set => SetProperty(ref _areAllElementsLoaded, value);
        }

        #endregion

        #region Commands

        public IMvxAsyncCommand ReloadCommand => new MvxAsyncCommand(Reload);

        public IMvxAsyncCommand LoadMoreElementsCommand => new MvxAsyncCommand(LoadMoreElements);

        #endregion

        #region Methods

        private async Task Reload()
        {
            IsRefreshing = true;
            await ApiRequestHelper.HandleApiResult(
                () => _userReportService.GetAllUserReports(Constants.Pagination.InitialCount, DateTimeOffset.Now),
                models =>
                {
                    ReportItemViewModels.SwitchTo(models.Select(m =>
                        new ReportItemViewModel(_navigationService, m)));
                });
            AreAllElementsLoaded = ReportItemViewModels.Count < Constants.Pagination.InitialCount;
            IsRefreshing = false;
        }

        private async Task LoadMoreElements()
        {
            IsLoading = true;
            await ApiRequestHelper.HandleApiResult(
                () => _userReportService.GetAllUserReports(Constants.Pagination.RequestMoreCount, ReportItemViewModels.Last().Report.DateReporting),
                models =>
                {
                    var baseUserReportViewModels = models as BaseUserReportViewModel[] ?? models.ToArray();
                    ReportItemViewModels.SwitchTo(baseUserReportViewModels.Select(m =>
                        new ReportItemViewModel(_navigationService, m)));
                    AreAllElementsLoaded = baseUserReportViewModels.Length < Constants.Pagination.InitialCount;
                });
            IsLoading = false;
        }

        #endregion
    }
}
