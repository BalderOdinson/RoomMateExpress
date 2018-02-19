using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoomMateExpressWebApi.EF.Models;

namespace BackendTest.Mockings
{
    public class UserMockings
    {
        public static Guid GetGuid(int id) => Guid.Parse($"ff9a1106-079a-46f7-ac34-ec7760e805{id:00}");

        public static List<User> Users { get; private set; }

        public static void Initalize()
        {
            Users = new List<User>();
            for (int i = 0; i < 30; i++)
            {
                Users.Add(new User
                {
                    Id = GetGuid(i),
                    FirstName = $"User{i:00}",
                    LastName = $"User{i:00}",
                    BirthDate = DateTimeOffset.Now.Subtract(new TimeSpan(6000000000000000)),
                    CreationDate = DateTimeOffset.Now,
                    Gender = Gender.Male,
                    DescriptionOfStudyOrWork = "MockStudy",
                    HasFaculty = true,
                    IsSmoker = false,
                    ProfilePictureUrl = $"profile{i}",
                });
            }
        }
    }
}
