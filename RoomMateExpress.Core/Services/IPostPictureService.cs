using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RoomMateExpress.Core.ApiModels;
using RoomMateExpress.Core.ViewModels.BaseViewModels;

namespace RoomMateExpress.Core.Services
{
    public interface IPostPictureService
    {
        Task<ApiResult<BasePostPictureViewModel>> GetPostPicture(Guid id);

        Task<ApiResult> DeletePostPicture(Guid id);
    }
}
