using MvvmCross.Plugins.Messenger;

namespace RoomMateExpress.Core.Helpers.MvxMessengerHelpers
{
    public class PictureMessage : MvxMessage
    {
        public string PictureUrl { get; }
        public string Error { get; }
        public bool Success { get; }

        public PictureMessage(object sender, string url, string error, bool success) : base(sender)
        {
            PictureUrl = url;
            Error = error;
            Success = success;
        }
    }
}
