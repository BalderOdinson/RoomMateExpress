using System.ComponentModel.DataAnnotations;

namespace RoomMateExpressWebApi.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string EmailAddress { get; set; }

        [Required]
        [Display(Name = "Device token")]
        public string DeviceToken { get; set; }
    }
}
