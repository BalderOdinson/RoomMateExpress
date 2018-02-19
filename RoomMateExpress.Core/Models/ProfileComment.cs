using System;

namespace RoomMateExpress.Core.Models
{
    public class ProfileComment : Entity
    {
        public User UserCommentator { get; set; }

        public User UserProfile { get; set; }

        public string Text { get; set; }

        public byte Grade { get; set; }

        public DateTimeOffset CommentedAt { get; set; }
    }
}
