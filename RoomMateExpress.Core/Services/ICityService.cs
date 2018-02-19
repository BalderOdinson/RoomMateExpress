using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RoomMateExpress.Core.ApiModels;
using RoomMateExpress.Core.ViewModels.BaseViewModels;

namespace RoomMateExpress.Core.Services
{
    public  interface ICityService
    {
        Task<ApiResult<IEnumerable<BaseCityViewModel>>> GetAllCities();

        Task<ApiResult<BaseCityViewModel>> GetCity(Guid id);

        Task<ApiResult<BaseCityViewModel>> CreateOrUpdateCity(BaseCityViewModel city);

        Task<ApiResult> DeleteCity(Guid id);
    }
}
