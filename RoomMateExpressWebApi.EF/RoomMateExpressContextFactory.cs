using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoomMateExpressWebApi.EF.Models;

namespace RoomMateExpressWebApi.EF
{
    public class RoomMateExpressContextFactory : IDbContextFactory<RoomMateExpressDbContext>
    {
        public RoomMateExpressDbContext Create()
        {
            return new RoomMateExpressDbContext(System.Configuration.ConfigurationManager.
                ConnectionStrings["RoomMateExpressDb"].ConnectionString);
        }
    }
}
