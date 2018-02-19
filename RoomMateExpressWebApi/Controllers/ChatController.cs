using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RoomMateExpressWebApi.EF.Exceptions;
using RoomMateExpressWebApi.EF.Models;
using RoomMateExpressWebApi.EF.Services;
using RoomMateExpressWebApi.Helpers;
using RoomMateExpressWebApi.Models;

namespace RoomMateExpressWebApi.Controllers
{
    [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme, Policy = Constants.Authorization.UserAndAdministratorPolicy)]
    [Produces("application/json")]
    [Route("chat")]
    public class ChatController : Controller
    {
        private readonly IChatService _chatService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ChatController(IChatService chatService, UserManager<ApplicationUser> userManager)
        {
            _chatService = chatService;
            _userManager = userManager;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetChat(Guid id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user?.ApplicationUserInfo != null)
            {
                return Ok(await _chatService.GetChat(user.ApplicationUserInfo.Value, id));
            }
            return NotFound(Constants.Errors.UserNotFoundError);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetChatByAnotherUser(Guid userId)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user?.ApplicationUserInfo != null)
            {
                return Ok(await _chatService.GetChatByAnotherUser(user.ApplicationUserInfo.Value, userId));
            }
            return NotFound(Constants.Errors.UserNotFoundError);
        }

        [HttpGet]
        public async Task<IActionResult> GetChats()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user?.ApplicationUserInfo != null)
            {
                return Ok(await _chatService.GetChats(user.ApplicationUserInfo.Value));
            }
            return NotFound(Constants.Errors.UserNotFoundError);
        }

        [HttpGet("page")]
        public async Task<IActionResult> GetChats(DateTimeOffset date, int numberToTake)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user?.ApplicationUserInfo != null)
                return Ok(await _chatService.GetChats(user.ApplicationUserInfo.Value, date, numberToTake));
            return NotFound(Constants.Errors.UserNotFoundError);
        }

        [HttpGet("search")]
        public async Task<IActionResult> GetChats(string keyword)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user?.ApplicationUserInfo != null)
            {
                return Ok(await _chatService.GetChatsByName(user.ApplicationUserInfo.Value, keyword));
            }
            return NotFound(Constants.Errors.UserNotFoundError);
        }

        [HttpGet("search/page")]
        public async Task<IActionResult> GetChats(string keyword, DateTimeOffset date, int numberToTake)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user?.ApplicationUserInfo != null)
                return Ok(await _chatService.GetChatsByName(user.ApplicationUserInfo.Value, keyword, date, numberToTake));
            return NotFound(Constants.Errors.UserNotFoundError);
        }

        [HttpGet("new")]
        public async Task<IActionResult> GetNewChats(DateTimeOffset date, int numberToTake)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user?.ApplicationUserInfo != null)
                return Ok(await _chatService.GetNewChats(user.ApplicationUserInfo.Value, date, numberToTake));
            return NotFound(Constants.Errors.UserNotFoundError);
        }

        [HttpPost]
        public async Task<IActionResult> CreateChat([FromBody] Chat chat)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user != null)
            {
                try
                {
                    if (user.ApplicationUserInfo != null)
                    {
                        chat.Id = Guid.NewGuid();
                        return Ok(await _chatService.CreateChat(user.ApplicationUserInfo.Value, chat));
                    }
                }
                catch (Exception e)
                {
                    if (e is ChatNotSufficientNumberOfUsersException)
                    {
                        return BadRequest(e.Message);
                    }

                    if (e is UserNotFoundException)
                    {
                        return NotFound(e.Message);
                    }

                    return BadRequest(e.Message);
                }
            }
            return NotFound(Constants.Errors.UserNotFoundError);
        }
    }
}