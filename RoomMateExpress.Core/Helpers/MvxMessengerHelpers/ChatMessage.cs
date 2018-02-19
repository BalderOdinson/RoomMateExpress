using System;
using System.Collections.Generic;
using System.Text;
using MvvmCross.Plugins.Messenger;
using RoomMateExpress.Core.Models;

namespace RoomMateExpress.Core.Helpers.MvxMessengerHelpers
{
    public class ChatMessage : MvxMessage
    {
        public ChatMessage(object sender, Guid chatId, Guid messageId) : base(sender)
        {
            ChatId = chatId;
            MessageId = messageId;
        }

        public Guid ChatId { get; }

        public Guid MessageId { get; }
    }
}
