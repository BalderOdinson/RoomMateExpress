using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Validation;
using FirebaseCloudMessagingApi;
using FirebaseCloudMessagingApi.Helpers;
using FirebaseCloudMessagingApi.Models;
using FirebaseCloudMessagingApi.Models.Android;
using FirebaseCloudMessagingApi.Models.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using RoomMateExpressWebApi.EF.Models;
using RoomMateExpressWebApi.EF.Services;
using RoomMateExpressWebApi.Helpers;
using RoomMateExpressWebApi.Models;
using RoomMateExpressWebApi.Models.AccountViewModels;
using RoomMateExpressWebApi.Services;

namespace RoomMateExpressWebApi.Controllers
{
    [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme, Policy = Constants.Authorization.UserAndAdministratorPolicy)]
    [Produces("application/json")]
    [Route("user")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IFirebaseCloudMessagingService _firebaseCloudMessagingService;

        public UserController(IUserService userService, UserManager<ApplicationUser> userManager, IFirebaseCloudMessagingService firebaseCloudMessagingService)
        {
            _userService = userService;
            _userManager = userManager;
            _firebaseCloudMessagingService = firebaseCloudMessagingService;
        }

        [HttpPut]
        public async Task<IActionResult> CreateOrUpdateUser([FromBody] User model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                if (user != null)
                {
                    if (user.ApplicationUserInfo == null)
                    {
                        model.Id = Guid.NewGuid();
                        var result = await _userService.CreateOrUpdateUser(model);
                        user.ApplicationUserInfo = result.Id;
                        await _userManager.UpdateAsync(user);
                        return Ok(result);
                    }
                    else
                    {
                        model.Id = user.ApplicationUserInfo.Value;
                        var result = await _userService.CreateOrUpdateUser(model);
                        return Ok(result);
                    }
                }
                return NotFound(Constants.Errors.UserNotFoundError);

            }
            return BadRequest(Constants.Errors.InvalidInput);
        }

        [HttpPut("send-request/{userId}")]
        public async Task<IActionResult> SendRoommateRequest(Guid userId)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                if (user?.ApplicationUserInfo != null)
                {
                    await _userService.SendRoommateRequest(user.ApplicationUserInfo.Value, userId);
                    var userSender = await _userService.GetUser(user.ApplicationUserInfo.Value);
                    await _firebaseCloudMessagingService.SendNotification(new TopicMessage
                    {
                        Android = new AndroidConfig
                        {
                            Notification = new AndroidNotification
                            {
                                TitleLocKey = Constants.FirebaseNotificaton.RoommateRequestTitleLocAndroid,
                                BodyLocKey = Constants.FirebaseNotificaton.RoommateRequestBodyLocAndroid,
                                BodyLocArgs = new List<string> { $"{userSender.FirstName} {userSender.LastName}" },
                                ClickAction = Constants.FirebaseNotificaton.RoommateRequestClickActionAndroid,
                                Color = Constants.FirebaseNotificaton.DefaultColorAndroid.ToFcmColor(),
                                Icon = Constants.FirebaseNotificaton.RoommateRequestIconAndroid,
                                Sound = Constants.FirebaseNotificaton.DefaultSoundAndroid
                            },
                            Priority = AndroidMessagePriority.Normal,
                            RestrictedPackageName = Constants.FirebaseNotificaton.RestrictedPackageNameAndroid,
                            Ttl = FirebaseCloudMessagingApiConstants.MaxTtl
                        },
                        Data = new Dictionary<string, string>
                        {
                            { "UserId", user.ApplicationUserInfo.ToString() }
                        },
                        Notification = new Notification
                        {
                            Title = Constants.FirebaseNotificaton.RoommateRequestTitle,
                            Body = $"{userSender.FirstName} {userSender.LastName}" + Constants.FirebaseNotificaton.RoommateRequestBody
                        },
                        Topic = userId.ToString()
                    }, false);
                    return Ok();
                }
                return NotFound(Constants.Errors.UserNotFoundError);
            }
            return BadRequest(Constants.Errors.InvalidInput);
        }

        [HttpPut("accept-request/{userId}")]
        public async Task<IActionResult> AcceptRoommateRequest(Guid userId)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                if (user?.ApplicationUserInfo != null)
                {
                    await _userService.AcceptRoommateRequest(user.ApplicationUserInfo.Value, userId);
                    return Ok();
                }
                return NotFound(Constants.Errors.UserNotFoundError);
            }
            return BadRequest(Constants.Errors.InvalidInput);
        }

        [HttpPut("decline-request/{userId}")]
        public async Task<IActionResult> DeclineRoommateRequest(Guid userId)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                if (user?.ApplicationUserInfo != null)
                {
                    await _userService.DeclineRoommateRequest(user.ApplicationUserInfo.Value, userId);
                    return Ok();
                }
                return NotFound(Constants.Errors.UserNotFoundError);
            }
            return BadRequest(Constants.Errors.InvalidInput);
        }

        [HttpGet("status/{userId}")]
        public async Task<IActionResult> CheckRoommateStatus(Guid userId)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                if (user?.ApplicationUserInfo != null)
                {
                    return Ok(await _userService.CheckRoommateStatus(user.ApplicationUserInfo.Value, userId));
                }
                return NotFound(Constants.Errors.UserNotFoundError);
            }
            return BadRequest(Constants.Errors.InvalidInput);
        }

        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user?.ApplicationUserInfo != null)
            {
                return Ok(_userService.GetUser(user.ApplicationUserInfo.Value));
            }
            return NotFound(Constants.Errors.UserNotFoundError);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            return Ok(await _userService.GetUser(id));
        }

        [HttpGet("search/{name}")]
        public async Task<IActionResult> SearchUserByName(string name)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user?.ApplicationUserInfo != null)
            {
                return Ok(await _userService.SearchUserByName(user.ApplicationUserInfo.Value, name));
            }
            return NotFound(Constants.Errors.UserNotFoundError);
        }

        [HttpGet("search/{name}/page")]
        public async Task<IActionResult> SearchUserByName(string name, DateTimeOffset date, int numberToTake)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user?.ApplicationUserInfo != null)
            {
                return Ok(await _userService.SearchUserByName(user.ApplicationUserInfo.Value, date, numberToTake, name));
            }
            return NotFound(Constants.Errors.UserNotFoundError);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(await _userService.GetAllUsers());
        }


        [HttpGet("page")]
        public async Task<IActionResult> GetAllUsers(DateTimeOffset date, int numberToTake)
        {
            return Ok(await _userService.GetAllUsers(date, numberToTake));
        }

        [HttpGet("roommates")]
        public async Task<IActionResult> GetUserRoomates()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user != null)
            {
                var result = await _userService.GetUserRoomates(user.Id);
                return Ok(result);
            }
            return NotFound(Constants.Errors.UserNotFoundError);
        }


        //DELETE Roomate
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveRoomate(Guid id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user != null)
            {
                var result = await _userService.RemoveRommate(user.Id, id);
                return Ok(result);
            }
            return NotFound(Constants.Errors.UserNotFoundError);
        }
    }
}