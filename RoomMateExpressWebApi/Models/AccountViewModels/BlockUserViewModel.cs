using System.ComponentModel.DataAnnotations;

namespace RoomMateExpressWebApi.Models.AccountViewModels
{
    public class BlockUserViewModel
    {
        [Required]
        [Display(Name = "Reason for blocking")]
        public string Reason { get; set; }
    }
}
