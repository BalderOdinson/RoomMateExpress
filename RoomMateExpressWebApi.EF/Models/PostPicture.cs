using System.ComponentModel.DataAnnotations;

namespace RoomMateExpressWebApi.EF.Models
{
    public class PostPicture : Entity
    {
        [Required]
        [StringLength(maximumLength: 255)]
        public string PictureUrl { get; set; }

        public Post Post { get; set; }
    }
}
