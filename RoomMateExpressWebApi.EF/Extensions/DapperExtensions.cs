using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using RoomMateExpressWebApi.EF.Exceptions;
using RoomMateExpressWebApi.EF.Models;
using RoomMateExpressWebApi.EF.ViewModels;

namespace RoomMateExpressWebApi.EF.Extensions
{
    public static class DapperExtensions
    {
        public static Task<UserViewModel> GetUserAsync(this RoomMateExpressDbContext context, Guid id)
        {
            const string getUserSql = "SELECT Users.Id, " +
                                        "Users.AccountId, " +
                                        "Users.BirthDate, " +
                                        "YEAR (CURRENT_TIMESTAMP) - YEAR(Users.BirthDate) AS Age, " +
                                        "Users.DescriptionOfStudyOrWork, " +
                                        "Users.FirstName, " +
                                        "Users.Gender, " +
                                        "Users.HasFaculty, " +
                                        "Users.IsSmoker, " +
                                        "Users.LastName, " +
                                        "Users.CreationDate, " +
                                        "Users.ProfilePictureUrl, " +
                                        "ISNULL(CAST(AVG(CAST(Comments.Grade AS Decimal(2,1))) AS Decimal(2,1)), 0) AS AverageGrade, " +
                                        "COUNT(Comments.UserProfile_Id) AS CommentsOnProfileCount " +
                                    "FROM Users " +
                                    "LEFT JOIN (SELECT ProfileComments.UserProfile_Id, ProfileComments.Grade " +
                                        "FROM ProfileComments) Comments " +
                                    "ON Users.Id = Comments.UserProfile_Id " +
                                    "WHERE Users.Id = @UserId " +
                                    "GROUP BY Id, AccountId, BirthDate, DescriptionOfStudyOrWork, FirstName, Gender, HasFaculty, IsSmoker, LastName, CreationDate, ProfilePictureUrl;";
            var connection = context.Database.Connection as SqlConnection;
            return connection.QueryFirstOrDefaultAsync<UserViewModel>(getUserSql, new { UserId = id });
        }

        public static Task<IEnumerable<UserViewModel>> GetUsersAsync(this RoomMateExpressDbContext context)
        {
            const string getUserSql = "SELECT Users.Id, " +
                                        "Users.AccountId, " +
                                        "Users.BirthDate, " +
                                        "YEAR (CURRENT_TIMESTAMP) - YEAR(Users.BirthDate) AS Age, " +
                                        "Users.DescriptionOfStudyOrWork, " +
                                        "Users.FirstName, " +
                                        "Users.Gender, " +
                                        "Users.HasFaculty, " +
                                        "Users.IsSmoker, " +
                                        "Users.LastName, " +
                                        "Users.CreationDate, " +
                                        "Users.ProfilePictureUrl, " +
                                        "ISNULL(CAST(AVG(CAST(Comments.Grade AS Decimal(2,1))) AS Decimal(2,1)), 0) AS AverageGrade, " +
                                        "COUNT(Comments.UserProfile_Id) AS CommentsOnProfileCount " +
                                      "FROM Users " +
                                      "LEFT JOIN (SELECT ProfileComments.UserProfile_Id, ProfileComments.Grade " +
                                        "FROM ProfileComments) Comments " +
                                      "ON Users.Id = Comments.UserProfile_Id " +
                                      "GROUP BY Id, AccountId, BirthDate, DescriptionOfStudyOrWork, FirstName, Gender, HasFaculty, IsSmoker, LastName, CreationDate, ProfilePictureUrl;";
            var connection = context.Database.Connection as SqlConnection;
            return connection.QueryAsync<UserViewModel>(getUserSql);
        }

        public static Task<IEnumerable<UserViewModel>> GetUsersAsync(this RoomMateExpressDbContext context, Guid currentUser, string keyword)
        {
            var getUserSql = "SELECT Users.Id, " +
                                        "Users.AccountId, " +
                                        "Users.BirthDate, " +
                                        "YEAR (CURRENT_TIMESTAMP) - YEAR(Users.BirthDate) AS Age, " +
                                        "Users.DescriptionOfStudyOrWork, " +
                                        "Users.FirstName, " +
                                        "Users.Gender, " +
                                        "Users.HasFaculty, " +
                                        "Users.IsSmoker, " +
                                        "Users.LastName, " +
                                        "Users.CreationDate, " +
                                        "Users.ProfilePictureUrl, " +
                                        "ISNULL(CAST(AVG(CAST(Comments.Grade AS Decimal(2,1))) AS Decimal(2,1)), 0) AS AverageGrade, " +
                                        "COUNT(Comments.UserProfile_Id) AS CommentsOnProfileCount " +
                                      "FROM Users " +
                                        "LEFT JOIN (SELECT ProfileComments.UserProfile_Id, ProfileComments.Grade " +
                                        "FROM ProfileComments) Comments " +
                                      "ON Users.Id = Comments.UserProfile_Id " +
                                      "WHERE Users.Id <> @UserId " +
                                      (keyword == null ? string.Empty : "AND LOWER(Users.FirstName + ' ' + Users.LastName) LIKE '%' + LOWER(@Keyword) + '%' ") +
                                      "GROUP BY Id, AccountId, BirthDate, DescriptionOfStudyOrWork, FirstName, Gender, HasFaculty, IsSmoker, LastName, CreationDate, ProfilePictureUrl;";
            var connection = context.Database.Connection as SqlConnection;
            return connection.QueryAsync<UserViewModel>(getUserSql, new { UserId = currentUser, Keyword = keyword });
        }

        public static Task<IEnumerable<UserViewModel>> GetUsersAsync(this RoomMateExpressDbContext context, DateTimeOffset date,
            int numberToTake)
        {
            const string getUserSql = "SELECT Users.Id, " +
                                        "Users.AccountId, " +
                                        "Users.BirthDate, " +
                                        "YEAR (CURRENT_TIMESTAMP) - YEAR(Users.BirthDate) AS Age, " +
                                        "Users.DescriptionOfStudyOrWork, " +
                                        "Users.FirstName, " +
                                        "Users.Gender, " +
                                        "Users.HasFaculty, " +
                                        "Users.IsSmoker, " +
                                        "Users.LastName, " +
                                        "Users.CreationDate, " +
                                        "Users.ProfilePictureUrl, " +
                                        "ISNULL(CAST(AVG(CAST(Comments.Grade AS Decimal(2,1))) AS Decimal(2,1)), 0) AS AverageGrade, " +
                                        "COUNT(Comments.UserProfile_Id) AS CommentsOnProfileCount " +
                                      "FROM Users " +
                                        "LEFT JOIN (SELECT ProfileComments.UserProfile_Id, ProfileComments.Grade " +
                                        "FROM ProfileComments) Comments " +
                                      "ON Users.Id = Comments.UserProfile_Id " +
                                      "WHERE Users.CreationDate < @DateParam " +
                                      "GROUP BY Id, AccountId, BirthDate, DescriptionOfStudyOrWork, FirstName, Gender, HasFaculty, IsSmoker, LastName, CreationDate, ProfilePictureUrl " +
                                      "ORDER BY Users.CreationDate DESC " +
                                      "OFFSET 0 ROWS " +
                                      "FETCH FIRST @NumberToTake ROWS ONLY;";
            var connection = context.Database.Connection as SqlConnection;
            return connection.QueryAsync<UserViewModel>(getUserSql, new { DateParam = date, NumberToTake = numberToTake });
        }

        public static Task<IEnumerable<UserViewModel>> GetUsersAsync(this RoomMateExpressDbContext context, Guid currentUserId, DateTimeOffset date,
            int numberToTake, string keyword)
        {
            var getUserSql = "SELECT Users.Id, " +
                                        "Users.AccountId, " +
                                        "Users.BirthDate, " +
                                        "YEAR (CURRENT_TIMESTAMP) - YEAR(Users.BirthDate) AS Age, " +
                                        "Users.DescriptionOfStudyOrWork, " +
                                        "Users.FirstName, " +
                                        "Users.Gender, " +
                                        "Users.HasFaculty, " +
                                        "Users.IsSmoker, " +
                                        "Users.LastName, " +
                                        "Users.CreationDate, " +
                                        "Users.ProfilePictureUrl, " +
                                        "ISNULL(CAST(AVG(CAST(Comments.Grade AS Decimal(2,1))) AS Decimal(2,1)), 0) AS AverageGrade, " +
                                        "COUNT(Comments.UserProfile_Id) AS CommentsOnProfileCount " +
                                      "FROM Users " +
                                        "LEFT JOIN (SELECT ProfileComments.UserProfile_Id, ProfileComments.Grade " +
                                        "FROM ProfileComments) Comments " +
                                      "ON Users.Id = Comments.UserProfile_Id " +
                                      "WHERE Users.CreationDate < @DateParam " +
                                      "AND Users.Id <> @UserId " +
                                      (keyword == null ? string.Empty : "AND LOWER(Users.FirstName + ' ' + Users.LastName) LIKE '%' + LOWER(@Keyword) + '%' ") +
                                      "GROUP BY Id, AccountId, BirthDate, DescriptionOfStudyOrWork, FirstName, Gender, HasFaculty, IsSmoker, LastName, CreationDate, ProfilePictureUrl " +
                                      "ORDER BY Users.CreationDate DESC " +
                                      "OFFSET 0 ROWS " +
                                      "FETCH FIRST @NumberToTake ROWS ONLY;";
            var connection = context.Database.Connection as SqlConnection;
            return connection.QueryAsync<UserViewModel>(getUserSql, new { UserId = currentUserId, DateParam = date, NumberToTake = numberToTake, Keyword = keyword });
        }

        public static Task<IEnumerable<UserViewModel>> GetRoommatesAsync(this RoomMateExpressDbContext context, Guid currentUserId)
        {
            const string getUserSql = "SELECT Users.Id, " +
                                        "Users.AccountId, " +
                                        "Users.BirthDate, " +
                                        "YEAR (CURRENT_TIMESTAMP) - YEAR(Users.BirthDate) AS Age, " +
                                        "Users.DescriptionOfStudyOrWork, " +
                                        "Users.FirstName, " +
                                        "Users.Gender, " +
                                        "Users.HasFaculty, " +
                                        "Users.IsSmoker, " +
                                        "Users.LastName, " +
                                        "Users.CreationDate, " +
                                        "Users.ProfilePictureUrl, " +
                                        "ISNULL(CAST(AVG(CAST(Comments.Grade AS Decimal(2,1))) AS Decimal(2,1)), 0) AS AverageGrade, " +
                                        "COUNT(Comments.UserProfile_Id) AS CommentsOnProfileCount " +
                                      "FROM UserUsers " +
                                      "JOIN Users ON UserUsers.User_Id = Users.Id " +
                                      "LEFT JOIN (SELECT ProfileComments.UserProfile_Id, ProfileComments.Grade " +
                                        "FROM ProfileComments) Comments " +
                                      "ON Users.Id = Comments.UserProfile_Id " +
                                      "WHERE UserUsers.User_Id1 = @UserId " +
                                      "GROUP BY Id, AccountId, BirthDate, DescriptionOfStudyOrWork, FirstName, Gender, HasFaculty, IsSmoker, LastName, CreationDate, ProfilePictureUrl " +

                                      "UNION " +

                                      "SELECT Users.Id, " +
                                        "Users.AccountId, " +
                                        "Users.BirthDate, " +
                                        "YEAR (CURRENT_TIMESTAMP) - YEAR(Users.BirthDate) AS Age, " +
                                        "Users.DescriptionOfStudyOrWork, " +
                                        "Users.FirstName, " +
                                        "Users.Gender, " +
                                        "Users.HasFaculty, " +
                                        "Users.IsSmoker, " +
                                        "Users.LastName, " +
                                        "Users.CreationDate, " +
                                        "Users.ProfilePictureUrl, " +
                                        "ISNULL(CAST(AVG(CAST(Comments.Grade AS Decimal(2,1))) AS Decimal(2,1)), 0) AS AverageGrade, " +
                                        "COUNT(Comments.UserProfile_Id) AS CommentsOnProfileCount " +
                                      "FROM UserUsers " +
                                      "JOIN Users ON UserUsers.User_Id1 = Users.Id " +
                                      "LEFT JOIN (SELECT ProfileComments.UserProfile_Id, ProfileComments.Grade " +
                                        "FROM ProfileComments) Comments " +
                                      "ON Users.Id = Comments.UserProfile_Id " +
                                      "WHERE UserUsers.User_Id = @UserId " +
                                      "GROUP BY Id, AccountId, BirthDate, DescriptionOfStudyOrWork, FirstName, Gender, HasFaculty, IsSmoker, LastName, CreationDate, ProfilePictureUrl;";
            var connection = context.Database.Connection as SqlConnection;
            return connection.QueryAsync<UserViewModel>(getUserSql, new { UserId = currentUserId });
        }

        public static async Task<IEnumerable<ChatViewModel>> GetChatsAsync(this RoomMateExpressDbContext context, Guid currentUserId)
        {
            const string sql = "SELECT Chats.Id, " +
                                "Chats.Name,  " +
                                "Chats.PictureUrl, " +
                                "Chats.LastModified, " +
                                "Users.Id, " +
                                "Users.FirstName, " +
                                "Users.LastName, " +
                                "Users.ProfilePictureUrl, " +
                                "ChatUsers.Chat_Id AS ChatId, " +
                                "LastMessage.Id, " +
                                "LastMessage.PictureUrl, " +
                                "LastMessage.PictureUrl, " +
                                "LastMessage.RecievedAt, " +
                                "LastMessage.SeenAt, " +
                                "LastMessage.SentAt, " +
                                "LastMessage.Text, " +
                                "LastMessage.UserSender_Id AS UserSenderId, " +
                                "UserSender.Id, " +
                                "UserSender.FirstName, " +
                                "UserSender.LastName, " +
                                "UserSender.ProfilePictureUrl " +
                               "FROM Chats " +
                                "INNER JOIN ChatUsers " +
                                    "INNER JOIN Users " +
                                    "ON Users.Id = ChatUsers.User_Id " +
                                "ON Chats.Id = ChatUsers.Chat_Id " +
                               "LEFT JOIN (SELECT Messages.* FROM " +
                                "(SELECT Messages.*, ROW_NUMBER() OVER(PARTITION BY Messages.Chat_Id ORDER BY Messages.SentAt desc) AS rn FROM Messages) AS Messages " +
                                    "WHERE rn = 1) AS LastMessage " +
                                "INNER JOIN Users AS UserSender " +
                                    "ON UserSender.Id = LastMessage.UserSender_Id " +
                               "ON LastMessage.Chat_Id = Chats.Id " +
                               "WHERE Chats.Id IN (SELECT CU.Chat_Id " +
                                "FROM ChatUsers AS CU " +
                                "WHERE CU.User_Id = @UserId) " +
                               "ORDER BY Chats.LastModified;";
            var connection = context.Database.Connection as SqlConnection;
            var chatDictionary = new Dictionary<Guid, ChatViewModel>();
            return (await connection.QueryAsync<ChatViewModel, User, Message, User, ChatViewModel>(sql, (chat, user, message, user1) =>
            {
                if (!chatDictionary.TryGetValue(chat.Id, out var outChat))
                {
                    outChat = chat;
                    chatDictionary.Add(outChat.Id, outChat);
                }
                if (message != null && outChat.LastMessage == null)
                {
                    message.UserSender = user1;
                    outChat.LastMessage = message;
                }
                if (!outChat.Users.Contains(user))
                    outChat.Users.Add(user);
                return outChat;
            }, new { UserId = currentUserId })).Distinct().Select(model =>
            {
                model.Name = model.Name ?? string.Join(", ", model.Users.Where(u => u.Id != currentUserId).Select(u => u.FirstName));
                if (model.Users.Count == 2)
                    model.PictureUrl = model.PictureUrl ?? model.Users.FirstOrDefault(u => u.Id != currentUserId)?.ProfilePictureUrl;
                return model;
            });
        }

        public static async Task<IEnumerable<ChatViewModel>> GetChatsAsync(this RoomMateExpressDbContext context, Guid currentUserId, DateTimeOffset date, int numberToTake)
        {
            const string sql = "SELECT Chats.Id, " +
                                "Chats.Name, " +
                                "Chats.PictureUrl, " +
                                "Chats.LastModified, " +
                                "Users.Id, " +
                                "Users.FirstName, " +
                                "Users.LastName, " +
                                "Users.ProfilePictureUrl, " +
                                "ChatUsers.Chat_Id AS ChatId, " +
                                "LastMessage.Id, " +
                                "LastMessage.PictureUrl, " +
                                "LastMessage.PictureUrl, " +
                                "LastMessage.RecievedAt, " +
                                "LastMessage.SeenAt, " +
                                "LastMessage.SentAt, " +
                                "LastMessage.Text, " +
                                "LastMessage.UserSender_Id AS UserSenderId, " +
                                "UserSender.Id, " +
                                "UserSender.FirstName, " +
                                "UserSender.LastName, " +
                                "UserSender.ProfilePictureUrl " +
                               "FROM Chats " +
                               "INNER JOIN ChatUsers " +
                                "INNER JOIN Users " +
                                "ON Users.Id = ChatUsers.User_Id " +
                               "ON Chats.Id = ChatUsers.Chat_Id " +
                               "LEFT JOIN (SELECT Messages.* FROM " +
                                "(SELECT Messages.*, ROW_NUMBER() OVER(PARTITION BY Messages.Chat_Id ORDER BY Messages.SentAt desc) AS rn FROM Messages) AS Messages " +
                                "WHERE rn = 1) AS LastMessage " +
                                "INNER JOIN Users AS UserSender " +
                                "ON UserSender.Id = LastMessage.UserSender_Id " +
                               "ON LastMessage.Chat_Id = Chats.Id " +
                               "WHERE Chats.Id IN (SELECT Id FROM (SELECT C.Id, DENSE_RANK() OVER(ORDER BY C.LastModified DESC) AS RankNum " +
                                        "FROM Chats AS C " +
                                        "INNER JOIN ChatUsers AS CCH " +
                                            "INNER JOIN Users AS UCH " +
                                            "ON UCH.Id = CCH.User_Id " +
                                        "ON C.Id = CCH.Chat_Id " +
                                        "WHERE C.LastModified < @DateParam " +
                                        "AND C.Id IN (SELECT CU.Chat_Id " +
                                        "FROM ChatUsers AS CU " +
                                        "WHERE CU.User_Id = @UserId)) AS R " +
                                        "WHERE R.RankNum <= @NumberToTake) " +
                               "ORDER BY Chats.LastModified DESC; ";
            var connection = context.Database.Connection as SqlConnection;
            var chatDictionary = new Dictionary<Guid, ChatViewModel>();
            return (await connection.QueryAsync<ChatViewModel, User, Message, User, ChatViewModel>(sql, (chat, user, message, user1) =>
            {
                if (!chatDictionary.TryGetValue(chat.Id, out var outChat))
                {
                    outChat = chat;
                    chatDictionary.Add(outChat.Id, outChat);
                }
                if (message != null && outChat.LastMessage == null)
                {
                    message.UserSender = user1;
                    outChat.LastMessage = message;
                }
                if (!outChat.Users.Contains(user))
                    outChat.Users.Add(user);
                return outChat;
            }, new { UserId = currentUserId, DateParam = date, NumberToTake = numberToTake })).Distinct().Select(model =>
            {
                model.Name = model.Name ?? string.Join(", ", model.Users.Where(u => u.Id != currentUserId).Select(u => u.FirstName));
                if (model.Users.Count == 2)
                    model.PictureUrl = model.PictureUrl ?? model.Users.FirstOrDefault(u => u.Id != currentUserId)?.ProfilePictureUrl;
                return model;
            });
        }

        public static async Task<IEnumerable<ChatViewModel>> GetChatsByNameAsync(this RoomMateExpressDbContext context, Guid currentUserId, string name)
        {
            var sql = "SELECT Chats.Id, " +
                                 "Chats.Name,  " +
                                 "Chats.PictureUrl, " +
                                 "Chats.LastModified, " +
                                 "Users.Id, " +
                                 "Users.FirstName, " +
                                 "Users.LastName, " +
                                 "Users.ProfilePictureUrl, " +
                                 "ChatUsers.Chat_Id AS ChatId, " +
                                 "LastMessage.Id, " +
                                 "LastMessage.PictureUrl, " +
                                 "LastMessage.PictureUrl, " +
                                 "LastMessage.RecievedAt, " +
                                 "LastMessage.SeenAt, " +
                                 "LastMessage.SentAt, " +
                                 "LastMessage.Text, " +
                                 "LastMessage.UserSender_Id AS UserSenderId, " +
                                 "UserSender.Id, " +
                                 "UserSender.FirstName, " +
                                 "UserSender.LastName, " +
                                 "UserSender.ProfilePictureUrl " +
                                "FROM Chats " +
                                 "INNER JOIN ChatUsers " +
                                     "INNER JOIN Users " +
                                     "ON Users.Id = ChatUsers.User_Id " +
                                 "ON Chats.Id = ChatUsers.Chat_Id " +
                                "LEFT JOIN (SELECT Messages.* FROM " +
                                 "(SELECT Messages.*, ROW_NUMBER() OVER(PARTITION BY Messages.Chat_Id ORDER BY Messages.SentAt desc) AS rn FROM Messages) AS Messages " +
                                     "WHERE rn = 1) AS LastMessage " +
                                 "INNER JOIN Users AS UserSender " +
                                     "ON UserSender.Id = LastMessage.UserSender_Id " +
                                "ON LastMessage.Chat_Id = Chats.Id " +
                                "WHERE Chats.Id IN (SELECT CU.Chat_Id " +
                                 "FROM ChatUsers AS CU " +
                                 "INNER JOIN Users AS U " +
                                 "ON CU.User_Id = U.Id " +
                                 "WHERE CU.User_Id = @UserId) " +
                                 (name == null ? string.Empty : "AND Chats.Id IN (SELECT CU.Chat_Id " +
                                                                "FROM ChatUsers AS CU " +
                                                                "INNER JOIN Users AS U " +
                                                                "ON CU.User_Id = U.Id " +
                                                                "WHERE Chats.Name LIKE '%' + @Keyword + '%' OR (U.FirstName + ' ' + U.LastName LIKE '%' + @Keyword + '%' AND UCH.Id <> @UserId)) ") +
                                "ORDER BY Chats.LastModified;";
            var connection = context.Database.Connection as SqlConnection;
            var chatDictionary = new Dictionary<Guid, ChatViewModel>();
            return (await connection.QueryAsync<ChatViewModel, User, Message, User, ChatViewModel>(sql, (chat, user, message, user1) =>
            {
                if (!chatDictionary.TryGetValue(chat.Id, out var outChat))
                {
                    outChat = chat;
                    chatDictionary.Add(outChat.Id, outChat);
                }
                if (message != null && outChat.LastMessage == null)
                {
                    message.UserSender = user1;
                    outChat.LastMessage = message;
                }
                if (!outChat.Users.Contains(user))
                    outChat.Users.Add(user);
                return outChat;
            }, new { UserId = currentUserId, Keyword = name })).Distinct().Select(model =>
            {
                model.Name = model.Name ?? string.Join(", ", model.Users.Where(u => u.Id != currentUserId).Select(u => u.FirstName));
                if (model.Users.Count == 2)
                    model.PictureUrl = model.PictureUrl ?? model.Users.FirstOrDefault(u => u.Id != currentUserId)?.ProfilePictureUrl;
                return model;
            });
        }

        public static async Task<IEnumerable<ChatViewModel>> GetChatsByNameAsync(this RoomMateExpressDbContext context, Guid currentUserId, string name, DateTimeOffset date, int numberToTake)
        {
            var sql = "SELECT Chats.Id, " +
                                "Chats.Name, " +
                                "Chats.PictureUrl, " +
                                "Chats.LastModified, " +
                                "Users.Id, " +
                                "Users.FirstName, " +
                                "Users.LastName, " +
                                "Users.ProfilePictureUrl, " +
                                "ChatUsers.Chat_Id AS ChatId, " +
                                "LastMessage.Id, " +
                                "LastMessage.PictureUrl, " +
                                "LastMessage.PictureUrl, " +
                                "LastMessage.RecievedAt, " +
                                "LastMessage.SeenAt, " +
                                "LastMessage.SentAt, " +
                                "LastMessage.Text, " +
                                "LastMessage.UserSender_Id AS UserSenderId, " +
                                "UserSender.Id, " +
                                "UserSender.FirstName, " +
                                "UserSender.LastName, " +
                                "UserSender.ProfilePictureUrl " +
                               "FROM Chats " +
                               "INNER JOIN ChatUsers " +
                                "INNER JOIN Users " +
                                "ON Users.Id = ChatUsers.User_Id " +
                               "ON Chats.Id = ChatUsers.Chat_Id " +
                               "LEFT JOIN (SELECT Messages.* FROM " +
                                "(SELECT Messages.*, ROW_NUMBER() OVER(PARTITION BY Messages.Chat_Id ORDER BY Messages.SentAt desc) AS rn FROM Messages) AS Messages " +
                                "WHERE rn = 1) AS LastMessage " +
                                "INNER JOIN Users AS UserSender " +
                                "ON UserSender.Id = LastMessage.UserSender_Id " +
                               "ON LastMessage.Chat_Id = Chats.Id " +
                               "WHERE Chats.Id IN (SELECT Id FROM (SELECT C.Id, DENSE_RANK() OVER(ORDER BY C.LastModified DESC) AS RankNum " +
                                        "FROM Chats AS C " +
                                        "INNER JOIN ChatUsers AS CCH " +
                                            "INNER JOIN Users AS UCH " +
                                            "ON UCH.Id = CCH.User_Id " +
                                        "ON C.Id = CCH.Chat_Id " +
                                        "WHERE C.LastModified < @DateParam " +
                                        "AND C.Id IN (SELECT CU.Chat_Id " +
                                        "FROM ChatUsers AS CU " +
                                        "INNER JOIN Users AS U " +
                                        "ON CU.User_Id = U.Id " +
                                        "WHERE CU.User_Id = @UserId) " +
                                        (name == null ? ") AS R " : "AND (C.Name LIKE '%' + @Keyword + '%' OR (UCH.FirstName + ' ' + UCH.LastName LIKE '%' + @Keyword + '%' AND UCH.Id <> @UserId))) AS R ") +
                                        "WHERE R.RankNum <= @NumberToTake) " +
                               "ORDER BY Chats.LastModified DESC; ";
            var connection = context.Database.Connection as SqlConnection;
            var chatDictionary = new Dictionary<Guid, ChatViewModel>();
            return (await connection.QueryAsync<ChatViewModel, User, Message, User, ChatViewModel>(sql, (chat, user, message, user1) =>
            {
                if (!chatDictionary.TryGetValue(chat.Id, out var outChat))
                {
                    outChat = chat;
                    chatDictionary.Add(outChat.Id, outChat);
                }
                if (message != null && outChat.LastMessage == null)
                {
                    message.UserSender = user1;
                    outChat.LastMessage = message;
                }
                if (!outChat.Users.Contains(user))
                    outChat.Users.Add(user);
                return outChat;
            }, new { UserId = currentUserId, Keyword = name, DateParam = date, NumberToTake = numberToTake })).Distinct().Select(model =>
            {
                model.Name = model.Name ?? string.Join(", ", model.Users.Where(u => u.Id != currentUserId).Select(u => u.FirstName));
                if (model.Users.Count == 2)
                    model.PictureUrl = model.PictureUrl ?? model.Users.FirstOrDefault(u => u.Id != currentUserId)?.ProfilePictureUrl;
                return model;
            });
        }

        public static async Task<IEnumerable<ChatViewModel>> GetNewChatsAsync(this RoomMateExpressDbContext context, Guid currentUserId, DateTimeOffset date, int numberToTake)
        {
            const string sql = "SELECT Chats.Id, " +
                                "Chats.Name, " +
                                "Chats.PictureUrl, " +
                                "Chats.LastModified, " +
                                "Users.Id, " +
                                "Users.FirstName, " +
                                "Users.LastName, " +
                                "Users.ProfilePictureUrl, " +
                                "ChatUsers.Chat_Id AS ChatId, " +
                                "LastMessage.Id, " +
                                "LastMessage.PictureUrl, " +
                                "LastMessage.PictureUrl, " +
                                "LastMessage.RecievedAt, " +
                                "LastMessage.SeenAt, " +
                                "LastMessage.SentAt, " +
                                "LastMessage.Text, " +
                                "LastMessage.UserSender_Id AS UserSenderId, " +
                                "UserSender.Id, " +
                                "UserSender.FirstName, " +
                                "UserSender.LastName, " +
                                "UserSender.ProfilePictureUrl " +
                               "FROM Chats " +
                               "INNER JOIN ChatUsers " +
                                "INNER JOIN Users " +
                                "ON Users.Id = ChatUsers.User_Id " +
                               "ON Chats.Id = ChatUsers.Chat_Id " +
                               "LEFT JOIN (SELECT Messages.* FROM " +
                                "(SELECT Messages.*, ROW_NUMBER() OVER(PARTITION BY Messages.Chat_Id ORDER BY Messages.SentAt desc) AS rn FROM Messages) AS Messages " +
                                "WHERE rn = 1) AS LastMessage " +
                                "INNER JOIN Users AS UserSender " +
                                "ON UserSender.Id = LastMessage.UserSender_Id " +
                               "ON LastMessage.Chat_Id = Chats.Id " +
                               "WHERE Chats.Id IN (SELECT Id FROM (SELECT C.Id, DENSE_RANK() OVER(ORDER BY C.LastModified DESC) AS RankNum " +
                                        "FROM Chats AS C " +
                                        "INNER JOIN ChatUsers AS CCH " +
                                            "INNER JOIN Users AS UCH " +
                                            "ON UCH.Id = CCH.User_Id " +
                                        "ON C.Id = CCH.Chat_Id " +
                                        "WHERE C.LastModified > @DateParam " +
                                        "AND C.Id IN (SELECT CU.Chat_Id " +
                                        "FROM ChatUsers AS CU " +
                                        "WHERE CU.User_Id = @UserId)) AS R " +
                                        "WHERE R.RankNum <= @NumberToTake) " +
                               "ORDER BY Chats.LastModified DESC; ";
            var connection = context.Database.Connection as SqlConnection;
            var chatDictionary = new Dictionary<Guid, ChatViewModel>();
            return (await connection.QueryAsync<ChatViewModel, User, Message, User, ChatViewModel>(sql, (chat, user, message, user1) =>
            {
                if (!chatDictionary.TryGetValue(chat.Id, out var outChat))
                {
                    outChat = chat;
                    chatDictionary.Add(outChat.Id, outChat);
                }
                if (message != null && outChat.LastMessage == null)
                {
                    message.UserSender = user1;
                    outChat.LastMessage = message;
                }
                if (!outChat.Users.Contains(user))
                    outChat.Users.Add(user);
                return outChat;
            }, new { UserId = currentUserId, DateParam = date, NumberToTake = numberToTake })).Distinct().Select(model =>
            {
                model.Name = model.Name ?? string.Join(", ", model.Users.Where(u => u.Id != currentUserId).Select(u => u.FirstName));
                if (model.Users.Count == 2)
                    model.PictureUrl = model.PictureUrl ?? model.Users.FirstOrDefault(u => u.Id != currentUserId)?.ProfilePictureUrl;
                return model;
            });
        }

        public static async Task<ChatViewModel> GetChatAsync(this RoomMateExpressDbContext context, Guid currentUserId, Guid chatId)
        {
            const string sql = "SELECT Chats.Id, " +
                                "Chats.Name,  " +
                                "Chats.PictureUrl, " +
                                "Chats.LastModified, " +
                                "Users.Id, " +
                                "Users.FirstName, " +
                                "Users.LastName, " +
                                "Users.ProfilePictureUrl, " +
                                "ChatUsers.Chat_Id AS ChatId, " +
                                "LastMessage.Id, " +
                                "LastMessage.PictureUrl, " +
                                "LastMessage.RecievedAt, " +
                                "LastMessage.SeenAt, " +
                                "LastMessage.SentAt, " +
                                "LastMessage.Text, " +
                                "LastMessage.UserSender_Id AS UserSenderId, " +
                                "UserSender.Id, " +
                                "UserSender.FirstName, " +
                                "UserSender.LastName, " +
                                "UserSender.ProfilePictureUrl " +
                               "FROM Chats " +
                                "INNER JOIN ChatUsers " +
                                    "INNER JOIN Users " +
                                    "ON Users.Id = ChatUsers.User_Id " +
                                "ON Chats.Id = ChatUsers.Chat_Id " +
                               "LEFT JOIN (SELECT Messages.* FROM " +
                                "(SELECT Messages.*, ROW_NUMBER() OVER(PARTITION BY Messages.Chat_Id ORDER BY Messages.SentAt desc) AS rn FROM Messages) AS Messages " +
                                    "WHERE rn <= 20) AS LastMessage " +
                                "INNER JOIN Users AS UserSender " +
                                    "ON UserSender.Id = LastMessage.UserSender_Id " +
                               "ON LastMessage.Chat_Id = Chats.Id " +
                               "WHERE Chats.Id = @ChatId;";
            var connection = context.Database.Connection as SqlConnection;
            var chatDictionary = new Dictionary<Guid, ChatViewModel>();
            return (await connection.QueryAsync<ChatViewModel, User, Message, User, ChatViewModel>(sql, (chat, user, message, user1) =>
            {
                if (!chatDictionary.TryGetValue(chat.Id, out var outChat))
                {
                    outChat = chat;
                    chatDictionary.Add(outChat.Id, outChat);
                }
                if (message != null && !outChat.Messages.Contains(message))
                {
                    message.UserSender = user1;
                    outChat.Messages.Add(message);
                }
                if (!outChat.Users.Contains(user))
                    outChat.Users.Add(user);
                return outChat;
            }, new { ChatId = chatId })).Distinct().Select(model =>
            {
                model.Name = model.Name ?? string.Join(", ", model.Users.Where(u => u.Id != currentUserId).Select(u => u.FirstName));
                if (model.Users.Count == 2)
                    model.PictureUrl = model.PictureUrl ?? model.Users.FirstOrDefault(u => u.Id != currentUserId)?.ProfilePictureUrl;
                model.LastMessage = model.Messages.FirstOrDefault();
                return model;
            }).FirstOrDefault();
        }

        public static async Task<ChatViewModel> GetChatByUsersAsync(this RoomMateExpressDbContext context, Guid currentUserId, Guid userId)
        {
            const string sql = "SELECT Chats.Id, " +
                                "Chats.Name, " +
                                "Chats.PictureUrl, " +
                                "Chats.LastModified, " +
                                "Users.Id, " +
                                "Users.FirstName, " +
                                "Users.LastName, " +
                                "Users.ProfilePictureUrl, " +
                                "ChatUsers.Chat_Id AS ChatId, " +
                                "LastMessage.Id, " +
                                "LastMessage.PictureUrl, " +
                                "LastMessage.RecievedAt, " +
                                "LastMessage.SeenAt, " +
                                "LastMessage.SentAt, " +
                                "LastMessage.Text, " +
                                "LastMessage.UserSender_Id AS UserSenderId, " +
                                "UserSender.Id, " +
                                "UserSender.FirstName, " +
                                "UserSender.LastName, " +
                                "UserSender.ProfilePictureUrl " +
                               "FROM Chats " +
                               "INNER JOIN ChatUsers " +
                                "INNER JOIN Users " +
                                "ON Users.Id = ChatUsers.User_Id " +
                               "ON Chats.Id = ChatUsers.Chat_Id " +
                               "LEFT JOIN (SELECT Messages.* FROM " +
                                "(SELECT Messages.*, ROW_NUMBER() OVER(PARTITION BY Messages.Chat_Id ORDER BY Messages.SentAt desc) AS rn FROM Messages) AS Messages " +
                                "WHERE rn <= 20) AS LastMessage " +
                                    "INNER JOIN Users AS UserSender " +
                                    "ON UserSender.Id = LastMessage.UserSender_Id " +
                               "ON LastMessage.Chat_Id = Chats.Id " +
                               "WHERE (SELECT COUNT(*) FROM ChatUsers AS CH " +
                                    "WHERE CH.Chat_Id = Chats.Id) = 2 " +
                               "AND (SELECT COUNT(*) FROM ChatUsers AS CH " +
                                    "WHERE CH.Chat_Id = Chats.Id " +
                                    "AND (CH.User_Id = @CurrentUserId OR " +
                                    "CH.User_Id = @UserId)) = 2;";
            var connection = context.Database.Connection as SqlConnection;
            var chatDictionary = new Dictionary<Guid, ChatViewModel>();
            return (await connection.QueryAsync<ChatViewModel, User, Message, User, ChatViewModel>(sql, (chat, user, message, user1) =>
            {
                if (!chatDictionary.TryGetValue(chat.Id, out var outChat))
                {
                    outChat = chat;
                    chatDictionary.Add(outChat.Id, outChat);
                }
                if (message != null && !outChat.Messages.Contains(message))
                {
                    message.UserSender = user1;
                    outChat.Messages.Add(message);
                }
                if (!outChat.Users.Contains(user))
                    outChat.Users.Add(user);
                return outChat;
            }, new { UserId = userId, CurrentUserId = currentUserId })).Distinct().Select(model =>
            {
                model.Name = model.Name ?? string.Join(", ", model.Users.Where(u => u.Id != currentUserId).Select(u => u.FirstName));
                if (model.Users.Count == 2)
                    model.PictureUrl = model.PictureUrl ?? model.Users.FirstOrDefault(u => u.Id != currentUserId)?.ProfilePictureUrl;
                model.LastMessage = model.Messages.FirstOrDefault();
                return model;
            }).FirstOrDefault();
        }

        public static async Task<Message> InsertMessageAsync(this RoomMateExpressDbContext context, Message message)
        {
            const string sql =
                "INSERT INTO Messages (Messages.Id, Messages.SentAt, Messages.Text, Messages.PictureUrl, Messages.UserSender_Id, Messages.Chat_Id) " +
                "VALUES (@Id, @SentAt, @Text, @PictureUrl, @UserSenderId, @ChatId); " +
                "UPDATE Chats SET Chats.LastModified = @LastModified WHERE Chats.Id = @ChatId;";
            var connection = context.Database.Connection as SqlConnection;
            await connection.ExecuteAsync(sql,
                new
                {
                    message.Id,
                    message.SentAt,
                    message.Text,
                    message.PictureUrl,
                    UserSenderId = message.UserSender.Id,
                    ChatId = message.Chat.Id,
                    LastModified = message.SentAt
                });
            return message;
        }

        public static async Task<Message> GetMessageAsync(this RoomMateExpressDbContext context, Guid id)
        {
            const string sql = "SELECT Messages.Id, " +
                               "Messages.PictureUrl, " +
                               "Messages.RecievedAt, " +
                               "Messages.SeenAt, " +
                               "Messages.SentAt, " +
                               "Messages.Text, " +
                               "Messages.UserSender_Id AS UserId, " +
                               "Users.Id, " +
                               "Users.FirstName, " +
                               "Users.LastName, " +
                               "Users.ProfilePictureUrl " +
                               "FROM Messages " +
                               "INNER JOIN Users " +
                               "ON Messages.UserSender_Id = Users.Id " +
                               "WHERE Messages.Id = @Id;";
            var connection = context.Database.Connection as SqlConnection;
            return (await connection.QueryAsync<Message, User, Message>(sql, (message, user) =>
            {
                message.UserSender = user;
                return message;
            }, new { Id = id })).FirstOrDefault();
        }

        public static Task<IEnumerable<Message>> GetMessagesAsync(this RoomMateExpressDbContext context, Guid chatId)
        {
            const string sql = "SELECT Messages.Id, " +
                               "Messages.PictureUrl, " +
                               "Messages.RecievedAt, " +
                               "Messages.SeenAt, " +
                               "Messages.SentAt, " +
                               "Messages.Text, " +
                               "Messages.UserSender_Id AS UserId, " +
                               "Users.Id, " +
                               "Users.FirstName, " +
                               "Users.LastName, " +
                               "Users.ProfilePictureUrl " +
                               "FROM Messages " +
                               "INNER JOIN Users " +
                               "ON Messages.UserSender_Id = Users.Id " +
                               "WHERE Messages.Chat_Id = @ChatId " +
                               "ORDER BY Messages.SentAt DESC ";
            var connection = context.Database.Connection as SqlConnection;
            return connection.QueryAsync<Message, User, Message>(sql, (message, user) =>
            {
                message.UserSender = user;
                return message;
            }, new { ChatId = chatId });
        }

        public static Task<IEnumerable<Message>> GetMessagesAsync(this RoomMateExpressDbContext context, Guid chatId, DateTimeOffset date, int numberToTake)
        {
            const string sql = "SELECT Messages.Id, " +
                               "Messages.PictureUrl, " +
                               "Messages.RecievedAt, " +
                               "Messages.SeenAt, " +
                               "Messages.SentAt, " +
                               "Messages.Text, " +
                               "Messages.UserSender_Id AS UserId, " +
                               "Users.Id, " +
                               "Users.FirstName, " +
                               "Users.LastName, " +
                               "Users.ProfilePictureUrl " +
                               "FROM Messages " +
                               "INNER JOIN Users " +
                               "ON Messages.UserSender_Id = Users.Id " +
                               "WHERE Messages.Chat_Id = @ChatId " +
                               "AND Messages.SentAt < @DateParam " +
                               "ORDER BY Messages.SentAt DESC " +
                               "OFFSET 0 ROWS " +
                               "FETCH FIRST @NumberToTake ROWS ONLY;";
            var connection = context.Database.Connection as SqlConnection;
            return connection.QueryAsync<Message, User, Message>(sql, (message, user) =>
            {
                message.UserSender = user;
                return message;
            }, new { ChatId = chatId, DateParam = date, NumberToTake = numberToTake });
        }

        public static Task<IEnumerable<Message>> GetNewMessagesAsync(this RoomMateExpressDbContext context, Guid chatId, DateTimeOffset date, int numberToTake)
        {
            const string sql = "SELECT Messages.Id, " +
                               "Messages.PictureUrl, " +
                               "Messages.RecievedAt, " +
                               "Messages.SeenAt, " +
                               "Messages.SentAt, " +
                               "Messages.Text, " +
                               "Messages.UserSender_Id AS UserId, " +
                               "Users.Id, " +
                               "Users.FirstName, " +
                               "Users.LastName, " +
                               "Users.ProfilePictureUrl " +
                               "FROM Messages " +
                               "INNER JOIN Users " +
                               "ON Messages.UserSender_Id = Users.Id " +
                               "WHERE Messages.Chat_Id = @ChatId " +
                               "AND Messages.SentAt > @DateParam " +
                               "ORDER BY Messages.SentAt DESC " +
                               "OFFSET 0 ROWS " +
                               "FETCH FIRST @NumberToTake ROWS ONLY;";
            var connection = context.Database.Connection as SqlConnection;
            return connection.QueryAsync<Message, User, Message>(sql, (message, user) =>
            {
                message.UserSender = user;
                return message;
            }, new { ChatId = chatId, DateParam = date, NumberToTake = numberToTake });
        }

        public static async Task<ProfileComment> InsertProfileCommentAsync(this RoomMateExpressDbContext context, ProfileComment commentForProfile)
        {
            const string sql =
                "INSERT INTO ProfileComments (ProfileComments.Id, ProfileComments.CommentedAt, ProfileComments.Text, ProfileComments.Grade, ProfileComments.UserCommentator_Id, ProfileComments.UserProfile_Id) " +
                "VALUES (@Id, @CommentedAt, @Text, @Grade, @UserCommentatorId, @UserProfileId); ";
            var connection = context.Database.Connection as SqlConnection;
            await connection.ExecuteAsync(sql,
                new
                {
                    commentForProfile.Id,
                    commentForProfile.CommentedAt,
                    commentForProfile.Text,
                    commentForProfile.Grade,
                    UserCommentatorId = commentForProfile.UserCommentator.Id,
                    UserProfileId = commentForProfile.UserProfile.Id
                });
            return commentForProfile;
        }

        public static Task<IEnumerable<ProfileComment>> GetProfileCommentsForUserAsync(this RoomMateExpressDbContext context, Guid userId)
        {
            const string sql = "SELECT ProfileComments.Id, " +
                               "ProfileComments.CommentedAt, " +
                               "ProfileComments.Text, " +
                               "ProfileComments.Grade, " +
                               "ProfileComments.UserCommentator_Id AS UserId, " +
                               "Users.Id, " +
                               "Users.FirstName, " +
                               "Users.LastName, " +
                               "Users.ProfilePictureUrl " +
                               "FROM ProfileComments " +
                               "INNER JOIN Users " +
                               "ON ProfileComments.UserCommentator_Id = Users.Id " +
                               "WHERE ProfileComments.UserProfile_Id = @UserId " +
                               "ORDER BY ProfileComments.CommentedAt DESC;";
            var connection = context.Database.Connection as SqlConnection;
            return connection.QueryAsync<ProfileComment, User, ProfileComment>(sql, (comment, user) =>
            {
                comment.UserCommentator = user;
                return comment;
            }, new { UserId = userId });
        }

        public static Task<IEnumerable<ProfileComment>> GetProfileCommentsForUserAsync(this RoomMateExpressDbContext context, Guid userId, DateTimeOffset date, int numberToTake)
        {
            const string sql = "SELECT ProfileComments.Id, " +
                               "ProfileComments.CommentedAt, " +
                               "ProfileComments.Text, " +
                               "ProfileComments.Grade, " +
                               "ProfileComments.UserCommentator_Id AS UserId, " +
                               "Users.Id, " +
                               "Users.FirstName, " +
                               "Users.LastName, " +
                               "Users.ProfilePictureUrl " +
                               "FROM ProfileComments " +
                               "INNER JOIN Users " +
                               "ON ProfileComments.UserCommentator_Id = Users.Id " +
                               "WHERE ProfileComments.UserProfile_Id = @UserId " +
                               "AND ProfileComments.CommentedAt < @DateParam " +
                               "ORDER BY ProfileComments.CommentedAt DESC " +
                               "OFFSET 0 ROWS " +
                               "FETCH FIRST @NumberToTake ROWS ONLY;";
            var connection = context.Database.Connection as SqlConnection;
            return connection.QueryAsync<ProfileComment, User, ProfileComment>(sql, (comment, user) =>
            {
                comment.UserCommentator = user;
                return comment;
            }, new { UserId = userId, DateParam = date, NumberToTake = numberToTake });
        }

        public static Task<IEnumerable<ProfileComment>> GetProfileCommentsByUserAsync(this RoomMateExpressDbContext context, Guid userId)
        {
            const string sql = "SELECT ProfileComments.Id, " +
                               "ProfileComments.CommentedAt, " +
                               "ProfileComments.Text, " +
                               "ProfileComments.Grade, " +
                               "ProfileComments.UserProfile_Id AS UserId, " +
                               "Users.Id, " +
                               "Users.FirstName, " +
                               "Users.LastName, " +
                               "Users.ProfilePictureUrl " +
                               "FROM ProfileComments " +
                               "INNER JOIN Users " +
                               "ON ProfileComments.UserProfile_Id = Users.Id " +
                               "WHERE ProfileComments.UserCommentator_Id = @UserId " +
                               "ORDER BY ProfileComments.CommentedAt DESC;";
            var connection = context.Database.Connection as SqlConnection;
            return connection.QueryAsync<ProfileComment, User, ProfileComment>(sql, (comment, user) =>
            {
                comment.UserProfile = user;
                return comment;
            }, new { UserId = userId });
        }

        public static Task<IEnumerable<ProfileComment>> GetProfileCommentsByUserAsync(this RoomMateExpressDbContext context, Guid userId, DateTimeOffset date, int numberToTake)
        {
            const string sql = "SELECT ProfileComments.Id, " +
                               "ProfileComments.CommentedAt, " +
                               "ProfileComments.Text, " +
                               "ProfileComments.Grade, " +
                               "ProfileComments.UserProfile_Id AS UserId, " +
                               "Users.Id, " +
                               "Users.FirstName, " +
                               "Users.LastName, " +
                               "Users.ProfilePictureUrl " +
                               "FROM ProfileComments " +
                               "INNER JOIN Users " +
                               "ON ProfileComments.UserProfile_Id = Users.Id " +
                               "WHERE ProfileComments.UserCommentator_Id = @UserId " +
                               "AND ProfileComments.CommentedAt < @DateParam " +
                               "ORDER BY ProfileComments.CommentedAt DESC " +
                               "OFFSET 0 ROWS " +
                               "FETCH FIRST @NumberToTake ROWS ONLY;";
            var connection = context.Database.Connection as SqlConnection;
            return connection.QueryAsync<ProfileComment, User, ProfileComment>(sql, (comment, user) =>
            {
                comment.UserProfile = user;
                return comment;
            }, new { UserId = userId, DateParam = date, NumberToTake = numberToTake });
        }

        public static async Task<PostViewModel> InsertPostAsync(this RoomMateExpressDbContext context, Post post)
        {
            const string sql =
                "INSERT INTO Posts (Posts.Id, Posts.User_Id, Posts.Title, Posts.Description, Posts.Price, Posts.AccomodationOption, Posts.AccomodationType, Posts.PetOptions, Posts.ArePetsAllowed, Posts.IsSmokerAllowed, Posts.NumberOfRoommates, Posts.WantedGender, Posts.PostDate) " +
                "VALUES (@Id, @UserId, @Title, @Description, @Price, @AccomodationOption, @AccomodationType, @PetOptions, @ArePetsAllowed, @IsSmokerAllowed, @NumberOfRoommates, @WantedGender, @PostDate); ";
            const string neighborhoodSql = "INSERT INTO NeighborhoodPosts (NeighborhoodPosts.Neighborhood_Id, Post_Id) Values (@NeighborhoodId, @PostId);";
            const string pictureSql =
                "INSERT INTO PostPictures (PostPictures.Id, PostPictures.PictureUrl, PostPictures.Post_Id) " +
                "VALUES (@Id, @PictureUrl, @PostId);";
            var connection = context.Database.Connection as SqlConnection;
            await connection.OpenAsync();
            var transaction = connection.BeginTransaction("create_post");
            try
            {
                await connection.ExecuteAsync(sql,
                    new
                    {
                        post.Id,
                        UserId = post.User.Id,
                        post.Title,
                        post.Description,
                        post.Price,
                        post.AccomodationOption,
                        post.AccomodationType,
                        post.PetOptions,
                        post.ArePetsAllowed,
                        post.IsSmokerAllowed,
                        post.NumberOfRoommates,
                        post.WantedGender,
                        post.PostDate,
                    }, transaction);
                await connection.ExecuteAsync(neighborhoodSql, post.Neighborhoods.Select(n => new
                {
                    NeighborhoodId = n.Id,
                    PostId = post.Id
                }), transaction);
                await connection.ExecuteAsync(pictureSql, post.PostPictures.Select(p => new
                {
                    p.Id,
                    p.PictureUrl,
                    PostId = post.Id
                }), transaction);
                transaction.Commit();
            }
            catch(Exception)
            {
                transaction.Rollback();
                throw;
            }
            finally 
            {
                connection.Close();
            }
            return await GetPostAsync(context, post.Id, post.User.Id);
        }

        public static async Task<PostViewModel> UpdatePostAsync(this RoomMateExpressDbContext context, Post post, Guid userId)
        {
            const string sql =
                "UPDATE Posts SET " +
                        "Posts.Title = @Title, " +
                        "Posts.Description = @Description, " +
                        "Posts.Price = @Price, " +
                        "Posts.AccomodationOption = @AccomodationOption, " +
                        "Posts.AccomodationType = @AccomodationType, " +
                        "Posts.PetOptions = @PetOptions, " +
                        "Posts.ArePetsAllowed = @ArePetsAllowed, " +
                        "Posts.IsSmokerAllowed = @IsSmokerAllowed, " +
                        "Posts.NumberOfRoommates = @NumberOfRoommates, " +
                        "Posts.WantedGender = @WantedGender " +
                "WHERE Posts.Id = @Id AND Posts.User_Id = @CheckUserId; ";
            const string neighborhoodSql = "DELETE FROM NeighborhoodPosts WHERE NeighborhoodPosts.Post_Id = @PostId; " +
                                           "INSERT INTO NeighborhoodPosts (NeighborhoodPosts.Neighborhood_Id, Post_Id) Values (@NeighborhoodId, @PostId);";
            const string pictureSql =
                "DELETE FROM PostPictures WHERE PostPictures.Post_Id = @PostId; " +
                "INSERT INTO PostPictures (PostPictures.Id, PostPictures.PictureUrl, PostPictures.Post_Id) " +
                "VALUES (@Id, @PictureUrl, @PostId);";
            var connection = context.Database.Connection as SqlConnection;
            await connection.OpenAsync();
            var transaction = connection.BeginTransaction("update_post");
            try
            {
                var num = await connection.ExecuteAsync(sql,
                    new
                    {
                        post.Id,
                        UserId = post.User.Id,
                        post.Title,
                        post.Description,
                        post.Price,
                        post.AccomodationOption,
                        post.AccomodationType,
                        post.PetOptions,
                        post.ArePetsAllowed,
                        post.IsSmokerAllowed,
                        post.NumberOfRoommates,
                        post.WantedGender,
                        post.PostDate,
                        CheckUserId = userId
                    }, transaction);
                if(num == 0) throw new UserNotFoundException(Constants.Errors.UserNotFound);
                await connection.ExecuteAsync(neighborhoodSql, post.Neighborhoods.Select(n => new
                {
                    NeighborhoodId = n.Id,
                    PostId = post.Id
                }), transaction);
                await connection.ExecuteAsync(pictureSql, post.PostPictures.Select(p => new
                {
                    p.Id,
                    p.PictureUrl,
                    PostId = post.Id
                }), transaction);
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                connection.Close();
            }
            return await GetPostAsync(context, post.Id, post.User.Id);
        }

        public static async Task<PostViewModel> GetPostAsync(this RoomMateExpressDbContext context, Guid id)
        {
            const string sql = "SELECT Posts.Id, " +
                               "Posts.AccomodationOption, " +
                               "Posts.AccomodationType, " +
                               "Posts.ArePetsAllowed, " +
                               "Posts.Description, " +
                               "Posts.IsSmokerAllowed, " +
                               "Posts.NumberOfRoommates, " +
                               "Posts.PetOptions, " +
                               "Posts.PostDate, " +
                               "Posts.Price, " +
                               "Posts.Title, " +
                               "Posts.WantedGender, " +
                               "COUNT(DISTINCT PostComments.Id) AS CommentsCount, " +
                               "COUNT(PostUsers.Post_Id) AS LikesCount, " +
                               "UserAuthor.Id, " +
                               "UserAuthor.FirstName, " +
                               "UserAuthor.LastName, " +
                               "UserAuthor.ProfilePictureUrl, " +
                               "UserAuthor.AverageGrade, " +
                               "Neighborhoods.Id, " +
                               "Neighborhoods.Name, " +
                               "PostPictures.Id, " +
                               "PostPictures.PictureUrl " +
                               "FROM Posts " +
                               "INNER JOIN (SELECT UserAuthor.Id, " +
                               "UserAuthor.FirstName, " +
                               "UserAuthor.LastName, " +
                               "UserAuthor.ProfilePictureUrl, " +
                               "ISNULL(CAST(AVG(CAST(Comments.Grade AS Decimal(2,1))) AS Decimal(2,1)), 0) AS AverageGrade " +
                               "FROM Users AS UserAuthor " +
                               "LEFT JOIN (SELECT ProfileComments.UserProfile_Id, ProfileComments.Grade " +
                               "FROM ProfileComments) Comments " +
                               "ON UserAuthor.Id = Comments.UserProfile_Id " +
                               "GROUP BY UserAuthor.Id, UserAuthor.FirstName, UserAuthor.LastName, UserAuthor.ProfilePictureUrl) AS UserAuthor " +
                               "ON UserAuthor.Id = Posts.User_Id " +
                               "INNER JOIN NeighborhoodPosts " +
                               "INNER JOIN Neighborhoods " +
                               "ON Neighborhoods.Id = NeighborhoodPosts.Neighborhood_Id " +
                               "ON Posts.Id = NeighborhoodPosts.Post_Id " +
                               "LEFT JOIN PostPictures " +
                               "ON PostPictures.Post_Id = Posts.Id " +
                               "LEFT JOIN PostComments " +
                               "ON Posts.Id = PostComments.Post_Id " +
                               "LEFT JOIN PostUsers " +
                               "ON Posts.Id = PostUsers.Post_Id " +
                               "WHERE Posts.Id = @PostId " +
                               "GROUP BY Posts.Id, " +
                               "Posts.AccomodationOption, " +
                               "Posts.AccomodationType, " +
                               "Posts.ArePetsAllowed, " +
                               "Posts.Description, " +
                               "Posts.IsSmokerAllowed, " +
                               "Posts.NumberOfRoommates, " +
                               "Posts.PetOptions, " +
                               "Posts.PostDate, " +
                               "Posts.Price, " +
                               "Posts.Title, " +
                               "Posts.WantedGender, " +
                               "UserAuthor.Id, " +
                               "UserAuthor.FirstName, " +
                               "UserAuthor.LastName, " +
                               "UserAuthor.ProfilePictureUrl, " +
                               "UserAuthor.AverageGrade, " +
                               "Neighborhoods.Id, " +
                               "Neighborhoods.Name, " +
                               "PostPictures.Id, " +
                               "PostPictures.PictureUrl, " +
                               "PostPictures.Post_Id;";
            var connection = context.Database.Connection as SqlConnection;
            var postDictionary = new Dictionary<Guid, PostViewModel>();
            return (await connection.QueryAsync<PostViewModel, UserViewModel, Neighborhood, PostPicture, PostViewModel>(sql, (post, user, neighborhood, postPicture) =>
            {
                if (!postDictionary.TryGetValue(post.Id, out var outPost))
                {
                    outPost = post;
                    outPost.User = user;
                    postDictionary.Add(outPost.Id, outPost);
                }
                if (neighborhood != null && !outPost.Neighborhoods.Contains(neighborhood))
                    outPost.Neighborhoods.Add(neighborhood);
                if (postPicture != null && !outPost.PostPictures.Contains(postPicture))
                    outPost.PostPictures.Add(postPicture);
                return outPost;
            }, new { PostId = id })).Distinct().FirstOrDefault();
        }

        public static async Task<PostViewModel> GetPostAsync(this RoomMateExpressDbContext context, Guid id, Guid currentUserId)
        {
            const string sql = "SELECT Posts.Id, " +
                               "Posts.AccomodationOption, " +
                               "Posts.AccomodationType, " +
                               "Posts.ArePetsAllowed, " +
                               "Posts.Description, " +
                               "Posts.IsSmokerAllowed, " +
                               "Posts.NumberOfRoommates, " +
                               "Posts.PetOptions, " +
                               "Posts.PostDate, " +
                               "Posts.Price, " +
                               "Posts.Title, " +
                               "Posts.WantedGender, " +
                               "COUNT(DISTINCT PostComments.Id) AS CommentsCount, " +
                               "COUNT(PostUsers.Post_Id) AS LikesCount, " +
                               "CAST(COUNT(UserLikes.Post_Id) AS BIT) AS IsLiked, " +
                               "UserAuthor.Id, " +
                               "UserAuthor.FirstName, " +
                               "UserAuthor.LastName, " +
                               "UserAuthor.ProfilePictureUrl, " +
                               "UserAuthor.AverageGrade, " +
                               "Neighborhoods.Id, " +
                               "Neighborhoods.Name, " +
                               "PostPictures.Id, " +
                               "PostPictures.PictureUrl " +
                               "FROM Posts " +
                               "INNER JOIN (SELECT UserAuthor.Id, " +
                               "UserAuthor.FirstName, " +
                               "UserAuthor.LastName, " +
                               "UserAuthor.ProfilePictureUrl, " +
                               "ISNULL(CAST(AVG(CAST(Comments.Grade AS Decimal(2,1))) AS Decimal(2,1)), 0) AS AverageGrade " +
                               "FROM Users AS UserAuthor " +
                               "LEFT JOIN (SELECT ProfileComments.UserProfile_Id, ProfileComments.Grade " +
                               "FROM ProfileComments) Comments " +
                               "ON UserAuthor.Id = Comments.UserProfile_Id " +
                               "GROUP BY UserAuthor.Id, UserAuthor.FirstName, UserAuthor.LastName, UserAuthor.ProfilePictureUrl) AS UserAuthor " +
                               "ON UserAuthor.Id = Posts.User_Id " +
                               "INNER JOIN NeighborhoodPosts " +
                               "INNER JOIN Neighborhoods " +
                               "ON Neighborhoods.Id = NeighborhoodPosts.Neighborhood_Id " +
                               "ON Posts.Id = NeighborhoodPosts.Post_Id " +
                               "LEFT JOIN PostPictures " +
                               "ON PostPictures.Post_Id = Posts.Id " +
                               "LEFT JOIN PostComments " +
                               "ON Posts.Id = PostComments.Post_Id " +
                               "LEFT JOIN PostUsers " +
                               "ON Posts.Id = PostUsers.Post_Id " +
                               "LEFT JOIN PostUsers AS UserLikes " +
                               "ON Posts.Id = UserLikes.Post_Id " +
                               "AND UserLikes.User_Id = @UserId " +
                               "WHERE Posts.Id = @PostId " +
                               "GROUP BY Posts.Id, " +
                               "Posts.AccomodationOption, " +
                               "Posts.AccomodationType, " +
                               "Posts.ArePetsAllowed, " +
                               "Posts.Description, " +
                               "Posts.IsSmokerAllowed, " +
                               "Posts.NumberOfRoommates, " +
                               "Posts.PetOptions, " +
                               "Posts.PostDate, " +
                               "Posts.Price, " +
                               "Posts.Title, " +
                               "Posts.WantedGender, " +
                               "UserAuthor.Id, " +
                               "UserAuthor.FirstName, " +
                               "UserAuthor.LastName, " +
                               "UserAuthor.ProfilePictureUrl, " +
                               "UserAuthor.AverageGrade, " +
                               "Neighborhoods.Id, " +
                               "Neighborhoods.Name, " +
                               "PostPictures.Id, " +
                               "PostPictures.PictureUrl, " +
                               "PostPictures.Post_Id;";
            var connection = context.Database.Connection as SqlConnection;
            var postDictionary = new Dictionary<Guid, PostViewModel>();
            return (await connection.QueryAsync<PostViewModel, UserViewModel, Neighborhood, PostPicture, PostViewModel>(sql, (post, user, neighborhood, postPicture) =>
            {
                if (!postDictionary.TryGetValue(post.Id, out var outPost))
                {
                    outPost = post;
                    outPost.User = user;
                    postDictionary.Add(outPost.Id, outPost);
                }
                if (neighborhood != null && !outPost.Neighborhoods.Contains(neighborhood))
                    outPost.Neighborhoods.Add(neighborhood);
                if (postPicture != null && !outPost.PostPictures.Contains(postPicture))
                    outPost.PostPictures.Add(postPicture);
                return outPost;
            }, new { UserId = currentUserId, PostId = id })).Distinct().FirstOrDefault();
        }

        public static async Task<IEnumerable<PostViewModel>> GetAllPostsAsync(this RoomMateExpressDbContext context,
            PostSortOptions sortOptions = PostSortOptions.Date,
            SortOrderOption orderOption = SortOrderOption.Descending, AccomodationOptions? accomodationOptions = null,
            decimal? minPrice = null, decimal? maxPrice = null, string city = null, string keyword = null)
        {
            var sql = "SELECT Posts.Id, " +
                      "Posts.AccomodationOption, " +
                      "Posts.AccomodationType, " +
                      "Posts.ArePetsAllowed, " +
                      "Posts.Description, " +
                      "Posts.IsSmokerAllowed, " +
                      "Posts.NumberOfRoommates, " +
                      "Posts.PetOptions, " +
                      "Posts.PostDate, " +
                      "Posts.Price, " +
                      "Posts.Title, " +
                      "Posts.WantedGender, " +
                      "COUNT(DISTINCT PostComments.Id) AS CommentsCount, " +
                      "COUNT(PostUsers.Post_Id) AS LikesCount, " +
                      "UserAuthor.Id, " +
                      "UserAuthor.FirstName, " +
                      "UserAuthor.LastName, " +
                      "UserAuthor.ProfilePictureUrl, " +
                      "UserAuthor.AverageGrade, " +
                      "Neighborhoods.Id, " +
                      "Neighborhoods.Name, " +
                      "PostPictures.Id, " +
                      "PostPictures.PictureUrl " +
                      "FROM Posts " +
                      "INNER JOIN (SELECT UserAuthor.Id, " +
                      "UserAuthor.FirstName, " +
                      "UserAuthor.LastName, " +
                      "UserAuthor.ProfilePictureUrl, " +
                      "ISNULL(CAST(AVG(CAST(Comments.Grade AS Decimal(2,1))) AS Decimal(2,1)), 0) AS AverageGrade " +
                      "FROM Users AS UserAuthor " +
                      "LEFT JOIN (SELECT ProfileComments.UserProfile_Id, ProfileComments.Grade " +
                      "FROM ProfileComments) Comments " +
                      "ON UserAuthor.Id = Comments.UserProfile_Id " +
                      "GROUP BY UserAuthor.Id, UserAuthor.FirstName, UserAuthor.LastName, UserAuthor.ProfilePictureUrl) AS UserAuthor " +
                      "ON UserAuthor.Id = Posts.User_Id " +
                      "INNER JOIN NeighborhoodPosts " +
                      "INNER JOIN Neighborhoods " +
                      "ON Neighborhoods.Id = NeighborhoodPosts.Neighborhood_Id " +
                      "ON Posts.Id = NeighborhoodPosts.Post_Id " +
                      "LEFT JOIN PostPictures " +
                      "ON PostPictures.Post_Id = Posts.Id " +
                      "LEFT JOIN PostComments " +
                      "ON Posts.Id = PostComments.Post_Id " +
                      "LEFT JOIN PostUsers " +
                      "ON Posts.Id = PostUsers.Post_Id " +
                      GetPostFilterSql(accomodationOptions, minPrice, maxPrice, city, keyword) +
                      "GROUP BY Posts.Id, " +
                      "Posts.AccomodationOption, " +
                      "Posts.AccomodationType, " +
                      "Posts.ArePetsAllowed, " +
                      "Posts.Description, " +
                      "Posts.IsSmokerAllowed, " +
                      "Posts.NumberOfRoommates, " +
                      "Posts.PetOptions, " +
                      "Posts.PostDate, " +
                      "Posts.Price, " +
                      "Posts.Title, " +
                      "Posts.WantedGender, " +
                      "UserAuthor.Id, " +
                      "UserAuthor.FirstName, " +
                      "UserAuthor.LastName, " +
                      "UserAuthor.ProfilePictureUrl, " +
                      "UserAuthor.AverageGrade, " +
                      "Neighborhoods.Id, " +
                      "Neighborhoods.Name, " +
                      "PostPictures.Id, " +
                      "PostPictures.PictureUrl, " +
                      "PostPictures.Post_Id " +
                      GetPostSortingSql(sortOptions, orderOption);
            var connection = context.Database.Connection as SqlConnection;
            var postDictionary = new Dictionary<Guid, PostViewModel>();
            return (await connection.QueryAsync<PostViewModel, UserViewModel, Neighborhood, PostPicture, PostViewModel>(sql, (post, user, neighborhood, postPicture) =>
            {
                if (!postDictionary.TryGetValue(post.Id, out var outPost))
                {
                    outPost = post;
                    outPost.User = user;
                    postDictionary.Add(outPost.Id, outPost);
                }
                if (neighborhood != null && !outPost.Neighborhoods.Contains(neighborhood))
                    outPost.Neighborhoods.Add(neighborhood);
                if (postPicture != null && !outPost.PostPictures.Contains(postPicture))
                    outPost.PostPictures.Add(postPicture);
                return outPost;
            }, new { AccomodationOption = (int?)accomodationOptions, MinPrice = minPrice, MaxPrice = maxPrice, City = city, Keyword = keyword })).Distinct();
        }

        public static async Task<IEnumerable<PostViewModel>> GetAllPostsAsync(this RoomMateExpressDbContext context, 
            DateTimeOffset date, object pagingModifier, int numberToTake,
            PostSortOptions sortOptions = PostSortOptions.Date,
            SortOrderOption orderOption = SortOrderOption.Descending, AccomodationOptions? accomodationOptions = null,
            decimal? minPrice = null, decimal? maxPrice = null, string city = null, string keyword = null)
        {
            var sql = "SELECT Posts.Id, " +
                      "Posts.AccomodationOption, " +
                      "Posts.AccomodationType, " +
                      "Posts.ArePetsAllowed, " +
                      "Posts.Description, " +
                      "Posts.IsSmokerAllowed, " +
                      "Posts.NumberOfRoommates, " +
                      "Posts.PetOptions, " +
                      "Posts.PostDate, " +
                      "Posts.Price, " +
                      "Posts.Title, " +
                      "Posts.WantedGender, " +
                      "COUNT(DISTINCT PostComments.Id) AS CommentsCount, " +
                      "COUNT(PostUsers.Post_Id) AS LikesCount, " +
                      "UserAuthor.Id, " +
                      "UserAuthor.FirstName, " +
                      "UserAuthor.LastName, " +
                      "UserAuthor.ProfilePictureUrl, " +
                      "UserAuthor.AverageGrade, " +
                      "Neighborhoods.Id, " +
                      "Neighborhoods.Name, " +
                      "PostPictures.Id, " +
                      "PostPictures.PictureUrl " +
                      "FROM Posts " +
                      "INNER JOIN (SELECT UserAuthor.Id, " +
                      "UserAuthor.FirstName, " +
                      "UserAuthor.LastName, " +
                      "UserAuthor.ProfilePictureUrl, " +
                      "ISNULL(CAST(AVG(CAST(Comments.Grade AS Decimal(2,1))) AS Decimal(2,1)), 0) AS AverageGrade " +
                      "FROM Users AS UserAuthor " +
                      "LEFT JOIN (SELECT ProfileComments.UserProfile_Id, ProfileComments.Grade " +
                      "FROM ProfileComments) Comments " +
                      "ON UserAuthor.Id = Comments.UserProfile_Id " +
                      "GROUP BY UserAuthor.Id, UserAuthor.FirstName, UserAuthor.LastName, UserAuthor.ProfilePictureUrl) AS UserAuthor " +
                      "ON UserAuthor.Id = Posts.User_Id " +
                      "INNER JOIN NeighborhoodPosts " +
                      "INNER JOIN Neighborhoods " +
                      "ON Neighborhoods.Id = NeighborhoodPosts.Neighborhood_Id " +
                      "ON Posts.Id = NeighborhoodPosts.Post_Id " +
                      "LEFT JOIN PostPictures " +
                      "ON PostPictures.Post_Id = Posts.Id " +
                      "LEFT JOIN PostComments " +
                      "ON Posts.Id = PostComments.Post_Id " +
                      "LEFT JOIN PostUsers " +
                      "ON Posts.Id = PostUsers.Post_Id " +
                      $"WHERE Posts.ID IN {GetPostPaging(DapperPostFlags.None, sortOptions, orderOption, accomodationOptions, minPrice, maxPrice, city, keyword)} " +
                      "GROUP BY Posts.Id, " +
                      "Posts.AccomodationOption, " +
                      "Posts.AccomodationType, " +
                      "Posts.ArePetsAllowed, " +
                      "Posts.Description, " +
                      "Posts.IsSmokerAllowed, " +
                      "Posts.NumberOfRoommates, " +
                      "Posts.PetOptions, " +
                      "Posts.PostDate, " +
                      "Posts.Price, " +
                      "Posts.Title, " +
                      "Posts.WantedGender, " +
                      "UserAuthor.Id, " +
                      "UserAuthor.FirstName, " +
                      "UserAuthor.LastName, " +
                      "UserAuthor.ProfilePictureUrl, " +
                      "UserAuthor.AverageGrade, " +
                      "Neighborhoods.Id, " +
                      "Neighborhoods.Name, " +
                      "PostPictures.Id, " +
                      "PostPictures.PictureUrl, " +
                      "PostPictures.Post_Id " +
                      GetPostSortingSql(sortOptions, orderOption);
            var connection = context.Database.Connection as SqlConnection;
            var postDictionary = new Dictionary<Guid, PostViewModel>();
            return (await connection.QueryAsync<PostViewModel, UserViewModel, Neighborhood, PostPicture, PostViewModel>(sql, (post, user, neighborhood, postPicture) =>
            {
                if (!postDictionary.TryGetValue(post.Id, out var outPost))
                {
                    outPost = post;
                    outPost.User = user;
                    postDictionary.Add(outPost.Id, outPost);
                }
                if (neighborhood != null && !outPost.Neighborhoods.Contains(neighborhood))
                    outPost.Neighborhoods.Add(neighborhood);
                if (postPicture != null && !outPost.PostPictures.Contains(postPicture))
                    outPost.PostPictures.Add(postPicture);
                return outPost;
            }, new { AccomodationOption = (int?)accomodationOptions, MinPrice = minPrice, MaxPrice = maxPrice, City = city, Keyword = keyword, DateParam = date, NumberToTake = numberToTake, PagingModifier = pagingModifier })).Distinct();
        }

        public static async Task<IEnumerable<PostViewModel>> GetAllPostsAsync(this RoomMateExpressDbContext context, Guid currentUserId,
            PostSortOptions sortOptions = PostSortOptions.Date,
            SortOrderOption orderOption = SortOrderOption.Descending, AccomodationOptions? accomodationOptions = null,
            decimal? minPrice = null, decimal? maxPrice = null, string city = null, string keyword = null)
        {
            var sql = "SELECT Posts.Id, " +
                      "Posts.AccomodationOption, " +
                      "Posts.AccomodationType, " +
                      "Posts.ArePetsAllowed, " +
                      "Posts.Description, " +
                      "Posts.IsSmokerAllowed, " +
                      "Posts.NumberOfRoommates, " +
                      "Posts.PetOptions, " +
                      "Posts.PostDate, " +
                      "Posts.Price, " +
                      "Posts.Title, " +
                      "Posts.WantedGender, " +
                      "COUNT(DISTINCT PostComments.Id) AS CommentsCount, " +
                      "COUNT(PostUsers.Post_Id) AS LikesCount, " +
                      "CAST(COUNT(UserLikes.Post_Id) AS BIT) AS IsLiked, " +
                      "UserAuthor.Id, " +
                      "UserAuthor.FirstName, " +
                      "UserAuthor.LastName, " +
                      "UserAuthor.ProfilePictureUrl, " +
                      "UserAuthor.AverageGrade, " +
                      "Neighborhoods.Id, " +
                      "Neighborhoods.Name, " +
                      "PostPictures.Id, " +
                      "PostPictures.PictureUrl " +
                      "FROM Posts " +
                      "INNER JOIN (SELECT UserAuthor.Id, " +
                      "UserAuthor.FirstName, " +
                      "UserAuthor.LastName, " +
                      "UserAuthor.ProfilePictureUrl, " +
                      "ISNULL(CAST(AVG(CAST(Comments.Grade AS Decimal(2,1))) AS Decimal(2,1)), 0) AS AverageGrade " +
                      "FROM Users AS UserAuthor " +
                      "LEFT JOIN (SELECT ProfileComments.UserProfile_Id, ProfileComments.Grade " +
                      "FROM ProfileComments) Comments " +
                      "ON UserAuthor.Id = Comments.UserProfile_Id " +
                      "GROUP BY UserAuthor.Id, UserAuthor.FirstName, UserAuthor.LastName, UserAuthor.ProfilePictureUrl) AS UserAuthor " +
                      "ON UserAuthor.Id = Posts.User_Id " +
                      "AND UserAuthor.Id <> @UserId " +
                      "INNER JOIN NeighborhoodPosts " +
                      "INNER JOIN Neighborhoods " +
                      "ON Neighborhoods.Id = NeighborhoodPosts.Neighborhood_Id " +
                      "ON Posts.Id = NeighborhoodPosts.Post_Id " +
                      "LEFT JOIN PostPictures " +
                      "ON PostPictures.Post_Id = Posts.Id " +
                      "LEFT JOIN PostComments " +
                      "ON Posts.Id = PostComments.Post_Id " +
                      "LEFT JOIN PostUsers " +
                      "ON Posts.Id = PostUsers.Post_Id " +
                      "LEFT JOIN PostUsers AS UserLikes " +
                      "ON Posts.Id = UserLikes.Post_Id " +
                      "AND UserLikes.User_Id = @UserId " +
                      GetPostFilterSql(accomodationOptions, minPrice, maxPrice, city, keyword) +
                      "GROUP BY Posts.Id, " +
                      "Posts.AccomodationOption, " +
                      "Posts.AccomodationType, " +
                      "Posts.ArePetsAllowed, " +
                      "Posts.Description, " +
                      "Posts.IsSmokerAllowed, " +
                      "Posts.NumberOfRoommates, " +
                      "Posts.PetOptions, " +
                      "Posts.PostDate, " +
                      "Posts.Price, " +
                      "Posts.Title, " +
                      "Posts.WantedGender, " +
                      "UserAuthor.Id, " +
                      "UserAuthor.FirstName, " +
                      "UserAuthor.LastName, " +
                      "UserAuthor.ProfilePictureUrl, " +
                      "UserAuthor.AverageGrade, " +
                      "Neighborhoods.Id, " +
                      "Neighborhoods.Name, " +
                      "PostPictures.Id, " +
                      "PostPictures.PictureUrl, " +
                      "PostPictures.Post_Id " +
                      GetPostSortingSql(sortOptions, orderOption);
            var connection = context.Database.Connection as SqlConnection;
            var postDictionary = new Dictionary<Guid, PostViewModel>();
            return (await connection.QueryAsync<PostViewModel, UserViewModel, Neighborhood, PostPicture, PostViewModel>(sql, (post, user, neighborhood, postPicture) =>
            {
                if (!postDictionary.TryGetValue(post.Id, out var outPost))
                {
                    outPost = post;
                    outPost.User = user;
                    postDictionary.Add(outPost.Id, outPost);
                }
                if (neighborhood != null && !outPost.Neighborhoods.Contains(neighborhood))
                    outPost.Neighborhoods.Add(neighborhood);
                if (postPicture != null && !outPost.PostPictures.Contains(postPicture))
                    outPost.PostPictures.Add(postPicture);
                return outPost;
            }, new { AccomodationOption = (int?)accomodationOptions, MinPrice = minPrice, MaxPrice = maxPrice, City = city, Keyword = keyword, UserId = currentUserId })).Distinct();
        }

        public static async Task<IEnumerable<PostViewModel>> GetAllPostsAsync(this RoomMateExpressDbContext context,
            Guid currentUserId,
            DateTimeOffset date, object pagingModifier, int numberToTake,
            PostSortOptions sortOptions = PostSortOptions.Date,
            SortOrderOption orderOption = SortOrderOption.Descending, AccomodationOptions? accomodationOptions = null,
            decimal? minPrice = null, decimal? maxPrice = null, string city = null, string keyword = null)
        {
            var sql = "SELECT Posts.Id, " +
                      "Posts.AccomodationOption, " +
                      "Posts.AccomodationType, " +
                      "Posts.ArePetsAllowed, " +
                      "Posts.Description, " +
                      "Posts.IsSmokerAllowed, " +
                      "Posts.NumberOfRoommates, " +
                      "Posts.PetOptions, " +
                      "Posts.PostDate, " +
                      "Posts.Price, " +
                      "Posts.Title, " +
                      "Posts.WantedGender, " +
                      "COUNT(DISTINCT PostComments.Id) AS CommentsCount, " +
                      "COUNT(PostUsers.Post_Id) AS LikesCount, " +
                      "CAST(COUNT(UserLikes.Post_Id) AS BIT) AS IsLiked, " +
                      "UserAuthor.Id, " +
                      "UserAuthor.FirstName, " +
                      "UserAuthor.LastName, " +
                      "UserAuthor.ProfilePictureUrl, " +
                      "UserAuthor.AverageGrade, " +
                      "Neighborhoods.Id, " +
                      "Neighborhoods.Name, " +
                      "PostPictures.Id, " +
                      "PostPictures.PictureUrl " +
                      "FROM Posts " +
                      "INNER JOIN (SELECT UserAuthor.Id, " +
                      "UserAuthor.FirstName, " +
                      "UserAuthor.LastName, " +
                      "UserAuthor.ProfilePictureUrl, " +
                      "ISNULL(CAST(AVG(CAST(Comments.Grade AS Decimal(2,1))) AS Decimal(2,1)), 0) AS AverageGrade " +
                      "FROM Users AS UserAuthor " +
                      "LEFT JOIN (SELECT ProfileComments.UserProfile_Id, ProfileComments.Grade " +
                      "FROM ProfileComments) Comments " +
                      "ON UserAuthor.Id = Comments.UserProfile_Id " +
                      "GROUP BY UserAuthor.Id, UserAuthor.FirstName, UserAuthor.LastName, UserAuthor.ProfilePictureUrl) AS UserAuthor " +
                      "ON UserAuthor.Id = Posts.User_Id " +
                      "INNER JOIN NeighborhoodPosts " +
                      "INNER JOIN Neighborhoods " +
                      "ON Neighborhoods.Id = NeighborhoodPosts.Neighborhood_Id " +
                      "ON Posts.Id = NeighborhoodPosts.Post_Id " +
                      "LEFT JOIN PostPictures " +
                      "ON PostPictures.Post_Id = Posts.Id " +
                      "LEFT JOIN PostComments " +
                      "ON Posts.Id = PostComments.Post_Id " +
                      "LEFT JOIN PostUsers " +
                      "ON Posts.Id = PostUsers.Post_Id " +
                      "LEFT JOIN PostUsers AS UserLikes " +
                      "ON Posts.Id = UserLikes.Post_Id " +
                      "AND UserLikes.User_Id = @CurrentUserId " +
                      $"WHERE Posts.ID IN {GetPostPaging(DapperPostFlags.CurrentUser, sortOptions, orderOption, accomodationOptions, minPrice, maxPrice, city, keyword)} " +
                      "GROUP BY Posts.Id, " +
                      "Posts.AccomodationOption, " +
                      "Posts.AccomodationType, " +
                      "Posts.ArePetsAllowed, " +
                      "Posts.Description, " +
                      "Posts.IsSmokerAllowed, " +
                      "Posts.NumberOfRoommates, " +
                      "Posts.PetOptions, " +
                      "Posts.PostDate, " +
                      "Posts.Price, " +
                      "Posts.Title, " +
                      "Posts.WantedGender, " +
                      "UserAuthor.Id, " +
                      "UserAuthor.FirstName, " +
                      "UserAuthor.LastName, " +
                      "UserAuthor.ProfilePictureUrl, " +
                      "UserAuthor.AverageGrade, " +
                      "Neighborhoods.Id, " +
                      "Neighborhoods.Name, " +
                      "PostPictures.Id, " +
                      "PostPictures.PictureUrl, " +
                      "PostPictures.Post_Id " +
                      GetPostSortingSql(sortOptions, orderOption);
            var connection = context.Database.Connection as SqlConnection;
            var postDictionary = new Dictionary<Guid, PostViewModel>();
            return (await connection.QueryAsync<PostViewModel, UserViewModel, Neighborhood, PostPicture, PostViewModel>(
                sql, (post, user, neighborhood, postPicture) =>
                {
                    if (!postDictionary.TryGetValue(post.Id, out var outPost))
                    {
                        outPost = post;
                        outPost.User = user;
                        postDictionary.Add(outPost.Id, outPost);
                    }

                    if (neighborhood != null && !outPost.Neighborhoods.Contains(neighborhood))
                        outPost.Neighborhoods.Add(neighborhood);
                    if (postPicture != null && !outPost.PostPictures.Contains(postPicture))
                        outPost.PostPictures.Add(postPicture);
                    return outPost;
                },
                new
                {
                    AccomodationOption = (int?) accomodationOptions,
                    MinPrice = minPrice,
                    MaxPrice = maxPrice,
                    City = city,
                    Keyword = keyword,
                    DateParam = date,
                    NumberToTake = numberToTake,
                    PagingModifier = pagingModifier,
                    CurrentUserId = currentUserId
                })).Distinct();
        }

        public static async Task<IEnumerable<PostViewModel>> GetPostsByUserAsync(this RoomMateExpressDbContext context, Guid userId, string keyword = null)
        {
            var sql = "SELECT Posts.Id, " +
                      "Posts.AccomodationOption, " +
                      "Posts.AccomodationType, " +
                      "Posts.ArePetsAllowed, " +
                      "Posts.Description, " +
                      "Posts.IsSmokerAllowed, " +
                      "Posts.NumberOfRoommates, " +
                      "Posts.PetOptions, " +
                      "Posts.PostDate, " +
                      "Posts.Price, " +
                      "Posts.Title, " +
                      "Posts.WantedGender, " +
                      "COUNT(DISTINCT PostComments.Id) AS CommentsCount, " +
                      "COUNT(PostUsers.Post_Id) AS LikesCount, " +
                      "UserAuthor.Id, " +
                      "UserAuthor.FirstName, " +
                      "UserAuthor.LastName, " +
                      "UserAuthor.ProfilePictureUrl, " +
                      "UserAuthor.AverageGrade, " +
                      "Neighborhoods.Id, " +
                      "Neighborhoods.Name, " +
                      "PostPictures.Id, " +
                      "PostPictures.PictureUrl " +
                      "FROM Posts " +
                      "INNER JOIN (SELECT UserAuthor.Id, " +
                      "UserAuthor.FirstName, " +
                      "UserAuthor.LastName, " +
                      "UserAuthor.ProfilePictureUrl, " +
                      "ISNULL(CAST(AVG(CAST(Comments.Grade AS Decimal(2,1))) AS Decimal(2,1)), 0) AS AverageGrade " +
                      "FROM Users AS UserAuthor " +
                      "LEFT JOIN (SELECT ProfileComments.UserProfile_Id, ProfileComments.Grade " +
                      "FROM ProfileComments) Comments " +
                      "ON UserAuthor.Id = Comments.UserProfile_Id " +
                      "GROUP BY UserAuthor.Id, UserAuthor.FirstName, UserAuthor.LastName, UserAuthor.ProfilePictureUrl) AS UserAuthor " +
                      "ON UserAuthor.Id = Posts.User_Id " +
                      "AND UserAuthor.Id = @UserId " +
                      "INNER JOIN NeighborhoodPosts " +
                      "INNER JOIN Neighborhoods " +
                      "ON Neighborhoods.Id = NeighborhoodPosts.Neighborhood_Id " +
                      "ON Posts.Id = NeighborhoodPosts.Post_Id " +
                      "LEFT JOIN PostPictures " +
                      "ON PostPictures.Post_Id = Posts.Id " +
                      "LEFT JOIN PostComments " +
                      "ON Posts.Id = PostComments.Post_Id " +
                      "LEFT JOIN PostUsers " +
                      "ON Posts.Id = PostUsers.Post_Id " +
                      GetPostFilterSql(keyword:keyword) +
                      "GROUP BY Posts.Id, " +
                      "Posts.AccomodationOption, " +
                      "Posts.AccomodationType, " +
                      "Posts.ArePetsAllowed, " +
                      "Posts.Description, " +
                      "Posts.IsSmokerAllowed, " +
                      "Posts.NumberOfRoommates, " +
                      "Posts.PetOptions, " +
                      "Posts.PostDate, " +
                      "Posts.Price, " +
                      "Posts.Title, " +
                      "Posts.WantedGender, " +
                      "UserAuthor.Id, " +
                      "UserAuthor.FirstName, " +
                      "UserAuthor.LastName, " +
                      "UserAuthor.ProfilePictureUrl, " +
                      "UserAuthor.AverageGrade, " +
                      "Neighborhoods.Id, " +
                      "Neighborhoods.Name, " +
                      "PostPictures.Id, " +
                      "PostPictures.PictureUrl, " +
                      "PostPictures.Post_Id " +
                      "ORDER BY Posts.PostDate DESC;";
            var connection = context.Database.Connection as SqlConnection;
            var postDictionary = new Dictionary<Guid, PostViewModel>();
            return (await connection.QueryAsync<PostViewModel, UserViewModel, Neighborhood, PostPicture, PostViewModel>(sql, (post, user, neighborhood, postPicture) =>
            {
                if (!postDictionary.TryGetValue(post.Id, out var outPost))
                {
                    outPost = post;
                    outPost.User = user;
                    postDictionary.Add(outPost.Id, outPost);
                }
                if (neighborhood != null && !outPost.Neighborhoods.Contains(neighborhood))
                    outPost.Neighborhoods.Add(neighborhood);
                if (postPicture != null && !outPost.PostPictures.Contains(postPicture))
                    outPost.PostPictures.Add(postPicture);
                return outPost;
            }, new { UserId = userId })).Distinct();
        }

        public static async Task<IEnumerable<PostViewModel>> GetPostsByUserAsync(this RoomMateExpressDbContext context,
            Guid userId, DateTimeOffset date, 
            int numberToTake, string keyword = null)
        {
            var sql = "SELECT Posts.Id, " +
                      "Posts.AccomodationOption, " +
                      "Posts.AccomodationType, " +
                      "Posts.ArePetsAllowed, " +
                      "Posts.Description, " +
                      "Posts.IsSmokerAllowed, " +
                      "Posts.NumberOfRoommates, " +
                      "Posts.PetOptions, " +
                      "Posts.PostDate, " +
                      "Posts.Price, " +
                      "Posts.Title, " +
                      "Posts.WantedGender, " +
                      "COUNT(DISTINCT PostComments.Id) AS CommentsCount, " +
                      "COUNT(PostUsers.Post_Id) AS LikesCount, " +
                      "UserAuthor.Id, " +
                      "UserAuthor.FirstName, " +
                      "UserAuthor.LastName, " +
                      "UserAuthor.ProfilePictureUrl, " +
                      "UserAuthor.AverageGrade, " +
                      "Neighborhoods.Id, " +
                      "Neighborhoods.Name, " +
                      "PostPictures.Id, " +
                      "PostPictures.PictureUrl " +
                      "FROM Posts " +
                      "INNER JOIN (SELECT UserAuthor.Id, " +
                      "UserAuthor.FirstName, " +
                      "UserAuthor.LastName, " +
                      "UserAuthor.ProfilePictureUrl, " +
                      "ISNULL(CAST(AVG(CAST(Comments.Grade AS Decimal(2,1))) AS Decimal(2,1)), 0) AS AverageGrade " +
                      "FROM Users AS UserAuthor " +
                      "LEFT JOIN (SELECT ProfileComments.UserProfile_Id, ProfileComments.Grade " +
                      "FROM ProfileComments) Comments " +
                      "ON UserAuthor.Id = Comments.UserProfile_Id " +
                      "GROUP BY UserAuthor.Id, UserAuthor.FirstName, UserAuthor.LastName, UserAuthor.ProfilePictureUrl) AS UserAuthor " +
                      "ON UserAuthor.Id = Posts.User_Id " +
                      "INNER JOIN NeighborhoodPosts " +
                      "INNER JOIN Neighborhoods " +
                      "ON Neighborhoods.Id = NeighborhoodPosts.Neighborhood_Id " +
                      "ON Posts.Id = NeighborhoodPosts.Post_Id " +
                      "LEFT JOIN PostPictures " +
                      "ON PostPictures.Post_Id = Posts.Id " +
                      "LEFT JOIN PostComments " +
                      "ON Posts.Id = PostComments.Post_Id " +
                      "LEFT JOIN PostUsers " +
                      "ON Posts.Id = PostUsers.Post_Id " +
                      $"WHERE Posts.ID IN {GetPostPaging(DapperPostFlags.User, PostSortOptions.Date, SortOrderOption.Descending, keyword: keyword)} " +
                      "GROUP BY Posts.Id, " +
                      "Posts.AccomodationOption, " +
                      "Posts.AccomodationType, " +
                      "Posts.ArePetsAllowed, " +
                      "Posts.Description, " +
                      "Posts.IsSmokerAllowed, " +
                      "Posts.NumberOfRoommates, " +
                      "Posts.PetOptions, " +
                      "Posts.PostDate, " +
                      "Posts.Price, " +
                      "Posts.Title, " +
                      "Posts.WantedGender, " +
                      "UserAuthor.Id, " +
                      "UserAuthor.FirstName, " +
                      "UserAuthor.LastName, " +
                      "UserAuthor.ProfilePictureUrl, " +
                      "UserAuthor.AverageGrade, " +
                      "Neighborhoods.Id, " +
                      "Neighborhoods.Name, " +
                      "PostPictures.Id, " +
                      "PostPictures.PictureUrl, " +
                      "PostPictures.Post_Id " +
                      GetPostSortingSql(PostSortOptions.Date, SortOrderOption.Descending);
            var connection = context.Database.Connection as SqlConnection;
            var postDictionary = new Dictionary<Guid, PostViewModel>();
            return (await connection.QueryAsync<PostViewModel, UserViewModel, Neighborhood, PostPicture, PostViewModel>(sql, (post, user, neighborhood, postPicture) =>
            {
                if (!postDictionary.TryGetValue(post.Id, out var outPost))
                {
                    outPost = post;
                    outPost.User = user;
                    postDictionary.Add(outPost.Id, outPost);
                }
                if (neighborhood != null && !outPost.Neighborhoods.Contains(neighborhood))
                    outPost.Neighborhoods.Add(neighborhood);
                if (postPicture != null && !outPost.PostPictures.Contains(postPicture))
                    outPost.PostPictures.Add(postPicture);
                return outPost;
            }, new { UserId = userId, Keyword = keyword, DateParam = date, NumberToTake = numberToTake })).Distinct();
        }

        public static async Task<IEnumerable<PostViewModel>> GetPostsByUserAsync(this RoomMateExpressDbContext context, Guid userId, Guid currentUserId, string keyword = null)
        {
            var sql = "SELECT Posts.Id, " +
                      "Posts.AccomodationOption, " +
                      "Posts.AccomodationType, " +
                      "Posts.ArePetsAllowed, " +
                      "Posts.Description, " +
                      "Posts.IsSmokerAllowed, " +
                      "Posts.NumberOfRoommates, " +
                      "Posts.PetOptions, " +
                      "Posts.PostDate, " +
                      "Posts.Price, " +
                      "Posts.Title, " +
                      "Posts.WantedGender, " +
                      "COUNT(DISTINCT PostComments.Id) AS CommentsCount, " +
                      "COUNT(PostUsers.Post_Id) AS LikesCount, " +
                      "CAST(COUNT(UserLikes.Post_Id) AS BIT) AS IsLiked, " +
                      "UserAuthor.Id, " +
                      "UserAuthor.FirstName, " +
                      "UserAuthor.LastName, " +
                      "UserAuthor.ProfilePictureUrl, " +
                      "UserAuthor.AverageGrade, " +
                      "Neighborhoods.Id, " +
                      "Neighborhoods.Name, " +
                      "PostPictures.Id, " +
                      "PostPictures.PictureUrl " +
                      "FROM Posts " +
                      "INNER JOIN (SELECT UserAuthor.Id, " +
                      "UserAuthor.FirstName, " +
                      "UserAuthor.LastName, " +
                      "UserAuthor.ProfilePictureUrl, " +
                      "ISNULL(CAST(AVG(CAST(Comments.Grade AS Decimal(2,1))) AS Decimal(2,1)), 0) AS AverageGrade " +
                      "FROM Users AS UserAuthor " +
                      "LEFT JOIN (SELECT ProfileComments.UserProfile_Id, ProfileComments.Grade " +
                      "FROM ProfileComments) Comments " +
                      "ON UserAuthor.Id = Comments.UserProfile_Id " +
                      "GROUP BY UserAuthor.Id, UserAuthor.FirstName, UserAuthor.LastName, UserAuthor.ProfilePictureUrl) AS UserAuthor " +
                      "ON UserAuthor.Id = Posts.User_Id " +
                      "AND UserAuthor.Id = @UserId " +
                      "AND UserAuthor.Id <> @CurrentUserId " +
                      "INNER JOIN NeighborhoodPosts " +
                      "INNER JOIN Neighborhoods " +
                      "ON Neighborhoods.Id = NeighborhoodPosts.Neighborhood_Id " +
                      "ON Posts.Id = NeighborhoodPosts.Post_Id " +
                      "LEFT JOIN PostPictures " +
                      "ON PostPictures.Post_Id = Posts.Id " +
                      "LEFT JOIN PostComments " +
                      "ON Posts.Id = PostComments.Post_Id " +
                      "LEFT JOIN PostUsers " +
                      "ON Posts.Id = PostUsers.Post_Id " +
                      "LEFT JOIN PostUsers AS UserLikes " +
                      "ON Posts.Id = UserLikes.Post_Id " +
                      "AND UserLikes.User_Id = @CurrentUserId " +
                      GetPostFilterSql(keyword: keyword) +
                      "GROUP BY Posts.Id, " +
                      "Posts.AccomodationOption, " +
                      "Posts.AccomodationType, " +
                      "Posts.ArePetsAllowed, " +
                      "Posts.Description, " +
                      "Posts.IsSmokerAllowed, " +
                      "Posts.NumberOfRoommates, " +
                      "Posts.PetOptions, " +
                      "Posts.PostDate, " +
                      "Posts.Price, " +
                      "Posts.Title, " +
                      "Posts.WantedGender, " +
                      "UserAuthor.Id, " +
                      "UserAuthor.FirstName, " +
                      "UserAuthor.LastName, " +
                      "UserAuthor.ProfilePictureUrl, " +
                      "UserAuthor.AverageGrade, " +
                      "Neighborhoods.Id, " +
                      "Neighborhoods.Name, " +
                      "PostPictures.Id, " +
                      "PostPictures.PictureUrl, " +
                      "PostPictures.Post_Id " +
                      "ORDER BY Posts.PostDate DESC;";
            var connection = context.Database.Connection as SqlConnection;
            var postDictionary = new Dictionary<Guid, PostViewModel>();
            return (await connection.QueryAsync<PostViewModel, UserViewModel, Neighborhood, PostPicture, PostViewModel>(sql, (post, user, neighborhood, postPicture) =>
            {
                if (!postDictionary.TryGetValue(post.Id, out var outPost))
                {
                    outPost = post;
                    outPost.User = user;
                    postDictionary.Add(outPost.Id, outPost);
                }
                if (neighborhood != null && !outPost.Neighborhoods.Contains(neighborhood))
                    outPost.Neighborhoods.Add(neighborhood);
                if (postPicture != null && !outPost.PostPictures.Contains(postPicture))
                    outPost.PostPictures.Add(postPicture);
                return outPost;
            }, new { UserId = userId, CurrentUserId = currentUserId })).Distinct();
        }

        public static async Task<IEnumerable<PostViewModel>> GetPostsByUserAsync(this RoomMateExpressDbContext context,
            Guid userId, Guid currentUserId, DateTimeOffset date,
            int numberToTake, string keyword = null)
        {
            var sql = "SELECT Posts.Id, " +
                      "Posts.AccomodationOption, " +
                      "Posts.AccomodationType, " +
                      "Posts.ArePetsAllowed, " +
                      "Posts.Description, " +
                      "Posts.IsSmokerAllowed, " +
                      "Posts.NumberOfRoommates, " +
                      "Posts.PetOptions, " +
                      "Posts.PostDate, " +
                      "Posts.Price, " +
                      "Posts.Title, " +
                      "Posts.WantedGender, " +
                      "COUNT(DISTINCT PostComments.Id) AS CommentsCount, " +
                      "COUNT(PostUsers.Post_Id) AS LikesCount, " +
                      "CAST(COUNT(UserLikes.Post_Id) AS BIT) AS IsLiked, " +
                      "UserAuthor.Id, " +
                      "UserAuthor.FirstName, " +
                      "UserAuthor.LastName, " +
                      "UserAuthor.ProfilePictureUrl, " +
                      "UserAuthor.AverageGrade, " +
                      "Neighborhoods.Id, " +
                      "Neighborhoods.Name, " +
                      "PostPictures.Id, " +
                      "PostPictures.PictureUrl " +
                      "FROM Posts " +
                      "INNER JOIN (SELECT UserAuthor.Id, " +
                      "UserAuthor.FirstName, " +
                      "UserAuthor.LastName, " +
                      "UserAuthor.ProfilePictureUrl, " +
                      "ISNULL(CAST(AVG(CAST(Comments.Grade AS Decimal(2,1))) AS Decimal(2,1)), 0) AS AverageGrade " +
                      "FROM Users AS UserAuthor " +
                      "LEFT JOIN (SELECT ProfileComments.UserProfile_Id, ProfileComments.Grade " +
                      "FROM ProfileComments) Comments " +
                      "ON UserAuthor.Id = Comments.UserProfile_Id " +
                      "GROUP BY UserAuthor.Id, UserAuthor.FirstName, UserAuthor.LastName, UserAuthor.ProfilePictureUrl) AS UserAuthor " +
                      "ON UserAuthor.Id = Posts.User_Id " +
                      "INNER JOIN NeighborhoodPosts " +
                      "INNER JOIN Neighborhoods " +
                      "ON Neighborhoods.Id = NeighborhoodPosts.Neighborhood_Id " +
                      "ON Posts.Id = NeighborhoodPosts.Post_Id " +
                      "LEFT JOIN PostPictures " +
                      "ON PostPictures.Post_Id = Posts.Id " +
                      "LEFT JOIN PostComments " +
                      "ON Posts.Id = PostComments.Post_Id " +
                      "LEFT JOIN PostUsers " +
                      "ON Posts.Id = PostUsers.Post_Id " +
                      "LEFT JOIN PostUsers AS UserLikes " +
                      "ON Posts.Id = UserLikes.Post_Id " +
                      "AND UserLikes.User_Id = @CurrentUserId " +
                      $"WHERE Posts.ID IN {GetPostPaging(DapperPostFlags.User, PostSortOptions.Date, SortOrderOption.Descending, keyword: keyword)} " +
                      "GROUP BY Posts.Id, " +
                      "Posts.AccomodationOption, " +
                      "Posts.AccomodationType, " +
                      "Posts.ArePetsAllowed, " +
                      "Posts.Description, " +
                      "Posts.IsSmokerAllowed, " +
                      "Posts.NumberOfRoommates, " +
                      "Posts.PetOptions, " +
                      "Posts.PostDate, " +
                      "Posts.Price, " +
                      "Posts.Title, " +
                      "Posts.WantedGender, " +
                      "UserAuthor.Id, " +
                      "UserAuthor.FirstName, " +
                      "UserAuthor.LastName, " +
                      "UserAuthor.ProfilePictureUrl, " +
                      "UserAuthor.AverageGrade, " +
                      "Neighborhoods.Id, " +
                      "Neighborhoods.Name, " +
                      "PostPictures.Id, " +
                      "PostPictures.PictureUrl, " +
                      "PostPictures.Post_Id " +
                      GetPostSortingSql(PostSortOptions.Date, SortOrderOption.Descending);
            var connection = context.Database.Connection as SqlConnection;
            var postDictionary = new Dictionary<Guid, PostViewModel>();
            return (await connection.QueryAsync<PostViewModel, UserViewModel, Neighborhood, PostPicture, PostViewModel>(sql, (post, user, neighborhood, postPicture) =>
            {
                if (!postDictionary.TryGetValue(post.Id, out var outPost))
                {
                    outPost = post;
                    outPost.User = user;
                    postDictionary.Add(outPost.Id, outPost);
                }
                if (neighborhood != null && !outPost.Neighborhoods.Contains(neighborhood))
                    outPost.Neighborhoods.Add(neighborhood);
                if (postPicture != null && !outPost.PostPictures.Contains(postPicture))
                    outPost.PostPictures.Add(postPicture);
                return outPost;
            }, new { UserId = userId, CurrentUserId = currentUserId, Keyword = keyword, DateParam = date, NumberToTake = numberToTake })).Distinct();
        }

        public static async Task<PostViewModel> LikeDislikePost(this RoomMateExpressDbContext context, Guid postId,
            Guid userId)
        {
            const string sql = "IF EXISTS(SELECT PostUsers.Post_Id FROM PostUsers WHERE PostUsers.Post_Id = @PostId AND PostUsers.User_Id = @UserId) " +
                               "DELETE FROM PostUsers WHERE PostUsers.Post_Id = @PostId AND PostUsers.User_Id = @UserId " +
                               "ELSE " +
                               "INSERT INTO PostUsers (PostUsers.Post_Id,PostUsers.User_Id) VALUES (@PostId, @UserId);";
            var connection = context.Database.Connection as SqlConnection;
            await connection.ExecuteAsync(sql, new {PostId = postId, UserId = userId});
            return await GetPostAsync(context, postId, userId);
        }

        public static async Task<PostComment> InsertPostComment(this RoomMateExpressDbContext context,
            PostComment comment)
        {
            const string sql =
                "INSERT INTO PostComments (PostComments.Id, PostComments.Text, PostComments.CommentedAt, PostComments.Post_Id, PostComments.User_Id) " +
                "VALUES (@Id, @Text, @CommentedAt, @PostId, @UserId);";
            var connection = context.Database.Connection as SqlConnection;
            await connection.ExecuteAsync(sql, new { comment.Id, comment.Text, comment.CommentedAt, PostId = comment.Post.Id, UserId = comment.User.Id });
            return await GetPostCommentAsync(context, comment.Id);
        }

        public static async Task<PostComment> GetPostCommentAsync(this RoomMateExpressDbContext context, Guid id)
        {
            const string sql = "SELECT PostComments.Id, " +
                               "PostComments.Text, " +
                               "PostComments.CommentedAt, " +
                               "Users.Id," +
                               "Users.FirstName, " +
                               "Users.LastName, " +
                               "Users.ProfilePictureUrl " +
                               "FROM PostComments " +
                               "INNER JOIN Users " +
                               "ON Users.Id = PostComments.User_Id " +
                               "WHERE PostComments.Id = @Id;";
            var connection = context.Database.Connection as SqlConnection;
            return (await connection.QueryAsync<PostComment, User, PostComment>(sql, (comment, user) =>
            {
                comment.User = user;
                return comment;
            }, new { Id = id })).FirstOrDefault();
        }

        public static async Task<IEnumerable<PostComment>> GetPostCommentsAsync(this RoomMateExpressDbContext context,
            Guid postId)
        {
            const string sql = "SELECT PostComments.Id, " +
                               "PostComments.Text, " +
                               "PostComments.CommentedAt, " +
                               "Users.Id," +
                               "Users.FirstName, " +
                               "Users.LastName, " +
                               "Users.ProfilePictureUrl " +
                               "FROM PostComments " +
                               "INNER JOIN Users " +
                               "ON Users.Id = PostComments.User_Id " +
                               "WHERE PostComments.Post_Id = @Id " +
                               "ORDER BY PostComments.CommentedAt DESC;";
            var connection = context.Database.Connection as SqlConnection;
            return (await connection.QueryAsync<PostComment, User, PostComment>(sql, (comment, user) =>
            {
                comment.User = user;
                return comment;
            }, new { Id = postId })).Distinct();
        }

        public static async Task<IEnumerable<PostComment>> GetPostCommentsAsync(this RoomMateExpressDbContext context,
            Guid postId, DateTimeOffset date, int numberToTake)
        {
            const string sql = "SELECT PostComments.Id, " +
                               "PostComments.Text, " +
                               "PostComments.CommentedAt, " +
                               "Users.Id," +
                               "Users.FirstName, " +
                               "Users.LastName, " +
                               "Users.ProfilePictureUrl " +
                               "FROM PostComments " +
                               "INNER JOIN Users " +
                               "ON Users.Id = PostComments.User_Id " +
                               "WHERE PostComments.Post_Id = @Id " +
                               "AND PostComments.CommentedAt < @DateParam " +
                               "ORDER BY PostComments.CommentedAt DESC " +
                               "OFFSET 0 ROWS " +
                               "FETCH FIRST @NumberToTake ROWS ONLY;";
            var connection = context.Database.Connection as SqlConnection;
            return (await connection.QueryAsync<PostComment, User, PostComment>(sql, (comment, user) =>
            {
                comment.User = user;
                return comment;
            }, new { Id = postId, DateParam = date, NumberToTake = numberToTake })).Distinct();
        }

        #region Helpers

        private static string GetPostSortingSql(PostSortOptions sortOptions, SortOrderOption orderOption)
        {
            switch (sortOptions)
            {
                case PostSortOptions.Date:
                    if (orderOption == SortOrderOption.Ascending)
                        return "ORDER BY Posts.PostDate ASC;";
                    return "ORDER BY Posts.PostDate DESC;";
                case PostSortOptions.Popularity:
                    if (orderOption == SortOrderOption.Ascending)
                        return "ORDER BY LikesCount ASC, Posts.PostDate DESC;";
                    return "ORDER BY LikesCount DESC, Posts.PostDate DESC;";
                case PostSortOptions.Price:
                    if (orderOption == SortOrderOption.Ascending)
                        return "ORDER BY Posts.Price ASC, Posts.PostDate DESC;";
                    return "ORDER BY Posts.Price DESC, Posts.PostDate DESC;";
                case PostSortOptions.UserRating:
                    if (orderOption == SortOrderOption.Ascending)
                        return "ORDER BY UserAuthor.AverageGrade ASC, Posts.PostDate DESC;";
                    return "ORDER BY UserAuthor.AverageGrade DESC, Posts.PostDate DESC;";
                default:
                    throw new ArgumentOutOfRangeException(nameof(sortOptions), sortOptions, null);
            }
        }

        private static string GetPostFilterSql(AccomodationOptions? accomodationOptions = null,
            decimal? minPrice = null, decimal? maxPrice = null, string city = null, string keyword = null)
        {
            var sb = new StringBuilder();
            if (accomodationOptions.HasValue)
                sb.Append("WHERE Posts.AccomodationOption = @AccomodationOption ");

            if (minPrice.HasValue)
                sb.Append(sb.Length == 0 ? "WHERE Posts.Price > @MinPrice " : "AND Posts.Price > @MinPrice ");

            if (maxPrice.HasValue)
                sb.Append(sb.Length == 0 ? "WHERE Posts.Price < @MaxPrice " : "AND Posts.Price < @MaxPrice ");

            if (city != null)
                sb.Append(sb.Length == 0
                    ? "WHERE Posts.Id IN (SELECT P.Id FROM Posts AS P " +
                      "INNER JOIN NeighborhoodPosts AS NP " +
                      "INNER JOIN Neighborhoods AS N " +
                      "INNER JOIN Cities AS C " +
                      "ON C.Id = N.City_Id AND C.Name LIKE @City " +
                      "ON N.Id = NP.Neighborhood_Id " +
                      "ON P.Id = NP.Post_Id) "
                    : "AND Posts.Id IN (SELECT P.Id FROM Posts AS P " +
                      "INNER JOIN NeighborhoodPosts AS NP " +
                      "INNER JOIN Neighborhoods AS N " +
                      "INNER JOIN Cities AS C " +
                      "ON C.Id = N.City_Id AND C.Name LIKE @City " +
                      "ON N.Id = NP.Neighborhood_Id " +
                      "ON P.Id = NP.Post_Id) ");

            if (keyword != null)
            {
                sb.Append(sb.Length == 0
                    ? "WHERE (Posts.Title LIKE '%' + @Keyword + '%' " +
                      "OR Posts.Description LIKE '%' + @Keyword + '%' " +
                      "OR UserAuthor.FirstName + ' ' + UserAuthor.LastName LIKE '%' + @Keyword + '%') "
                    : "AND (Posts.Title LIKE '%' + @Keyword + '%' " +
                      "OR Posts.Description LIKE '%' + @Keyword + '%' " +
                      "OR UserAuthor.FirstName + ' ' + UserAuthor.LastName LIKE '%' + @Keyword + '%') ");
            }

            return sb.ToString();
        }

        private static string GetPostFilterPagingSql(AccomodationOptions? accomodationOptions = null,
            decimal? minPrice = null, decimal? maxPrice = null, string city = null, string keyword = null)
        {
            var sb = new StringBuilder();
            if (accomodationOptions.HasValue)
                sb.Append("AND PPage.AccomodationOption = @AccomodationOption ");

            if (minPrice.HasValue)
                sb.Append("AND PPage.Price > @MinPrice ");

            if (maxPrice.HasValue)
                sb.Append("AND PPage.Price < @MaxPrice ");

            if (city != null)
                sb.Append(
                      "AND PPage.Id IN (SELECT P.Id FROM Posts AS P " +
                      "INNER JOIN NeighborhoodPosts AS NP " +
                      "INNER JOIN Neighborhoods AS N " +
                      "INNER JOIN Cities AS C " +
                      "ON C.Id = N.City_Id AND C.Name LIKE @City " +
                      "ON N.Id = NP.Neighborhood_Id " +
                      "ON P.Id = NP.Post_Id) ");

            if (keyword != null)
            {
                sb.Append(
                      "AND (PPage.Title LIKE '%' + @Keyword + '%' " +
                      "OR PPage.Description LIKE '%' + @Keyword + '%' " +
                      "OR UAPage.FirstName + ' ' + UAPage.LastName LIKE '%' + @Keyword + '%') ");
            }

            return sb.ToString();
        }

        private static string GetPostPaging(DapperPostFlags postFlags, PostSortOptions sortOptions, SortOrderOption orderOption, AccomodationOptions? accomodationOptions = null,
            decimal? minPrice = null, decimal? maxPrice = null, string city = null, string keyword = null)
        {
            switch (sortOptions)
            {
                case PostSortOptions.Date:
                    if (orderOption == SortOrderOption.Ascending)
                        return "(SELECT Id FROM " +
                               "(SELECT PPage.Id, DENSE_RANK() OVER(ORDER BY PPage.PostDate ASC) AS RankNum " +
                                        "FROM Posts AS PPage " +
                                        "INNER JOIN Users AS UAPage " +
                                        "ON UAPage.Id = PPage.User_Id " +
                                        (postFlags.HasFlag(DapperPostFlags.CurrentUser) ? "AND UAPage.Id <> @CurrentUserId " : string.Empty) +
                                        (postFlags.HasFlag(DapperPostFlags.User) ? "AND UAPage.Id = @UserId " : string.Empty) +
                               "WHERE PPage.PostDate > @DateParam " +
                               $"{GetPostFilterPagingSql(accomodationOptions, minPrice, maxPrice, city, keyword)}) AS PPage " +
                               "WHERE PPage.RankNum <= @NumberToTake)  ";
                    return "(SELECT Id FROM " +
                               "(SELECT PPage.Id, DENSE_RANK() OVER(ORDER BY PPage.PostDate DESC) AS RankNum " +
                                        "FROM Posts AS PPage " +
                                        "INNER JOIN Users AS UAPage " +
                                        "ON UAPage.Id = PPage.User_Id " +
                                        (postFlags.HasFlag(DapperPostFlags.CurrentUser) ? "AND UAPage.Id <> @CurrentUserId " : string.Empty) +
                                        (postFlags.HasFlag(DapperPostFlags.User) ? "AND UAPage.Id = @UserId " : string.Empty) +
                               "WHERE PPage.PostDate < @DateParam " +
                               $"{GetPostFilterPagingSql(accomodationOptions, minPrice, maxPrice, city, keyword)}) AS PPage " +
                               "WHERE PPage.RankNum <= @NumberToTake)  ";
                case PostSortOptions.Popularity:
                    if (orderOption == SortOrderOption.Ascending)
                        return "(SELECT Id FROM " +
                               "(SELECT PPage.Id, DENSE_RANK() OVER(ORDER BY COUNT(PUPage.User_Id) ASC, Posts.PostDate DESC) AS RankNum " +
                                        "FROM Posts AS PPage " +
                                        "INNER JOIN Users AS UAPage " +
                                        "ON UAPage.Id = PPage.User_Id " +
                                        (postFlags.HasFlag(DapperPostFlags.CurrentUser) ? "AND UAPage.Id <> @CurrentUserId " : string.Empty) +
                                        (postFlags.HasFlag(DapperPostFlags.User) ? "AND UAPage.Id = @UserId " : string.Empty) +
                               "LEFT JOIN PostUsers AS PUPage " +
                               "ON PPage.Id = PUPage.Post_Id " +
                               (string.IsNullOrWhiteSpace(GetPostFilterPagingSql(accomodationOptions, minPrice, maxPrice, city, keyword)) ? string.Empty :
                               $"WHERE 1=1 {GetPostFilterPagingSql(accomodationOptions, minPrice, maxPrice, city, keyword)}") +
                               "GROUP BY PPage.Id, PPage.PostDate " +
                               "HAVING (COUNT(PUPage.User_Id) = @PagingModifier AND PPage.PostDate < @DateParam OR COUNT(PUPage.Post_Id) > @PagingModifier)) AS PPage " +
                               "WHERE PPage.RankNum <= @NumberToTake)  ";
                    return "(SELECT Id FROM " +
                               "(SELECT PPage.Id, DENSE_RANK() OVER(ORDER BY COUNT(PUPage.User_Id) DESC, Posts.PostDate DESC) AS RankNum " +
                                        "FROM Posts AS PPage " +
                                        "INNER JOIN Users AS UAPage " +
                                        "ON UAPage.Id = PPage.User_Id " +
                                        (postFlags.HasFlag(DapperPostFlags.CurrentUser) ? "AND UAPage.Id <> @CurrentUserId " : string.Empty) +
                                        (postFlags.HasFlag(DapperPostFlags.User) ? "AND UAPage.Id = @UserId " : string.Empty) +
                               "LEFT JOIN PostUsers AS PUPage " +
                               "ON PPage.Id = PUPage.Post_Id " +
                               (string.IsNullOrWhiteSpace(GetPostFilterPagingSql(accomodationOptions, minPrice, maxPrice, city, keyword)) ? string.Empty :
                               $"WHERE 1=1 {GetPostFilterPagingSql(accomodationOptions, minPrice, maxPrice, city, keyword)}") +
                               "GROUP BY PPage.Id, PPage.PostDate " +
                               "HAVING (COUNT(PUPage.User_Id) = @PagingModifier AND PPage.PostDate < @DateParam OR COUNT(PUPage.Post_Id) < @PagingModifier)) AS PPage " +
                               "WHERE PPage.RankNum <= @NumberToTake)  ";
                case PostSortOptions.Price:
                    if (orderOption == SortOrderOption.Ascending)
                        return "(SELECT Id FROM " +
                               "(SELECT PPage.Id, DENSE_RANK() OVER(ORDER BY PPage.Price ASC, PPage.PostDate Desc) AS RankNum " +
                                        "FROM Posts AS PPage " +
                                        "INNER JOIN Users AS UAPage " +
                                        "ON UAPage.Id = PPage.User_Id " +
                                        (postFlags.HasFlag(DapperPostFlags.CurrentUser) ? "AND UAPage.Id <> @CurrentUserId " : string.Empty) +
                                        (postFlags.HasFlag(DapperPostFlags.User) ? "AND UAPage.Id = @UserId " : string.Empty) +
                               "WHERE (PPage.Price = @PagingModifier AND PPage.PostDate < @DateParam OR PPage.Price > @PagingModifier) " +
                               $"{GetPostFilterPagingSql(accomodationOptions, minPrice, maxPrice, city, keyword)}) AS PPage " +
                               "WHERE PPage.RankNum <= @NumberToTake) ";
                    return "(SELECT Id FROM " +
                               "(SELECT PPage.Id, DENSE_RANK() OVER(ORDER BY PPage.Price DESC, PPage.PostDate Desc) AS RankNum " +
                                        "FROM Posts AS PPage " +
                                        "INNER JOIN Users AS UAPage " +
                                        "ON UAPage.Id = PPage.User_Id " +
                                        (postFlags.HasFlag(DapperPostFlags.CurrentUser) ? "AND UAPage.Id <> @CurrentUserId " : string.Empty) +
                                        (postFlags.HasFlag(DapperPostFlags.User) ? "AND UAPage.Id = @UserId " : string.Empty) +
                               "WHERE (PPage.Price = @PagingModifier AND PPage.PostDate < @DateParam OR PPage.Price < @PagingModifier) " +
                               $"{GetPostFilterPagingSql(accomodationOptions, minPrice, maxPrice, city, keyword)}) AS PPage " +
                               "WHERE PPage.RankNum <= @NumberToTake)  ";
                case PostSortOptions.UserRating:
                    if (orderOption == SortOrderOption.Ascending)
                        return "(SELECT Id FROM " +
                               "(SELECT PPage.Id, UAPage.AverageGrade, DENSE_RANK() OVER(ORDER BY UAPage.AverageGrade ASC, PPage.PostDate DESC) AS RankNum " +
                                        "FROM Posts AS PPage " +
                                        "INNER JOIN (SELECT UAPage.Id, " +
                                                        "ISNULL(CAST(AVG(CAST(CPage.Grade AS Decimal(2,1))) AS Decimal(2,1)), 0) AS AverageGrade " +
                                                    "FROM Users AS UAPage " +
                                                    "LEFT JOIN (SELECT ProfileComments.UserProfile_Id, ProfileComments.Grade " +
                                                                "FROM ProfileComments) CPage " +
                                                    "ON UAPage.Id = CPage.UserProfile_Id " +
                                                    "GROUP BY UAPage.Id) AS UAPage " +
                                        "ON UAPage.Id = PPage.User_Id " +
                                        (postFlags.HasFlag(DapperPostFlags.CurrentUser) ? "AND UAPage.Id <> @CurrentUserId " : string.Empty) +
                                        (postFlags.HasFlag(DapperPostFlags.User) ? "AND UAPage.Id = @UserId " : string.Empty) +
                               "WHERE (UAPage.AverageGrade = @PagingModifier AND PPage.PostDate < @DateParam OR UAPage.AverageGrade > @PagingModifier) " +
                               $"{GetPostFilterPagingSql(accomodationOptions, minPrice, maxPrice, city, keyword)}) AS PPage " +
                               "WHERE PPage.RankNum <= @NumberToTake)  ";
                    return "(SELECT Id FROM " +
                               "(SELECT PPage.Id, DENSE_RANK() OVER(ORDER BY UAPage.AverageGrade DESC, PPage.PostDate DESC) AS RankNum " +
                                        "FROM Posts AS PPage " +
                                        "INNER JOIN (SELECT UAPage.Id, " +
                                                        "ISNULL(CAST(AVG(CAST(CPage.Grade AS Decimal(2,1))) AS Decimal(2,1)), 0) AS AverageGrade " +
                                                    "FROM Users AS UAPage " +
                                                    "LEFT JOIN (SELECT ProfileComments.UserProfile_Id, ProfileComments.Grade " +
                                                                "FROM ProfileComments) CPage " +
                                                    "ON UAPage.Id = CPage.UserProfile_Id " +
                                                    "GROUP BY UAPage.Id) AS UAPage " +
                                        "ON UAPage.Id = PPage.User_Id " +
                                        (postFlags.HasFlag(DapperPostFlags.CurrentUser) ? "AND UAPage.Id <> @CurrentUserId " : string.Empty) +
                                        (postFlags.HasFlag(DapperPostFlags.User) ? "AND UAPage.Id = @UserId " : string.Empty) +
                               "WHERE (UAPage.AverageGrade = @PagingModifier AND PPage.PostDate < @DateParam OR UAPage.AverageGrade < @PagingModifier) " +
                               $"{GetPostFilterPagingSql(accomodationOptions, minPrice, maxPrice, city, keyword)}) AS PPage " +
                               "WHERE PPage.RankNum <= @NumberToTake)  ";
                default:
                    throw new ArgumentOutOfRangeException(nameof(sortOptions), sortOptions, null);
            }
        }

        #endregion

    }
}
