using System;
using System.Collections.Generic;
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
    public class AdminService : IAdminService
    {
        private readonly IRoommateExpressApi _api;
        private readonly ILocalizationService _localizationService;

        public AdminService(IRoommateExpressApi api, ILocalizationService localizationService)
        {
            _api = api;
            _localizationService = localizationService;
        }

        public async Task<ApiResult<BaseAdminViewModel>> GetAdmin()
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                var admin = await _api.GetAdmin();
                return new ApiResult<BaseAdminViewModel>(string.Empty, true, new BaseAdminViewModel(admin));
            });
        }

        public async Task<ApiResult<BaseAdminViewModel>> CreateAdmin(BaseAdminViewModel admin)
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                var newAdmin = await _api.CreateAdmin(Mapper.Map<Admin>(admin));
                return new ApiResult<BaseAdminViewModel>(string.Empty, true, new BaseAdminViewModel(newAdmin));
            });
        }

        public async Task<ApiResult> DeleteAdmin(Guid id)
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                await _api.DeleteAdmin(id);
                return new ApiResult(string.Empty, true);
            });
        }
    }
}
