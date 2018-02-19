
namespace RoomMateExpress.Core.Models
{
    public class PostPicture : Entity
    {
        public string PictureUrl { get; set; }

        public Post Post { get; set; }
    }
}
