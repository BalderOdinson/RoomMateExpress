using System.ComponentModel.DataAnnotations;

namespace RoomMateExpressWebApi.Models.AdminViewModels
{
    public class AdminViewModel
    {
        [Required]
        [StringLength(maximumLength: 20, MinimumLength = 3)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(maximumLength: 20, MinimumLength = 3)]
        public string LastName { get; set; }
    }

}
