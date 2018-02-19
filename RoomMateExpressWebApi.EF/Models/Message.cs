using System;
using System.ComponentModel.DataAnnotations;

namespace RoomMateExpressWebApi.EF.Models
{
    public class Message : Entity
    {
        public User UserSender { get; set; }

        public DateTimeOffset SentAt { get; set; }

        public DateTimeOffset? RecievedAt { get; set; }

        public DateTimeOffset? SeenAt { get; set; }

        public string Text { get; set; }

        [StringLength(maximumLength: 255)]
        public string PictureUrl { get; set; }

        public Chat Chat { get; set; }
    }
}
