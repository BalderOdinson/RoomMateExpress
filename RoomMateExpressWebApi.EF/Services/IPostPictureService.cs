using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoomMateExpressWebApi.EF.Models;

namespace RoomMateExpressWebApi.EF.Services
{
    public interface IPostPictureService
    {
        Task<List<PostPicture>> GetPostPictures(Guid postId);

        Task<PostPicture> GetPostPicture(Guid id);

        Task<PostPicture> AddPostPicture(PostPicture picture);

        Task<bool> DeletePostPicture(Guid id);
    }
}
