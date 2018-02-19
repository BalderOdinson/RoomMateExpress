using System;
using System.ComponentModel.DataAnnotations;

namespace RoomMateExpressWebApi.Models.UserReportViewModels
{
    public class ReportUserViewModel
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public string Text { get; set; }
    }
}
