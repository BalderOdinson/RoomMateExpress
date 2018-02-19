using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoomMateExpressWebApi.EF.Models;

namespace RoomMateExpressWebApi.EF.Services
{
    public class UserReportService : IUserReportService
    {
        private readonly RoomMateExpressDbContext _context;

        public UserReportService(RoomMateExpressDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserReport>> GetAllUserReports()
        {
            return await _context.UserReports.Include(a => a.Admin)
                .Include(u => u.UserReported)
                .Include(ur => ur.UserReporting)
                .ToListAsync();
        }

        public async Task<List<UserReport>> GetAllUserReports(DateTimeOffset oldestReport,
            int numberToTake)
        {
            return await _context.UserReports.Include(a => a.Admin)
                .Include(u => u.UserReported)
                .Include(ur => ur.UserReporting)
                .Where(ur => ur.DateReporting < oldestReport).Take(numberToTake).ToListAsync();
        }

        public async Task<List<UserReport>> GetAllInProcessUserReports()
        {
            return await _context.UserReports.Include(a => a.Admin)
                .Include(u => u.UserReported)
                .Include(ur => ur.UserReporting)
                .Where(ur => !ur.IsProcessed).ToListAsync();
        }

        public async Task<List<UserReport>> GetAllInProcessUserReports(DateTimeOffset oldestReport,
            int numberToTake)
        {
            return await _context.UserReports.Include(a => a.Admin)
                .Include(u => u.UserReported)
                .Include(ur => ur.UserReporting)
                .Where(ur => !ur.IsProcessed && ur.DateReporting < oldestReport).Take(numberToTake).ToListAsync();
        }

        public async Task<List<UserReport>> GetAllProcessedUserReports()
        {
            return await _context.UserReports.Include(a => a.Admin)
                .Include(u => u.UserReported)
                .Include(ur => ur.UserReporting)
                .Where(ur => ur.IsProcessed).ToListAsync();
        }

        public async Task<List<UserReport>> GetAllProcessedUserReports(DateTimeOffset oldestReport,
            int numberToTake)
        {
            return await _context.UserReports.Include(a => a.Admin)
                .Include(u => u.UserReported)
                .Include(ur => ur.UserReporting)
                .Where(ur => ur.IsProcessed && ur.DateReporting < oldestReport).Take(numberToTake).ToListAsync();
        }

        public async Task<List<UserReport>> GetUserReports(Guid adminId)
        {
            return await _context.UserReports
                .Include(u => u.UserReported)
                .Include(ur => ur.UserReporting)
                .Where(ur => ur.Admin.Id.Equals(adminId)).ToListAsync();
        }

        public async Task<List<UserReport>> GetUserReports(Guid adminId, DateTimeOffset oldestReport,
            int numberToTake)
        {
            return await _context.UserReports
                .Include(u => u.UserReported)
                .Include(ur => ur.UserReporting)
                .Where(ur => ur.Admin.Id.Equals(adminId) && ur.DateReporting < oldestReport).Take(numberToTake).ToListAsync();
        }

        public async Task<List<UserReport>> GetInProcessUserReports(Guid adminId)
        {
            return await _context.UserReports
                .Include(u => u.UserReported)
                .Include(ur => ur.UserReporting)
                .Where(ur => ur.Admin.Id.Equals(adminId))
                .Where(ur => !ur.IsProcessed).ToListAsync();
        }

        public async Task<List<UserReport>> GetInProcessUserReports(Guid adminId, DateTimeOffset oldestReport,
             int numberToTake)
        {
            return await _context.UserReports
                .Include(u => u.UserReported)
                .Include(ur => ur.UserReporting)
                .Where(ur => ur.Admin.Id.Equals(adminId))
                .Where(ur => !ur.IsProcessed && ur.DateReporting < oldestReport).Take(numberToTake).ToListAsync();
        }

        public async Task<List<UserReport>> GetProcessedUserReports(Guid adminId)
        {
            return await _context.UserReports
                .Include(u => u.UserReported)
                .Include(ur => ur.UserReporting)
                .Where(ur => ur.Admin.Id.Equals(adminId))
                .Where(ur => ur.IsProcessed).ToListAsync();
        }

        public async Task<List<UserReport>> GetProcessedUserReports(Guid adminId, DateTimeOffset oldestReport,
            int numberToTake)
        {
            return await _context.UserReports
                .Include(u => u.UserReported)
                .Include(ur => ur.UserReporting)
                .Where(ur => ur.Admin.Id.Equals(adminId))
                .Where(ur => ur.IsProcessed && ur.DateReporting < oldestReport).Take(numberToTake).ToListAsync();
        }

        public async Task<List<UserReport>> GetUserReportsHistory(Guid userId)
        {
            return await _context.UserReports
                .Include(a => a.Admin)
                .Include(u => u.UserReported)
                .Include(ur => ur.UserReporting)
                .Where(ur => ur.UserReported.Id.Equals(userId))
                .ToListAsync();
        }

        public async Task<List<UserReport>> GetUserReportsHistory(Guid userId, DateTimeOffset oldestReport,
            int numberToTake)
        {
            return await _context.UserReports
                .Include(a => a.Admin)
                .Include(u => u.UserReported)
                .Include(ur => ur.UserReporting)
                .Where(ur => ur.UserReported.Id.Equals(userId) && ur.DateReporting < oldestReport)
                .Take(numberToTake)
                .ToListAsync();
        }

        public async Task<List<UserReport>> GetUserReportingHistory(Guid userId)
        {
            return await _context.UserReports
                .Include(a => a.Admin)
                .Include(u => u.UserReported)
                .Include(ur => ur.UserReporting)
                .Where(ur => ur.UserReporting.Id.Equals(userId))
                .ToListAsync();
        }

        public async Task<List<UserReport>> GetUserReportingHistory(Guid userId, DateTimeOffset oldestReport,
            int numberToTake)
        {
            return await _context.UserReports
                .Include(a => a.Admin)
                .Include(u => u.UserReported)
                .Include(ur => ur.UserReporting)
                .Where(ur => ur.UserReporting.Id.Equals(userId) && ur.DateReporting < oldestReport)
                .Take(numberToTake)
                .ToListAsync();
        }

        public async Task<UserReport> ReportUser(Guid userToReportId, Guid userReportingId, string text)
        {
            var users = await _context.Users.AsNoTracking().Where(u => u.Id.Equals(userReportingId) || u.Id.Equals(userToReportId)).ToListAsync();
            if (users.Count != 2)
                return null;
            var userReporting = users.FirstOrDefault(u => u.Id.Equals(userReportingId));
            var userReported = users.FirstOrDefault(u => u.Id.Equals(userToReportId));
            var admin = await _context.Admins.AsNoTracking().FirstOrDefaultAsync(a =>
                a.UserReports.Count.Equals(_context.Admins.Min(ac => ac.UserReports.Count)));
            var userReport = new UserReport
            {
                Admin = admin,
                UserReporting = userReporting,
                UserReported = userReported,
                DateReporting = DateTimeOffset.Now,
                Text = text
            };
            _context.Entry(admin).State = EntityState.Unchanged;
            _context.Entry(userReporting).State = EntityState.Unchanged;
            _context.Entry(userReported).State = EntityState.Unchanged;
            _context.UserReports.Add(userReport);
            await _context.SaveChangesAsync();
            return userReport;
        }

        public async Task<UserReport> ProcessReport(Guid id, string decision)
        {
            var userReport = await _context.UserReports.FirstOrDefaultAsync(ur => ur.Id.Equals(id));
            if (userReport == null)
                return null;
            userReport.AdminDecision = decision;
            userReport.DateProcessed = DateTimeOffset.Now;
            await _context.SaveChangesAsync();
            return userReport;
        }

        public async Task<UserReport> SetInProcessReport(Guid id)
        {
            var userReport = await _context.UserReports.FirstOrDefaultAsync(ur => ur.Id.Equals(id));
            if (userReport == null)
                return null;
            userReport.AdminDecision = null;
            userReport.DateProcessed = null;
            await _context.SaveChangesAsync();
            return userReport;
        }
    }
}
