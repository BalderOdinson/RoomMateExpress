using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoomMateExpressWebApi.EF.Models;

namespace BackendTest.Mockings
{
    public class ChatMockings
    {
        public static Guid GetGuid(int id) => Guid.Parse($"ff9a1106-079a-46f7-ac34-ec7760e805{id:00}");

        public static List<Chat> Chats { get; private set; }

        public static void Initalize()
        {
            Chats = new List<Chat>();
            for (int i = 0; i < 30; i++)
            {
                Chats.Add(new Chat
                {
                    Id = GetGuid(i),
                    Users = i % 10 == 0 ?
                        new List<User>
                        {
                            new User{ Id = GetGuid(i) },
                            new User{ Id = GetGuid((i + 1) % 30) },
                            new User{ Id = GetGuid((i + 2) % 30) }
                        }
                        : new List<User>
                        {
                            new User{ Id = GetGuid(i) },
                            new User{ Id = GetGuid((i + 1) % 30) }
                        },
                    Name = i % 10 == 0 ? $"Chat{i}" : null,
                    PictureUrl = i % 20 == 0 ? "ChatPitcure" : null,
                    Messages = i % 9 == 0 ? 
                        new List<Message>
                        {
                            new Message
                            {
                                Id = GetGuid(i),
                                Text = "Bok",
                                UserSender = new User{ Id = GetGuid(i) }
                            }
                        } : new List<Message>()
                });
            }
        }
    }
}
