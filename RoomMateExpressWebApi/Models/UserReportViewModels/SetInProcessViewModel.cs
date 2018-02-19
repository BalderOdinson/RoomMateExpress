using System;
using System.ComponentModel.DataAnnotations;

namespace RoomMateExpressWebApi.Models.UserReportViewModels
{
    public class SetInProcessViewModel
    {
        [Required]
        public Guid Id { get; set; }
    }
}
