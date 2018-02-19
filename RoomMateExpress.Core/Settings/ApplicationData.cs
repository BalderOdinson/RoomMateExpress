using System;
using MvvmCross.Core.ViewModels;
using RoomMateExpress.Core.Helpers.Collections;
using RoomMateExpress.Core.ViewModels;
using RoomMateExpress.Core.ViewModels.BaseViewModels;

namespace RoomMateExpress.Core.Settings
{
    public static class ApplicationData
    {
        public static BaseUserViewModel CurrentUserViewModel { get; set; }
        public static BaseAdminViewModel CurrentAdminViewModel { get; set; }
        public static string RefreshToken { get; set; } = string.Empty;
        public static string AccessToken { get; set; } = string.Empty;
        public static UserData UserData { get; set; }
        public static string FirebaseToken { get; set; } = string.Empty;
        public static DateTimeOffset? TokenExpireTime { get; set; }
        public static bool AreNotificationsOn { get; set; } = true;
        public static object DataBaseLock { get; } = new object();
    }
}
