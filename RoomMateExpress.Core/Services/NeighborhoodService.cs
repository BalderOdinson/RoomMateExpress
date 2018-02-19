using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Refit;
using RoomMateExpress.Core.ApiModels;
using RoomMateExpress.Core.Helpers;
using RoomMateExpress.Core.ViewModels.BaseViewModels;

namespace RoomMateExpress.Core.Services
{
    public class NeighborhoodService : INeighborhoodService
    {
        private readonly IRoommateExpressApi _api;
        private readonly ILocalizationService _localizationService;

        public NeighborhoodService(IRoommateExpressApi api, ILocalizationService localizationService)
        {
            _api = api;
            _localizationService = localizationService;
        }

        public async Task<ApiResult<IEnumerable<BaseNeighborhoodViewModel>>> GetAllNeighborhoods()
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                var neighborhoods = await _api.GetAllNeighborhoods();
                return new ApiResult<IEnumerable<BaseNeighborhoodViewModel>>(string.Empty, true, neighborhoods.Select(c => new BaseNeighborhoodViewModel(c)));
            });
        }

        public async Task<ApiResult<BaseNeighborhoodViewModel>> GetNeighborhood(Guid id)
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                var neighborhood = await _api.GetNeighborhood(id);
                return new ApiResult<BaseNeighborhoodViewModel>(string.Empty, true, new BaseNeighborhoodViewModel(neighborhood));
            });
        }
    }
}
