using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using RoomMateExpress.Core.Helpers;
using RoomMateExpress.Core.Helpers.Enums;
using RoomMateExpress.Core.Helpers.SortFilterHelpers;
using RoomMateExpress.Core.Models;
using RoomMateExpress.Core.Services;
using RoomMateExpress.Core.ViewModels.BaseViewModels;

namespace RoomMateExpress.Core.ViewModels
{
    public class PostsFilterViewModel : MvxViewModel<FilterViewModel, FilterViewModel>
    {
        #region Private fields

        private readonly IMvxNavigationService _navigationService;
        private decimal _max;
        private MvxObservableCollection<BaseCityViewModel> _citiesBaseCityViewModels;
        private readonly ICityService _cityService;
        private readonly IToastSerivce _toastSerivce;

        #endregion

        public PostsFilterViewModel(IMvxNavigationService navigationService, ICityService cityService, IToastSerivce toastSerivce)
        {
            _navigationService = navigationService;
            _cityService = cityService;
            _toastSerivce = toastSerivce;
            _max = 10000.00m;
            CitiesBaseCityViewModels = new MvxObservableCollection<BaseCityViewModel>();
        }

        #region Overrided methods

        public override async Task Initialize()
        {
            await base.Initialize();
            var result = await _cityService.GetAllCities();
            if (result.Success)
            {
                CitiesBaseCityViewModels = new MvxObservableCollection<BaseCityViewModel>(result.Result);
                return;
            }
            _toastSerivce.ShowByValue(result.Error);
        }

        #endregion

        #region Properties

        public MvxObservableCollection<BaseCityViewModel> CitiesBaseCityViewModels
        {
            get => _citiesBaseCityViewModels;
            set => SetProperty(ref _citiesBaseCityViewModels, value);
        }

        public FilterManager FilterManager { get; set; }

        public SortManager SortManager { get; set; }

        public bool IsDateSorting
        {
            get => SortManager.SortOptions == PostSortOptions.Date;
            set
            {
                if (!value) return;
                SortManager.SortOptions = PostSortOptions.Date;
                RaisePropertyChanged(nameof(IsDateSorting));
            }
        }

        public bool IsPopularitySorting
        {
            get => SortManager.SortOptions == PostSortOptions.Popularity;
            set
            {
                if (!value) return;
                SortManager.SortOptions = PostSortOptions.Popularity;
                RaisePropertyChanged(nameof(IsPopularitySorting));
            }
        }

        public bool IsPriceSorting
        {
            get => SortManager.SortOptions == PostSortOptions.Price;
            set
            {
                if (!value) return;
                SortManager.SortOptions = PostSortOptions.Price;
                RaisePropertyChanged(nameof(IsPriceSorting));
            }
        }

        public bool IsUserRatingSorting
        {
            get => SortManager.SortOptions == PostSortOptions.UserRating;
            set
            {
                if (!value) return;
                SortManager.SortOptions = PostSortOptions.UserRating;
                RaisePropertyChanged(nameof(IsPriceSorting));
            }
        }

        public bool IsAscendingSorting
        {
            get => SortManager.OrderOption == SortOrderOption.Ascending;
            set
            {
                SortManager.OrderOption = value ? SortOrderOption.Ascending : SortOrderOption.Descending;
                RaisePropertyChanged(nameof(IsAscendingSorting));
            }
        }

        public bool HasAccommodationFilter
        {
            get => FilterManager.ByAccomodation;
            set
            {
                FilterManager.ByAccomodation = value;
                RaisePropertyChanged(nameof(HasAccommodationFilter));
            }
        }

        public bool HasPriceFilter
        {
            get => FilterManager.ByPrice;
            set
            {
                FilterManager.ByPrice = value;
                RaisePropertyChanged(nameof(HasPriceFilter));
            }
        }

        public bool IsWithAccommodation
        {
            get => FilterManager.AccomodationOptions == AccomodationOptions.With;
            set
            {
                FilterManager.AccomodationOptions = value ? AccomodationOptions.With : AccomodationOptions.Without;
                RaisePropertyChanged(nameof(IsWithAccommodation));
            }
        }

        public bool HasCityFilter
        {
            get => FilterManager.ByCity;
            set
            {
                FilterManager.ByCity = value;
                RaisePropertyChanged(nameof(HasCityFilter));
            }
        }

        public override void Prepare(FilterViewModel parameter)
        {
            FilterManager = parameter.FilterManager;
            SortManager = parameter.SortManager;
            FilterManager.MaxPrice = _max;
            RaiseAllPropertiesChanged();
        }

        public BaseCityViewModel SelectedCity
        {
            get => Mapper.Map<BaseCityViewModel>(FilterManager.City);
            set
            {
                FilterManager.City = Mapper.Map<City>(value);
                RaisePropertyChanged();
            }
        }

        public decimal PriceFrom
        {
            get => FilterManager.MinPrice;
            set
            {
                FilterManager.MinPrice = value;
                RaisePropertyChanged(nameof(PriceFrom));
            }
        }

        public decimal PriceTo
        {
            get => FilterManager.MaxPrice;
            set
            {
                FilterManager.MaxPrice = value;
                RaisePropertyChanged(nameof(PriceTo));
            }
        }

        public decimal Max
        {
            get => _max;
            set => SetProperty(ref _max, value);
        }

        #endregion

        #region Commands

        public IMvxAsyncCommand CloseCommand => new MvxAsyncCommand(async () => await Close());

        #endregion

        #region Methods

        private async Task Close()
        {
            await _navigationService.Close(this, new FilterViewModel(FilterManager, SortManager));
        }

        #endregion
    }
}
