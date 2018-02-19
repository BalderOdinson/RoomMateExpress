using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoomMateExpressWebApi.EF.Models;

namespace BackendTest.Mockings
{
    public class MessageMockings
    {
        public static Guid GetGuid(int id) => Guid.Parse($"ff9a1106-079a-46f7-ac34-ec7760e805{id:00}");

        public static List<Message> Messages { get; private set; }

        public static void Initalize()
        {
            Messages = new List<Message>();
            for (int i = 0; i < 30; i++)
            {
                Messages.Add(new Message
                {
                    Id = GetGuid(i),
                    Chat = new Chat { Id = GetGuid(i % 2) },
                    UserSender = new User { Id = GetGuid((i % 3) % 2 + 1) },
                    SentAt = DateTimeOffset.Now,
                    Text = $"Message{i}"
                });
            }
        }
    }
}
