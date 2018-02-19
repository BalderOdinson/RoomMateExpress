using System;
using System.Collections.Generic;
using System.Text;

namespace RoomMateExpress.Core.Settings
{
    public class UserData
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string RefreshToken { get; set; }
        public bool AreNotificationsOn { get; set; }
        public bool IsLogedIn { get; set; }
        public List<SearchHistory> SearchHistories { get; set; }
    }
}
