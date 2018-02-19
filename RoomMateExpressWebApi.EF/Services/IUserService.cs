using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoomMateExpressWebApi.EF.Models;
using RoomMateExpressWebApi.EF.ViewModels;

namespace RoomMateExpressWebApi.EF.Services
{
    public interface IUserService
    {
        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns></returns>
        Task<List<UserViewModel>> GetAllUsers();

        /// <summary>
        /// Gets all users with pagination.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="numberToTake"></param>
        /// <returns></returns>
        Task<List<UserViewModel>> GetAllUsers(DateTimeOffset date, int numberToTake);

        /// <summary>
        /// Gets user with specific id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<UserViewModel> GetUser(Guid id);

        /// <summary>
        /// Adds or uodates user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<UserViewModel> CreateOrUpdateUser(User user);

        /// <summary>
        /// Gets all roomates of specific user.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<UserViewModel>> GetUserRoomates(Guid id);

        Task<RoommateStatus> CheckRoommateStatus(Guid currentUserId, Guid userId);

        Task SendRoommateRequest(Guid currentUserId, Guid userId);

        Task AcceptRoommateRequest(Guid currentUserId, Guid userId);

        Task DeclineRoommateRequest(Guid currentUserId, Guid userId);

        /// <summary>
        /// Removes roomate from specifc user.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roommateId"></param>
        /// <returns></returns>
        Task<bool> RemoveRommate(Guid userId, Guid roommateId);

        Task<List<UserViewModel>> SearchUserByName(Guid currentUserId, string name);

        Task<List<UserViewModel>> SearchUserByName(Guid currentUserId, DateTimeOffset date, int numberToTake, string name);
    }
}
