using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomMateExpressWebApi.EF
{
    public static class Constants
    {
        public static class Errors
        {
            public const string ChatNotSufficientNumberOfUsers = "chatNotSufNumOfUsers";
            public const string UserNotFound = "userNotFound";
            public const string PostNotFound = "postNotFound";
            public const string ChatNotFound = "chatNotFound";
            public const string NeighborhoodNotFound = "neighborhoodNotFound";
            public const string DuplicateRequest = "duplicateRequest";
        }

        public static class SqlErrors
        {
            public const string MessageUserNotFound =
                "The INSERT statement conflicted with the FOREIGN KEY constraint \"FK_dbo.Messages_dbo.Users_UserSender_Id\". The conflict occurred in database \"RoomMateExpressDb\", table \"dbo.Users\", column \'Id\'.\r\nThe statement has been terminated.";

            public const string MessageChatNotFound =
                "The INSERT statement conflicted with the FOREIGN KEY constraint \"FK_dbo.Messages_dbo.Chats_Chat_Id\". The conflict occurred in database \"RoomMateExpressDb\", table \"dbo.Chats\", column \'Id\'.\r\nThe statement has been terminated.";

            public const string ProfileCommentUserCommentatorNotFound =
                "The INSERT statement conflicted with the FOREIGN KEY constraint \"FK_dbo.ProfileComments_dbo.Users_UserCommentator_Id\". The conflict occurred in database \"RoomMateExpressDb\", table \"dbo.Users\", column \'Id\'.\r\nThe statement has been terminated.";

            public const string ProfileCommentUserProfileNotFound =
                "The INSERT statement conflicted with the FOREIGN KEY constraint \"FK_dbo.ProfileComments_dbo.Users_UserProfile_Id\". The conflict occurred in database \"RoomMateExpressDb\", table \"dbo.Users\", column \'Id\'.\r\nThe statement has been terminated.";

            public const string PostUserNotFound =
                "The INSERT statement conflicted with the FOREIGN KEY constraint \"FK_dbo.Posts_dbo.Users_User_Id\". The conflict occurred in database \"RoomMateExpressDb\", table \"dbo.Users\", column \'Id\'.\r\nThe statement has been terminated.";

            public const string PostNeighborhoodNotFound =
                "The INSERT statement conflicted with the FOREIGN KEY constraint \"FK_dbo.NeighborhoodPosts_dbo.Neighborhoods_Neighborhood_Id\". The conflict occurred in database \"RoomMateExpressDb\", table \"dbo.Neighborhoods\", column \'Id\'.\r\nThe statement has been terminated.";

            public const string LikePostNotFound =
                "The INSERT statement conflicted with the FOREIGN KEY constraint \"FK_dbo.PostUsers_dbo.Posts_Post_Id\". The conflict occurred in database \"RoomMateExpressDb\", table \"dbo.Posts\", column \'Id\'.\r\nThe statement has been terminated.";

            public const string LikeUserNotFound =
                "The INSERT statement conflicted with the FOREIGN KEY constraint \"FK_dbo.PostUsers_dbo.Users_User_Id\". The conflict occurred in database \"RoomMateExpressDb\", table \"dbo.Users\", column \'Id\'.\r\nThe statement has been terminated.";

            public const string PostCommentUserNotFound =
                "The INSERT statement conflicted with the FOREIGN KEY constraint \"FK_dbo.CommentForPosts_dbo.Users_User_Id\". The conflict occurred in database \"RoomMateExpressDb\", table \"dbo.Users\", column \'Id\'.\r\nThe statement has been terminated.";

            public const string PostCommentPostNotFound =
                "The INSERT statement conflicted with the FOREIGN KEY constraint \"FK_dbo.CommentForPosts_dbo.Posts_Post_Id\". The conflict occurred in database \"RoomMateExpressDb\", table \"dbo.Posts\", column \'Id\'.\r\nThe statement has been terminated.";
        }
    }
}
