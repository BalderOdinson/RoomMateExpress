using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RoomMateExpressWebApi.Helpers;

namespace RoomMateExpressWebApi.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message, bool isImportant);
    }
}
