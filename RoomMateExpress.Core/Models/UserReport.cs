using System;

namespace RoomMateExpress.Core.Models
{
    public class UserReport : Entity
    {
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
