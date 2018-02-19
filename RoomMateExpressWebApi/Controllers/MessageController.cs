using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FirebaseCloudMessagingApi;
using FirebaseCloudMessagingApi.Helpers;
using FirebaseCloudMessagingApi.Models;
using FirebaseCloudMessagingApi.Models.Android;
using FirebaseCloudMessagingApi.Models.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoomMateExpressWebApi.EF.Exceptions;
using RoomMateExpressWebApi.EF.Models;
using RoomMateExpressWebApi.EF.Services;
using RoomMateExpressWebApi.Helpers;
using RoomMateExpressWebApi.Services;

namespace RoomMateExpressWebApi.Controllers
{
    [Produces("application/json")]
    [Route("message")]
    public class MessageController : Controller
    {
        private readonly IMessageService _messageService;
        private readonly IFirebaseCloudMessagingService _firebaseCloudMessagingService;

        public MessageController(IMessageService messageService, IFirebaseCloudMessagingService firebaseCloudMessagingService)
        {
            _messageService = messageService;
            _firebaseCloudMessagingService = firebaseCloudMessagingService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMessage(Guid id)
        {
            return Ok(await _messageService.GetMessage(id));
        }

        [HttpGet("chat/{chatId}")]
        public async Task<IActionResult> GetMessages(Guid chatId)
        {
            return Ok(await _messageService.GetMessages(chatId));
        }

        [HttpGet("chat/{chatId}/page")]
        public async Task<IActionResult> GetMessages(Guid chatId, DateTimeOffset date, int numberToTake)
        {
            return Ok(await _messageService.GetMessages(chatId, date, numberToTake));
        }

        [HttpGet("chat/{chatId}/new/page")]
        public async Task<IActionResult> GetNewMessages(Guid chatId, DateTimeOffset date, int numberToTake)
        {
            return Ok(await _messageService.GetNewMessages(chatId, date, numberToTake));
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody]Message message)
        {
            try
            {
                var result = await _messageService.SendMessage(message);
                foreach (var user in message.Chat.Users.Where(u => u.Id != message.UserSender.Id))
                {
                    await _firebaseCloudMessagingService.SendNotification(new TopicMessage
                    {
                        Android = new AndroidConfig
                        {
                            Notification = new AndroidNotification
                            {
                                Title = message.Chat.Users.Any(u => u.FirstName.Equals(message.Chat.Name)) 
                                    ? $"{message.UserSender.FirstName} {message.UserSender.LastName}" 
                                    : message.Chat.Name,
                                Body = message.Text,
                                ClickAction =
                                        Constants.FirebaseNotificaton.MessageClickActionAndroid,
                                Tag = message.Chat.Id.ToString(),
                                Color =
                                        Constants.FirebaseNotificaton.DefaultColorAndroid
                                            .ToFcmColor(),
                                Icon = Constants.FirebaseNotificaton.MessageIconAndroid,
                                Sound = Constants.FirebaseNotificaton.DefaultSoundAndroid
                            },
                            Priority = AndroidMessagePriority.Normal,
                            RestrictedPackageName =
                                    Constants.FirebaseNotificaton.RestrictedPackageNameAndroid,
                            Ttl = FirebaseCloudMessagingApiConstants.MaxTtl
                        },
                        Data = new Dictionary<string, string>
                            {
                                {"ChatId", message.Chat.Id.ToString() },
                                {"MessageId", message.Id.ToString()}
                            },
                        Notification = new Notification
                        {
                            Title = message.Chat.Name,
                            Body = message.Text
                        },
                        Topic = user.Id.ToString()
                    },
                        false);
                }

                return Ok(result);
            }
            catch (Exception e)
            {
                if (e is ChatNotFoundException || e is UserNotFoundException)
                    return NotFound(e.Message);
                return BadRequest(e.Message);
            }
        }
    }
}