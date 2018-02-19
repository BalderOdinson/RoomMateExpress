namespace RoomMateExpress.Core.ApiModels
{
    public struct AccountModel
    {
        public string EmailAddress { get; }
        public string Password { get; }
        public string ConfirmPassword { get; }
        public string Reason { get; }
        public string DeviceToken { get; }
        public string ResetCode { get; }
        public string OldEmailAddress { get; }
        public string NewEmailAddress { get; }
        public string OldPassword { get; }
        public string NewPassword { get; }
        public string ConfirmNewPassword { get; }

        private AccountModel(string email = null,
            string password = null,
            string confirmPassword = null,
            string reason = null,
            string device = null,
            string reset = null,
            string oldEmail = null,
            string newEmail = null,
            string oldPassword = null,
            string newPassword = null,
            string confirmNewPassword = null)
        {
            EmailAddress = email;
            Password = password;
            ConfirmPassword = confirmPassword;
            Reason = reason;
            DeviceToken = device;
            ResetCode = reset;
            OldEmailAddress = oldEmail;
            NewEmailAddress = newEmail;
            OldPassword = oldPassword;
            NewPassword = newPassword;
            ConfirmNewPassword = confirmNewPassword;
        }

        public static AccountModel BlockUser(string reason)
        {
            return new AccountModel(reason: reason);
        }

        public static AccountModel RegisterUser(string email, string password, string confirmPassword)
        {
            return new AccountModel(email: email, password: password, confirmPassword: confirmPassword);
        }

        public static AccountModel ForgotPassword(string email, string deviceToken)
        {
            return new AccountModel(email: email, device: deviceToken);
        }

        public static AccountModel ResetPassword(string email, string resetCode, string password, string confirmPassword)
        {
            return new AccountModel(email: email, reset: resetCode, password: password, confirmPassword: confirmPassword);
        }

        public static AccountModel UpdateEmail(string oldEmail, string newEmail)
        {
            return new AccountModel(oldEmail: oldEmail, newEmail: newEmail);
        }

        public static AccountModel UpdatePassword(string email, string oldPassword, string newPassword, string confrimNewPassword)
        {
            return new AccountModel(email: email, oldPassword: oldPassword, newPassword: newPassword, confirmPassword: confrimNewPassword);
        }
    }
}
