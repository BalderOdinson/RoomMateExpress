using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Refit;
using RoomMateExpress.Core.ApiModels;
using RoomMateExpress.Core.Helpers;
using RoomMateExpress.Core.ViewModels.BaseViewModels;

namespace RoomMateExpress.Core.Services
{
    public class PostPictureService : IPostPictureService
    {
        private readonly IRoommateExpressApi _api;
        private readonly ILocalizationService _localizationService;

        public PostPictureService(IRoommateExpressApi api, ILocalizationService localizationService)
        {
            _api = api;
            _localizationService = localizationService;
        }

        public async Task<ApiResult<BasePostPictureViewModel>> GetPostPicture(Guid id)
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                var postPicture = await _api.GetPostPicture(id);
                return new ApiResult<BasePostPictureViewModel>(string.Empty, true, new BasePostPictureViewModel(postPicture));
            });
        }

        public async Task<ApiResult> DeletePostPicture(Guid id)
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                await _api.DeletePostPicture(id);
                return new ApiResult(string.Empty, true);
            });
        }
    }
}
