using System;
using System.ComponentModel.DataAnnotations;

namespace RoomMateExpressWebApi.EF.Models
{
    public class UserReport : Entity
    {
        [Required]
        public string Text { get; set; }

        public User UserReporting { get; set; }

        public User UserReported { get; set; }

        public Admin Admin { get; set; }

        public DateTimeOffset DateReporting { get; set; }

        public DateTimeOffset? DateProcessed { get; set; }

        public string AdminDecision { get; set; } 

        public bool IsProcessed => DateProcessed.HasValue;
    }
}
