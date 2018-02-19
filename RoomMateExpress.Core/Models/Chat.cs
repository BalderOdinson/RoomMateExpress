using System;
using System.Collections.Generic;

namespace RoomMateExpress.Core.Models
{
    public class Chat : Entity
    {
        public string Name { get; set; }

        public string PictureUrl { get; set; }

        public Message LastMessage { get; set; }

        public DateTimeOffset LastModified { get; set; }

        public List<User> Users { get; set; }

        public List<Message> Messages { get; set; }

        public Chat()
        {
            Users = new List<User>();
            Messages = new List<Message>();
        }

    }
}
