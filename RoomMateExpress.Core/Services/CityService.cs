using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Refit;
using RoomMateExpress.Core.ApiModels;
using RoomMateExpress.Core.Helpers;
using RoomMateExpress.Core.Models;
using RoomMateExpress.Core.ViewModels.BaseViewModels;

namespace RoomMateExpress.Core.Services
{
    public class CityService : ICityService
    {
        private readonly IRoommateExpressApi _api;
        private readonly ILocalizationService _localizationService;

        public CityService(IRoommateExpressApi api, ILocalizationService localizationService)
        {
            _api = api;
            _localizationService = localizationService;
        }

        public async Task<ApiResult<IEnumerable<BaseCityViewModel>>> GetAllCities()
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                var cities = await _api.GetAllCities();
                return new ApiResult<IEnumerable<BaseCityViewModel>>(string.Empty, true, cities.Select(c => new BaseCityViewModel(c)));
            });
        }

        public async Task<ApiResult<BaseCityViewModel>> GetCity(Guid id)
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                var city = await _api.GetCity(id);
                return new ApiResult<BaseCityViewModel>(string.Empty, true, new BaseCityViewModel(city));
            });
        }

        public async Task<ApiResult<BaseCityViewModel>> CreateOrUpdateCity(BaseCityViewModel city)
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                var newCity = await _api.CreateOrUpdateCity(Mapper.Map<City>(city));
                return new ApiResult<BaseCityViewModel>(string.Empty, true, new BaseCityViewModel(newCity));
            });
        }

        public async Task<ApiResult> DeleteCity(Guid id)
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                await _api.DeleteCity(id);
                return new ApiResult(string.Empty, true);
            });
        }
    }
}
