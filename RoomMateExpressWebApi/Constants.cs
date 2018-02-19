using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace RoomMateExpressWebApi
{
    public static class Constants
    {
        public static class Authorization
        {
            public const string AdministratorRole = "Administrator";
            public const string UserRole = "User";
            public const string AdministratorPolicy = "AdministratorPolicy";
            public const string UserPolicy = "UserPolicy";
            public const string UserAndAdministratorPolicy = "UserAndAdministratorPolicy";
            public const string BlockedUser = "BlockedUser";
            public const string BlockedUserPolicy = "BlockedUserPolicy";
        }
        public static class Success
        {
            public const string ConfirmEmailOk = "Hvala vam što ste potvrdili Vašu email adresu.";
        }

        public static class Errors
        {
            public const string ArgumentNullError = "argumentNull";
            public const string UserNotFoundError = "userNotFound";
            public const string AdminNotFoundError = "adminNotFound";
            public const string DuplicateEmailAddressError = "duplicateEmailAddress";
            public const string PasswordResetInProcessError = "passwordResetInProcess";
            public const string UnblockingNonBlockedUserError = "userNotBlocked";
            public const string EmailConfirmError = "Dogodila se greška, pokušajte ponovno.";
            public const string AdminNullError = "adminNoInfo";
            public const string UserReportNotFoundError = "reportNotFound";
            public const string InvalidInput = "invalidInput";
            public const string RegistrationFail = "registrationFail";
            public const string OperationError = "operationError";
            public const string UsernamePasswordInvalid = "usernamePasswordInvalid";
            public const string UserBlockedError = "userBlocked";
            public const string LoginAttemptsExceededError = "loginAttemptsExceeded";
            public const string RefreshTokenExpired = "refreshTokenExpired";
            public const string LoginRequired = "loginRequired";
            public const string UnsupportedGrantType = "unsupportedGrantType";
            public const string PostNotFound = "postNotFound";
        }

        public static class FirebaseNotificaton
        {
            public const string MessageClickActionAndroid = "com.roommateexpress.android.MESSAGE";
            public const string MessageIconAndroid = "message_processing";
            public const string PasswordResetTitle = "Reset password";
            public const string PasswordResetBody = "Click here to reset your password...";
            public const string PasswordResetTitleLocAndroid = "resetPassword";
            public const string PasswordResetBodyLocAndroid = "clickForReset";
            public const string PasswordResetClickActionAndroid = "com.roommateexpress.android.RESET";
            public const string PasswordResetTagAndroid = "PasswordReset";
            public const string PasswordResetIconAndroid = "lock_reset";
            public const string RoommateRequestTitle = "Request for roommate";
            public const string RoommateRequestBody = " is sending you a request.";
            public const string RoommateRequestTitleLocAndroid = "requestRoommate";
            public const string RoommateRequestBodyLocAndroid = "roommateRequest";
            public const string RoommateRequestClickActionAndroid = "com.roommateexpress.android.PROFILE";
            public const string RoommateRequestIconAndroid = "account_plus";
            public const string DefaultSoundAndroid = "default";
            public static readonly Color DefaultColorAndroid = Color.FromArgb(0x97, 0xbb, 0xf4);
            public const string RestrictedPackageNameAndroid = "com.roommateexpress.android";
        }
    }
}
