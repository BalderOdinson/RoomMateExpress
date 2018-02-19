using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using RoomMateExpressWebApi.EF.Exceptions;
using RoomMateExpressWebApi.EF.Extensions;
using RoomMateExpressWebApi.EF.Models;
using RoomMateExpressWebApi.EF.ViewModels;

namespace RoomMateExpressWebApi.EF.Services
{
    public class UserService : IUserService
    {
        private readonly RoomMateExpressDbContext _context;

        public UserService(RoomMateExpressDbContext context)
        {
            _context = context;
        }


        public async Task<List<UserViewModel>> GetAllUsers()
        {
            return (await _context.GetUsersAsync()).ToList();
        }

        public async Task<List<UserViewModel>> GetAllUsers(DateTimeOffset date, int numberToTake)
        {
            return (await _context.GetUsersAsync(date, numberToTake)).ToList();
        }


        public async Task<UserViewModel> GetUser(Guid id)
        {
            return await _context.GetUserAsync(id);
        }

        public async Task<UserViewModel> CreateOrUpdateUser(User user)
        {
            user.CreationDate = DateTimeOffset.Now;
            _context.Users.AddOrUpdate(user);
            await _context.SaveChangesAsync();
            return await _context.GetUserAsync(user.Id);
        }

        public async Task<List<UserViewModel>> GetUserRoomates(Guid id)
        {
            return (await _context.GetRoommatesAsync(id)).ToList();
        }

        public async Task<RoommateStatus> CheckRoommateStatus(Guid currentUserId, Guid userId)
        {
            var currentUser = await _context.Users.Include(u => u.MyRoommates)
                .Include(u => u.RoommatesWithMe)
                .Include(u => u.SentRoommateRequests)
                .Include(u => u.RecievedRoommateRequests)
                .Include(u => u.ProfileComments)
                .FirstOrDefaultAsync(u => u.Id == currentUserId);
            var user = new User { Id = userId };
            if (currentUser.SentRoommateRequests.Contains(user))
                return RoommateStatus.RequestSent;
            if (currentUser.RecievedRoommateRequests.Contains(user))
                return RoommateStatus.RequestRecieved;
            if (currentUser.ProfileComments.Select(cm => cm.UserProfile).Contains(user))
                return RoommateStatus.RoommatesRated;
            if (currentUser.MyRoommates.Union(currentUser.RoommatesWithMe).Contains(user))
                return RoommateStatus.Roommates;
            return RoommateStatus.None;
        }

        public async Task SendRoommateRequest(Guid currentUserId, Guid userId)
        {
            var currentUser = await _context.Users
                .Include(u => u.SentRoommateRequests)
                .Include(u => u.RecievedRoommateRequests)
                .Include(u => u.MyRoommates)
                .Include(u => u.RoommatesWithMe)
                .FirstOrDefaultAsync(u => u.Id == currentUserId);
            var user = new User { Id = userId };
            if (currentUser.SentRoommateRequests.Contains(user)
                || currentUser.RecievedRoommateRequests.Contains(user)
                || currentUser.MyRoommates.Contains(user)
                || currentUser.RoommatesWithMe.Contains(user))
                throw new DuplicateRequestException(Constants.Errors.DuplicateRequest);
            _context.Entry(user).State = EntityState.Unchanged;
            currentUser.SentRoommateRequests.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task AcceptRoommateRequest(Guid currentUserId, Guid userId)
        {
            var currentUser = await _context.Users
                .Include(u => u.RecievedRoommateRequests)
                .Include(u => u.MyRoommates)
                .FirstOrDefaultAsync(u => u.Id == currentUserId);
            var user = new User { Id = userId };
            if (!currentUser.RecievedRoommateRequests.Contains(user))
                throw new UserNotFoundException(Constants.Errors.UserNotFound);
            user = currentUser.RecievedRoommateRequests.FirstOrDefault(u => u.Id == userId);
            currentUser.RecievedRoommateRequests.Remove(user);
            currentUser.MyRoommates.Add(_context.Entry(user).Entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeclineRoommateRequest(Guid currentUserId, Guid userId)
        {
            var currentUser = await _context.Users
                .Include(u => u.RecievedRoommateRequests)
                .FirstOrDefaultAsync(u => u.Id == currentUserId);
            var user = new User { Id = userId };
            if (!currentUser.RecievedRoommateRequests.Contains(user))
                throw new UserNotFoundException(Constants.Errors.UserNotFound);
            user = currentUser.RecievedRoommateRequests.FirstOrDefault(u => u.Id == userId);
            currentUser.RecievedRoommateRequests.Remove(user);
            await _context.SaveChangesAsync();
        }


        public async Task<bool> RemoveRommate(Guid userId, Guid roommateId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(ur => ur.Id.Equals(userId));
            var roomate = await _context.Users.FirstOrDefaultAsync(ur => ur.Id.Equals(roommateId));
            if (user == null || roomate == null)
                return false;
            if (user.MyRoommates.Contains(roomate))
            {
                user.MyRoommates.Remove(roomate);
            }
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<UserViewModel>> SearchUserByName(Guid currentUserId, string name)
        {
            return (await _context.GetUsersAsync(currentUserId, name)).ToList();
        }

        public async Task<List<UserViewModel>> SearchUserByName(Guid currentUserId, DateTimeOffset date, int numberToTake, string name)
        {
            return (await _context.GetUsersAsync(currentUserId, date, numberToTake, name)).ToList();
        }
    }
}
