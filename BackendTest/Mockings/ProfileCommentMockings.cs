using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoomMateExpressWebApi.EF.Models;

namespace BackendTest.Mockings
{
    public class ProfileCommentMockings
    {
        public static Guid GetGuid(int id) => Guid.Parse($"ff9a1106-079a-46f7-ac34-ec7760e805{id:00}");

        public static List<ProfileComment> ProfileComments { get; private set; }

        public static void Initalize()
        {
            ProfileComments = new List<ProfileComment>();
            for (int i = 0; i < 30; i++)
            {
                ProfileComments.Add(new ProfileComment
                {
                    Id = GetGuid(i),
                    UserProfile = new User { Id = GetGuid(i % 2) },
                    UserCommentator = new User { Id = GetGuid(1 - i % 2) },
                    CommentedAt = DateTimeOffset.Now,
                    Text = $"ProfileComment{i}",
                    Grade = (byte) (i % 6)
                });
            }
        }
    }
}
