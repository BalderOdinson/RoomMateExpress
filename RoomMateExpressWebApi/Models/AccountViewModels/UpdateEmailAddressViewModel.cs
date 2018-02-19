using System.ComponentModel.DataAnnotations;

namespace RoomMateExpressWebApi.Models.AccountViewModels
{
    public class UpdateEmailAddressViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "OldEmail")]
        public string OldEmailAddress { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "NewEmail")]
        public string NewEmailAddress { get; set; }
    }
}
