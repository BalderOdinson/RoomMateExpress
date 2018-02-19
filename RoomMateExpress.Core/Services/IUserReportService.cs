using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RoomMateExpress.Core.ApiModels;
using RoomMateExpress.Core.ViewModels.BaseViewModels;

namespace RoomMateExpress.Core.Services
{
    public interface IUserReportService
    {
        Task<ApiResult<IEnumerable<BaseUserReportViewModel>>> GetAllUserReports();

        Task<ApiResult<IEnumerable<BaseUserReportViewModel>>> GetAllUserReports(int numberPerPage, DateTimeOffset oldestReport);

        Task<ApiResult<IEnumerable<BaseUserReportViewModel>>> GetAllProcessedUserReports();

        Task<ApiResult<IEnumerable<BaseUserReportViewModel>>> GetAllProcessedUserReports(int numberPerPage, DateTimeOffset oldestReport);

        Task<ApiResult> ReportUser(BaseUserReportViewModel model);

        Task<ApiResult<BaseUserReportViewModel>> ProcessReport(BaseUserReportViewModel model);
    }
}
