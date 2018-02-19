using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoomMateExpressWebApi.EF.Models;

namespace BackendTest.Mockings
{
    public static class AdminMockings
    {
        public static Guid GetGuid(int id) => Guid.Parse($"ff9a1106-079a-46f7-ac34-ec7760e805{id:00}");

        public static List<Admin> Admins { get; private set; }

        public static void Initalize()
        {
            Admins = new List<Admin>();
            for (int i = 0; i < 30; i++)
            {
                Admins.Add(new Admin
                {
                    Id = GetGuid(i),
                    FirstName = $"Admin{i:00}",
                    LastName = $"Admin{i:00}",
                });
            }
        }
    }
}
