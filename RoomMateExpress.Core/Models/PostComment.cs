using System;

namespace RoomMateExpress.Core.Models
{
    public class PostComment : Entity
    {
        public User User { get; set; }

        public Post Post { get; set; }

        public string Text { get; set; }

        public DateTimeOffset CommentedAt { get; set; }
    }
}
