using System;

namespace RoomMateExpress.Core.Models
{
    public class Message : Entity
    {
        public User UserSender { get; set; }

        public DateTimeOffset SentAt { get; set; }

        public DateTimeOffset? RecievedAt { get; set; }

        public DateTimeOffset? SeenAt { get; set; }

        public string Text { get; set; }

        public string PictureUrl { get; set; }

        public Chat Chat { get; set; }
    }
}
