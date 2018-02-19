using System;
using System.ComponentModel.DataAnnotations;

namespace RoomMateExpressWebApi.EF.Models
{
    public class ProfileComment : Entity
    {
        public User UserCommentator { get; set; }

        public User UserProfile { get; set; }

        [Required]
        public string Text { get; set; }

        public byte Grade { get; set; }

        public DateTimeOffset CommentedAt { get; set; }
    }
}
