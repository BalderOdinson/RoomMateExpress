using System;
using System.Collections.Generic;
using System.Text;
using MvvmCross.Core.ViewModels;
using RoomMateExpress.Core.ViewModels.BaseViewModels;
using MvvmCross.Core.Navigation;
using System.Threading.Tasks;
using System.Linq;
using AutoMapper;
using Newtonsoft.Json;
using RoomMateExpress.Core.Extensions;
using RoomMateExpress.Core.Helpers;
using RoomMateExpress.Core.Helpers.Enums;
using RoomMateExpress.Core.Helpers.SortFilterHelpers;
using RoomMateExpress.Core.Services;
using RoomMateExpress.Core.Settings;

namespace RoomMateExpress.Core.ViewModels
{
    public class AdminPostsViewModel : MvxViewModel
    {
        #region Private fields

        private string _searchText;
        private FilterViewModel _filterViewModel;
        private MvxObservableCollection<AdminPostItemViewModel> _postItemViewModels;
        private readonly IMvxNavigationService _navigationService;
        private readonly IPostService _postService;
        private readonly IToastSerivce _toastSerivce;
        private bool _isRefreshing;
        private bool _isLoading;
        private bool _areAllElementsLoaded;

        #endregion

        public AdminPostsViewModel(IMvxNavigationService navigationService, IPostService postService, IToastSerivce toastSerivce)
        {
            _navigationService = navigationService;
            _postService = postService;
            _toastSerivce = toastSerivce;
            _filterViewModel = new FilterViewModel(new FilterManager(), new SortManager());
            this.SubscribeSearchText(nameof(SearchText), SerachCommand, () => !IsLoading && !IsRefreshing);
            AdminPostItemViewModels = new MvxObservableCollection<AdminPostItemViewModel>();
        }


        #region Overrided methods

        public override async Task Initialize()
        {
            await base.Initialize();
            if (AdminPostItemViewModels == null || !AdminPostItemViewModels.Any())
            {
                await Reload();
            }
        }

        protected override void SaveStateToBundle(IMvxBundle bundle)
        {
            base.SaveStateToBundle(bundle);
            var filter = JsonConvert.SerializeObject(_filterViewModel);
            var areAllElementsLoaded = JsonConvert.SerializeObject(AreAllElementsLoaded);
            bundle.Data.Add("FilterManager", filter);
            bundle.Data.Add("AreAllElementsLoaded", areAllElementsLoaded);
        }

        protected override void ReloadFromBundle(IMvxBundle state)
        {
            base.ReloadFromBundle(state);
            _filterViewModel = JsonConvert.DeserializeObject<FilterViewModel>(state.Data["FilterManager"]);
            AreAllElementsLoaded = JsonConvert.DeserializeObject<bool>(state.Data["AreAllElementsLoaded"]);
        }

        #endregion

        #region Properties

        public MvxObservableCollection<AdminPostItemViewModel> AdminPostItemViewModels
        {
            get => _postItemViewModels;
            set => SetProperty(ref _postItemViewModels, value);
        }

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        public bool AreAllElementsLoaded
        {
            get => _areAllElementsLoaded;
            set => SetProperty(ref _areAllElementsLoaded, value);
        }

        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        #endregion

        #region Commands

        public IMvxAsyncCommand ReloadCommand => new MvxAsyncCommand(Reload);

        public IMvxAsyncCommand<AdminPostItemViewModel> RemovePostCommand => new MvxAsyncCommand<AdminPostItemViewModel>(RemovePost);

        public IMvxAsyncCommand OpenFilterDialogCommand => new MvxAsyncCommand(OpenFilterDialog);

        public IMvxAsyncCommand SerachCommand => new MvxAsyncCommand(Search);

        public IMvxAsyncCommand LoadMoreElementsCommand => new MvxAsyncCommand(LoadMoreElements);

        #endregion

        #region Methods

        private async Task Reload()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                IsRefreshing = true;
                await ApiRequestHelper.HandleApiResult(
                    () => _postService.GetAllPosts(DateTimeOffset.Now, null,
                        Constants.Pagination.InitialCount),
                    models =>
                    {
                        AdminPostItemViewModels = new MvxObservableCollection<AdminPostItemViewModel>(models.Select(m =>
                            new AdminPostItemViewModel(_navigationService, m, RemovePostCommand, _postService,
                                _toastSerivce)));
                        _filterViewModel = new FilterViewModel(new FilterManager(), new SortManager());
                    });
                AreAllElementsLoaded = AdminPostItemViewModels.Count < Constants.Pagination.InitialCount;
                IsRefreshing = false;
            }
            else
                SearchText = null;
        }

        private async Task RemovePost(AdminPostItemViewModel arg)
        {
            await ApiRequestHelper.HandleApiResult(() => _postService.DeletePost(arg.Post.Id),
                () => AdminPostItemViewModels.Remove(arg));
        }

        private async Task OpenFilterDialog()
        {
            var result = await _navigationService.Navigate<PostsFilterViewModel, FilterViewModel, FilterViewModel>(_filterViewModel);
            if (result != null)
            {
                _filterViewModel = result;
                IsLoading = true;
                AdminPostItemViewModels.Clear();
                await ApiRequestHelper.HandleApiResult(() => _postService.GetAllPosts(
                    _filterViewModel.SortManager.OrderOption == SortOrderOption.Descending ? DateTimeOffset.Now : DateTimeOffset.MinValue,
                    _filterViewModel.SortManager.SortOptions.GetDefaultModifier(_filterViewModel.SortManager.OrderOption),
                    Constants.Pagination.InitialCount,
                    _filterViewModel.SortManager.SortOptions,
                    _filterViewModel.SortManager.OrderOption,
                    _filterViewModel.FilterManager.ByAccomodation
                        ? (AccomodationOptions?)_filterViewModel.FilterManager.AccomodationOptions
                        : null,
                    _filterViewModel.FilterManager.ByPrice ? (decimal?)_filterViewModel.FilterManager.MinPrice : null,
                    _filterViewModel.FilterManager.ByPrice ? (decimal?)_filterViewModel.FilterManager.MaxPrice : null,
                    _filterViewModel.FilterManager.ByCity ? _filterViewModel.FilterManager.City.Name : null,
                    SearchText), models =>
                {
                    var basePostCommentViewModels = models as BasePostViewModel[] ?? models.ToArray();
                    AdminPostItemViewModels = new MvxObservableCollection<AdminPostItemViewModel>(basePostCommentViewModels.Select(m => new AdminPostItemViewModel(_navigationService, m, RemovePostCommand, _postService, _toastSerivce)));
                    AreAllElementsLoaded = basePostCommentViewModels.Length < Constants.Pagination.InitialCount;
                });
                IsLoading = false;
            }
        }

        private async Task Search()
        {
            IsRefreshing = true;
            AdminPostItemViewModels.Clear();
            await ApiRequestHelper.HandleApiResult(() => _postService.GetAllPosts(
                _filterViewModel.SortManager.OrderOption == SortOrderOption.Descending ? DateTimeOffset.Now : DateTimeOffset.MinValue,
                _filterViewModel.SortManager.SortOptions.GetDefaultModifier(_filterViewModel.SortManager.OrderOption),
                Constants.Pagination.InitialCount,
                _filterViewModel.SortManager.SortOptions,
                _filterViewModel.SortManager.OrderOption,
                _filterViewModel.FilterManager.ByAccomodation
                    ? (AccomodationOptions?)_filterViewModel.FilterManager.AccomodationOptions
                    : null,
                _filterViewModel.FilterManager.ByPrice ? (decimal?)_filterViewModel.FilterManager.MinPrice : null,
                _filterViewModel.FilterManager.ByPrice ? (decimal?)_filterViewModel.FilterManager.MaxPrice : null,
                _filterViewModel.FilterManager.ByCity ? _filterViewModel.FilterManager.City.Name : null,
                SearchText), models =>
            {
                var basePostViewModels = models as BasePostViewModel[] ?? models.ToArray();
                AdminPostItemViewModels = new MvxObservableCollection<AdminPostItemViewModel>(basePostViewModels.Select(m =>
                    new AdminPostItemViewModel(_navigationService, m, RemovePostCommand, _postService,
                        _toastSerivce)));
                AreAllElementsLoaded = basePostViewModels.Length < Constants.Pagination.InitialCount;
            });
            IsRefreshing = false;
        }

        private async Task LoadMoreElements()
        {
            IsLoading = true;
            await ApiRequestHelper.HandleApiResult(() => _postService.GetAllPosts(
                AdminPostItemViewModels.LastOrDefault().Post.PostDate,
                AdminPostItemViewModels.LastOrDefault().Post.GetSortingObject(_filterViewModel.SortManager.SortOptions),
                Constants.Pagination.RequestMoreCount,
                _filterViewModel.SortManager.SortOptions,
                _filterViewModel.SortManager.OrderOption,
                _filterViewModel.FilterManager.ByAccomodation
                    ? (AccomodationOptions?)_filterViewModel.FilterManager.AccomodationOptions
                    : null,
                _filterViewModel.FilterManager.ByPrice ? (decimal?)_filterViewModel.FilterManager.MinPrice : null,
                _filterViewModel.FilterManager.ByPrice ? (decimal?)_filterViewModel.FilterManager.MaxPrice : null,
                _filterViewModel.FilterManager.ByCity ? _filterViewModel.FilterManager.City.Name : null,
                SearchText), models =>
            {
                var basePostViewModels = models as BasePostViewModel[] ?? models.ToArray();
                AdminPostItemViewModels.AddRange(basePostViewModels.Select(m => new AdminPostItemViewModel(_navigationService, m, RemovePostCommand, _postService, _toastSerivce)));
                AreAllElementsLoaded = basePostViewModels.Length < Constants.Pagination.RequestMoreCount;
            });
            IsLoading = false;
        }

        #endregion
    }
}
