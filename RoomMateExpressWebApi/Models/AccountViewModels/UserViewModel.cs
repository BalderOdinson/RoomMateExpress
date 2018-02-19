using System.ComponentModel.DataAnnotations;

namespace RoomMateExpressWebApi.Models.AccountViewModels
{
    public class UserViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string EmailAddress { get; set; }
    }
}
