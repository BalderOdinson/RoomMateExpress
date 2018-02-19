using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoomMateExpressWebApi.EF.Models;

namespace BackendTest.Mockings
{
    public class PostMockings
    {
        public static Guid GetGuid(int id) => Guid.Parse($"ff9a1106-079a-46f7-ac34-ec7760e805{id:00}");

        public static List<Post> Posts { get; private set; }

        public static void Initalize()
        {
            Posts = new List<Post>();
            for (int i = 0; i < 30; i++)
            {
                Posts.Add(new Post
                {
                    Id = GetGuid(i),
                    Title = $"Post{i}",
                    User = new User { Id = GetGuid(i % 10) },
                    AccomodationOption = (AccomodationOptions)(i % 2),
                    AccomodationType = i % 2 == 0 ? (AccomodationType)(i % 2) : AccomodationType.None,
                    ArePetsAllowed = i % 2 == 0,
                    Description = $"This is magnifcient post n.{i}",
                    IsSmokerAllowed = i % 2 == 0,
                    NumberOfRoommates = (byte)(i % 10),
                    PetOptions = (PetOptions)(new Random().Next(0, 33)),
                    PostDate = DateTimeOffset.Now,
                    Price = i % 2 == 0 ? (decimal?)500.00m : null,
                    WantedGender = (Gender)(i % 3),
                    Neighborhoods = new List<Neighborhood>
                    {
                        new Neighborhood{ Id = Guid.Parse("69a11899-fa8e-47b8-8c9d-001554a03e55")},
                        new Neighborhood{ Id = Guid.Parse("a5ef97fc-6cfc-4735-abb8-020ac013bde2")},
                        new Neighborhood{ Id = Guid.Parse("c6be952c-e1af-47ed-b448-0435ca1b3cc4")}
                    },
                    PostPictures = i % 2 == 0 ? new List<PostPicture>
                    {
                        new PostPicture { PictureUrl = $"Picture{i}" },
                        new PostPicture { PictureUrl = $"Picture{i + 1}" },
                        new PostPicture { PictureUrl = $"Picture{i + 2}" }
                    } : new List<PostPicture>()
                });
            }
        }
    }
}
