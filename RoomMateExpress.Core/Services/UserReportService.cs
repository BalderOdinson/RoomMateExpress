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
    public class UserReportService : IUserReportService
    {
        private readonly IRoommateExpressApi _api;
        private readonly ILocalizationService _localizationService;

        public UserReportService(IRoommateExpressApi api, ILocalizationService localizationService)
        {
            _api = api;
            _localizationService = localizationService;
        }

        public async Task<ApiResult<IEnumerable<BaseUserReportViewModel>>> GetAllUserReports()
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                var userReports = await _api.GetAllUserReports();
                return new ApiResult<IEnumerable<BaseUserReportViewModel>>(string.Empty, true, userReports.Select(u => new BaseUserReportViewModel(u)));
            });
        }

        public async Task<ApiResult<IEnumerable<BaseUserReportViewModel>>> GetAllUserReports(int numberPerPage, DateTimeOffset oldestReport)
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                var userReports = await _api.GetAllUserReportsPart(numberPerPage, oldestReport);
                return new ApiResult<IEnumerable<BaseUserReportViewModel>>(string.Empty, true, userReports.Select(u => new BaseUserReportViewModel(u)));
            });
        }

        public async Task<ApiResult<IEnumerable<BaseUserReportViewModel>>> GetAllProcessedUserReports()
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                var userReports = await _api.GetAllProcessedUserReports();
                return new ApiResult<IEnumerable<BaseUserReportViewModel>>(string.Empty, true, userReports.Select(u => new BaseUserReportViewModel(u)));
            });
        }

        public async Task<ApiResult<IEnumerable<BaseUserReportViewModel>>> GetAllProcessedUserReports(int numberPerPage, DateTimeOffset oldestReport)
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                var userReports = await _api.GetAllProcessedUserReportsPart(numberPerPage, oldestReport);
                return new ApiResult<IEnumerable<BaseUserReportViewModel>>(string.Empty, true, userReports.Select(u => new BaseUserReportViewModel(u)));
            });
        }

        public async Task<ApiResult> ReportUser(BaseUserReportViewModel model)
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                await _api.ReportUser(Mapper.Map<UserReport>(model));
                return new ApiResult(string.Empty, true);
            });
        }

        public async Task<ApiResult<BaseUserReportViewModel>> ProcessReport(BaseUserReportViewModel model)
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                var userReport = await _api.ProcessReport(Mapper.Map<UserReport>(model));
                return new ApiResult<BaseUserReportViewModel>(string.Empty, true, new BaseUserReportViewModel(userReport));
            });
        }
    }
}
