using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoomMateExpressWebApi.EF.Models;

namespace RoomMateExpressWebApi.EF.Services
{
    public interface IAdminService
    {
        Task<List<Admin>> GetAllAdmins();

        Task<Admin> GetAdmin(Guid id, bool includeUserReports = false);

        Task<List<Admin>> GetAllAdmins(DateTimeOffset date, int numberToTake);

        Task<Admin> AddOrUpdateAdmin(Admin admin);

        Task<bool> DeleteAdmin(Guid id);
    }
}
