using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using Newtonsoft.Json;
using RoomMateExpress.Core.Extensions;
using RoomMateExpress.Core.Helpers;
using RoomMateExpress.Core.Helpers.Enums;
using RoomMateExpress.Core.Helpers.SortFilterHelpers;
using RoomMateExpress.Core.Services;
using RoomMateExpress.Core.Settings;
using RoomMateExpress.Core.ViewModels.BaseViewModels;

namespace RoomMateExpress.Core.ViewModels
{
    public class PostsViewModel : MvxViewModel
    {
        #region Private fields

        private string _searchText;
        private FilterViewModel _filterViewModel;
        private readonly IPostService _postService;
        private MvxObservableCollection<PostItemViewModel> _postItemViewModels;
        private readonly IMvxNavigationService _navigationService;
        private bool _isRefreshing;
        private bool _isLoading;
        private bool _areAllElementsLoaded;

        #endregion

        public PostsViewModel(IMvxNavigationService navigationService, IPostService postService)
        {
            _navigationService = navigationService;
            _postService = postService;
            _filterViewModel = new FilterViewModel(new FilterManager(), new SortManager());
            PostItemViewModels = new MvxObservableCollection<PostItemViewModel>();
            this.SubscribeSearchText(nameof(SearchText), SerachCommand, () => !IsLoading && !IsRefreshing);
        }

        #region Overrided methods

        public override async Task Initialize()
        {
            await base.Initialize();
            if (PostItemViewModels == null || !PostItemViewModels.Any())
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

        public MvxObservableCollection<PostItemViewModel> PostItemViewModels
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

        public IMvxAsyncCommand OpenFilterDialogCommand => new MvxAsyncCommand(OpenFilterDialog);

        public IMvxAsyncCommand NewPostCommand => new MvxAsyncCommand(NewPost);

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
                        PostItemViewModels = new MvxObservableCollection<PostItemViewModel>(models.Select(m =>
                            new PostItemViewModel(_navigationService, m)));
                        _filterViewModel = new FilterViewModel(new FilterManager(), new SortManager());
                    });
                IsRefreshing = false;
                AreAllElementsLoaded = PostItemViewModels.Count < Constants.Pagination.InitialCount;
            }
            else
                SearchText = null;
        }

        private async Task OpenFilterDialog()
        {
            var result = await _navigationService.Navigate<PostsFilterViewModel, FilterViewModel, FilterViewModel>(_filterViewModel);
            if (result != null)
            {
                _filterViewModel = result;
                IsLoading = true;
                PostItemViewModels.Clear();
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
                    PostItemViewModels = new MvxObservableCollection<PostItemViewModel>(basePostCommentViewModels.Select(m => new PostItemViewModel(_navigationService, m)));
                    AreAllElementsLoaded = basePostCommentViewModels.Length < Constants.Pagination.InitialCount;
                });
                IsLoading = false;
            }
        }

        private async Task NewPost()
        {
            await _navigationService.Navigate<NewPostViewModel, BasePostViewModel>(new BasePostViewModel
            {
                Id = Guid.Empty,
                User = ApplicationData.CurrentUserViewModel
            });
        }

        private async Task Search()
        {
            IsRefreshing = true;
            PostItemViewModels.Clear();
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
                PostItemViewModels = new MvxObservableCollection<PostItemViewModel>(basePostViewModels.Select(m => new PostItemViewModel(_navigationService, m)));
                AreAllElementsLoaded = basePostViewModels.Length < Constants.Pagination.InitialCount;
            });
            IsRefreshing = false;
        }

        private async Task LoadMoreElements()
        {
            IsLoading = true;
            await ApiRequestHelper.HandleApiResult(() => _postService.GetAllPosts(
                PostItemViewModels.LastOrDefault().Post.PostDate,
                PostItemViewModels.LastOrDefault().Post.GetSortingObject(_filterViewModel.SortManager.SortOptions),
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
                PostItemViewModels.AddRange(basePostViewModels.Select(m => new PostItemViewModel(_navigationService, m)));
                AreAllElementsLoaded = basePostViewModels.Length < Constants.Pagination.RequestMoreCount;
            });
            IsLoading = false;
        }

        #endregion
    }
}
