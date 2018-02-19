using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoomMateExpressWebApi.EF.Models;

namespace BackendTest.Mockings
{
    public class PostCommentMockings
    {
        public static Guid GetGuid(int id) => Guid.Parse($"ff9a1106-079a-46f7-ac34-ec7760e805{id:00}");

        public static List<PostComment> PostComments { get; private set; }

        public static void Initalize()
        {
            PostComments = new List<PostComment>();
            for (int i = 0; i < 30; i++)
            {
                PostComments.Add(new PostComment
                {
                    Id = GetGuid(i),
                    User = new User { Id = GetGuid(i % 27) },
                    Post = new Post { Id = GetGuid(1) },
                    CommentedAt = DateTimeOffset.Now,
                    Text = $"PostComment{i}",
                });
            }
        }
    }
}
