using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Refit;
using RoomMateExpress.Core.ApiModels;
using RoomMateExpress.Core.Models;
using RoomMateExpress.Core.ViewModels.BaseViewModels;

namespace RoomMateExpress.Core.Services
{
    public interface IAdminService
    {
        Task<ApiResult<BaseAdminViewModel>> GetAdmin();

        Task<ApiResult<BaseAdminViewModel>> CreateAdmin(BaseAdminViewModel admin);

        Task<ApiResult> DeleteAdmin(Guid id);
    }
}
