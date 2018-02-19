using RoomMateExpress.Core.Models;

namespace RoomMateExpress.Core.ApiModels
{
    public class LoginResult
    {
        public Admin Admin { get; set; }
        public User User { get; set; }
        public bool IsAdmin { get; set; }
    }
}
