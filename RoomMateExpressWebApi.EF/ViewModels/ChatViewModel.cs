using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoomMateExpressWebApi.EF.Models;

namespace RoomMateExpressWebApi.EF.ViewModels
{
    public class ChatViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string PictureUrl { get; set; }

        public Message LastMessage { get; set; }

        public DateTimeOffset LastModified { get; set; }

        public List<User> Users { get; set; }

        public List<Message> Messages { get; set; }

        public ChatViewModel()
        {
            Users = new List<User>();
            Messages = new List<Message>();
        }
    }
}
