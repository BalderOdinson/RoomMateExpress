using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RoomMateExpress.Core.ApiModels;
using RoomMateExpress.Core.ViewModels.BaseViewModels;

namespace RoomMateExpress.Core.Services
{
    public interface INeighborhoodService
    {
        Task<ApiResult<IEnumerable<BaseNeighborhoodViewModel>>> GetAllNeighborhoods();

        Task<ApiResult<BaseNeighborhoodViewModel>> GetNeighborhood(Guid id);
    }
}
