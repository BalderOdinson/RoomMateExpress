using System;
using System.Collections.Generic;
using System.Text;

namespace RoomMateExpress.Core
{
    public static class Constants
    {
        public static class Pagination
        {
            public const int InitialCount = 20;
            public const int RequestMoreCount = 10;
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
            public const string OperationCanceled = "operationCanceled";
        }
    }
}
