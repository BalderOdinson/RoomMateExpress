using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RoomMateExpressWebApi.EF.Models
{
    public class Admin : Entity
    {
        [Required]
        [StringLength(maximumLength: 35, MinimumLength = 3)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(maximumLength: 35, MinimumLength = 3)]
        public string LastName { get; set; }

        public List<UserReport> UserReports { get; set; }

        public Admin()
        {
            UserReports = new List<UserReport>();
        }

        public DateTimeOffset CreationDate { get; set; }

        public Guid AccountId { get; set; }
    }
}
