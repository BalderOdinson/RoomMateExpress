using System.Collections.Generic;

namespace RoomMateExpress.Core.Models
{
    public class Admin : Entity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public List<UserReport> UserReports { get; set; }

        public Admin()
        {
            UserReports = new List<UserReport>();
        }
    }
}
