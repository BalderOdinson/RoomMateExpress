using System;
using System.ComponentModel.DataAnnotations;

namespace RoomMateExpressWebApi.EF.Models
{
    public class PostComment : Entity
    {
        public User User { get; set; }

        public Post Post { get; set; }

        [Required]
        public string Text { get; set; }

        public DateTimeOffset CommentedAt { get; set; }
    }
}
