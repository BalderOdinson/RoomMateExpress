using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace RoomMateExpressWebApi.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public Guid? ApplicationUserInfo { get; set; }

        public string DeviceForPasswordReset { get; set; }

        public DateTimeOffset DevicePasswordResetExpires { get; set; }
    }
}
