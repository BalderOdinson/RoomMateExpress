using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RoomMateExpress.Core.ApiModels;
using RoomMateExpress.Core.Helpers;
using RoomMateExpress.Core.Helpers.Enums;

namespace RoomMateExpress.Core.Services
{
    public interface IPictureService
    {
        Task RequestPicture(int maxPixelDimension, int percentQuality, PictureOptions options);

        Task<ApiResult> DeletePicture(string filename);
    }
}
