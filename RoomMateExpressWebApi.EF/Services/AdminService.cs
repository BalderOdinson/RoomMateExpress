using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoomMateExpressWebApi.EF.Models;

namespace RoomMateExpressWebApi.EF.Services
{
    public class AdminService : IAdminService
    {
        private readonly RoomMateExpressDbContext _context;

        public AdminService(RoomMateExpressDbContext context)
        {
            _context = context;
        }

        public async Task<List<Admin>> GetAllAdmins()
        {
            return await _context.Admins.Include(a => a.UserReports).ToListAsync();
        }

        public async Task<List<Admin>> GetAllAdmins(DateTimeOffset date, int numberToTake)
        {
            return await _context.Admins
                .OrderByDescending(admin => admin.CreationDate)
                .Where(admin => admin.CreationDate < date)
                .Take(numberToTake)
                .ToListAsync();

        }

        public async Task<Admin> GetAdmin(Guid id, bool includeUserReports = false)
        {
            if (includeUserReports)
                return await _context.Admins
                    .Include(a => a.UserReports)
                    .FirstOrDefaultAsync(a => a.Id.Equals(id));
            return await _context.Admins.FirstOrDefaultAsync(a => a.Id.Equals(id));
        }

        public async Task<Admin> AddOrUpdateAdmin(Admin admin)
        {
            admin.CreationDate = DateTimeOffset.Now;
            _context.Admins.AddOrUpdate(admin);
            await _context.SaveChangesAsync();
            return admin;
        }

        public async Task<bool> DeleteAdmin(Guid id)
        {
            var admin = await _context.Admins.FirstOrDefaultAsync(a => a.Id == id);
            if (admin == null)
                return false;
            _context.Admins.Remove(admin);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
