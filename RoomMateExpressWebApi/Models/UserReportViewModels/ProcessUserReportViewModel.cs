using System;
using System.ComponentModel.DataAnnotations;

namespace RoomMateExpressWebApi.Models.UserReportViewModels
{
    public class ProcessUserReportViewModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Decision { get; set; }
    }
}
