using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoomMateExpressWebApi.EF.Exceptions;
using RoomMateExpressWebApi.EF.Models;

namespace RoomMateExpressWebApi.EF.Services
{
    public class PostPictureService : IPostPictureService
    {
        private readonly RoomMateExpressDbContext _context;

        public PostPictureService(RoomMateExpressDbContext context)
        {
            _context = context;
        }

        public async Task<PostPicture> GetPostPicture(Guid id)
        {
            return await _context.PostPictures.Include(p => p.Post)
                .FirstOrDefaultAsync(p => p.Id.Equals(id));
        }

        public async Task<List<PostPicture>> GetPostPictures(Guid postId)
        {
            return await _context.PostPictures
                .Where(p => p.Post.Id.Equals(postId))
                .ToListAsync();
        }

        public async Task<PostPicture> AddPostPicture(PostPicture picture)
        {
            if (!await _context.Posts.AnyAsync(p => p.Id.Equals(picture.Post.Id)))
                throw new PostNotFoundException(Constants.Errors.PostNotFound);
            _context.Entry(picture.Post).State = EntityState.Unchanged;
            _context.PostPictures.Add(picture);
            await _context.SaveChangesAsync();
            return picture;
        }

        public async Task<bool> DeletePostPicture(Guid id)
        {
            var picture = await _context.PostPictures.FirstOrDefaultAsync(p => p.Id == id);
            if (picture == null)
                return false;
            _context.PostPictures.Remove(picture);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
