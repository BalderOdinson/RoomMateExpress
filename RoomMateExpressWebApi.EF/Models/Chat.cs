using System;
using System.Collections.Generic;

namespace RoomMateExpressWebApi.EF.Models
{
    public class Chat : Entity
    {
        public List<User> Users { get; set; }

        public List<Message> Messages { get; set; }

        public string Name { get; set; }

        public string PictureUrl { get; set; }

        public DateTimeOffset LastModified { get; set; }

        public Chat()
        {
            Users = new List<User>();
            Messages = new List<Message>();
        }
    }
}
