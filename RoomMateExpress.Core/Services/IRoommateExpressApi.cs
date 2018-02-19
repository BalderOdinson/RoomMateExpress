using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;
using RoomMateExpress.Core.ApiModels;
using RoomMateExpress.Core.Helpers;
using RoomMateExpress.Core.Helpers.Enums;
using RoomMateExpress.Core.Helpers.SortFilterHelpers;
using RoomMateExpress.Core.Models;

namespace RoomMateExpress.Core.Services
{
    public interface IRoommateExpressApi
    {
        [Post("/account/register")]
        Task Register([Body] AccountModel registerModel);

        [Put("/account/change-email-adress")]
        Task ChangeEmailAdress([Body] AccountModel changeEmailModel);

        [Put("/account/change-password")]
        Task ChangePassword([Body] AccountModel changePasswordModel);

        [Post("/account/forgot-password")]
        Task ForgotPassword([Body] AccountModel forgotPasswordModel);

        [Post("/account/reset-password")]
        Task ResetPassword([Body] AccountModel resetPasswordModel);

        [Post("/account/block-user/{accountId}")]
        Task BlockUser(Guid accountId, [Body] AccountModel blockUserModel);

        [Get("/account/unblock-user/{accountId}")]
        Task UnblockUser(Guid accountId);

        [Get("/account/current-user")]
        Task<LoginResult> GetCurrentApplicationUser();

        [Get("/account/resend-confirmation-email")]
        Task ResendConfirmationEmail(string emailAddress);

        [Get("/account/resend-password-reset-email")]
        Task ResendPasswordResetEmail(string emailAddress);

        [Delete("/account/delete-user")]
        Task DeleteUser(string userName);

        [Put("/admin")]
        Task<Admin> CreateAdmin([Body]Admin admin);

        [Get("/admin")]
        Task<Admin> GetAdmin(bool includeReports = false);

        [Delete("/admin/{id}")]
        Task DeleteAdmin(Guid id);

        [Get("/chat/{id}")]
        Task<Chat> GetChat(Guid id);

        [Get("/chat/user/{userId}")]
        Task<Chat> GetChatByAnotherUser(Guid userId);

        [Get("/chat")]
        Task<List<Chat>> GetChats();

        [Get("/chat/page")]
        Task<List<Chat>> GetChatsPart(string date, int numberToTake);

        [Get("/chat/search")]
        Task<List<Chat>> GetChatsByKeyword(string keyword);

        [Get("/chat/search/page")]
        Task<List<Chat>> GetChatsByKeywordPart(string keyword, string date, int numberToTake);

        [Get("/chat/new")]
        Task<List<Chat>> GetNewChats(string date, int numberToTake);

        [Post("/chat")]
        Task<Chat> CreateChat([Body] Chat chat);

        [Get("/city")]
        Task<List<City>> GetAllCities();

        [Get("/city/{id}")]
        Task<City> GetCity(Guid id);

        [Put("/city")]
        Task<City> CreateOrUpdateCity([Body] City city);

        [Delete("/city/{id}")]
        Task DeleteCity(Guid id);

        [Get("/message/{id}")]
        Task<Message> GetMessage(Guid id);

        [Get("/message/chat/{chatId}")]
        Task<List<Message>> GetMessages(Guid chatId);

        [Get("/message/chat/{chatId}/page")]
        Task<List<Message>> GetMessagesPart(Guid chatId, string date, int numberToTake);

        [Get("/message/chat/{chatId}/new/page")]
        Task<List<Message>> GetNewMessages(Guid chatId, string date, int numberToTake);

        [Post("/message")]
        Task<Message> SendMessage([Body]Message message);

        [Get("/neighborhood")]
        Task<List<Neighborhood>> GetAllNeighborhoods();

        [Get("/neighborhood/{id}")]
         Task<Neighborhood> GetNeighborhood(Guid id);

        [Get("/comment-post/{id}")]
        Task<PostComment> GetPostComment(Guid id);

        [Get("/comment-post/user/{id}/")]
        Task<List<PostComment>> GetAllCommentsByUser(Guid id);

        [Get("/comment-post/user/{id}/page")]
        Task<List<PostComment>> GetAllCommentsByUserPart(Guid id, string date, int numberToTake);

        [Get("/comment-post/post/{id}/")]
        Task<List<PostComment>> GetAllPostComments(Guid id);

        [Get("/comment-post/post/{id}/page")]
        Task<List<PostComment>> GetAllPostCommentsPart(Guid id, string date, int numberToTake);

        [Post("/comment-post")]
        Task<PostComment> CreatePostComment([Body] PostComment comment);

        [Delete("/comment-post/{id}")]
        Task DeletePostComment(Guid id);

        [Get("/post/{id}")]
        Task<Post> GetPost(Guid id);

        [Get("/post/current")]
        Task<List<Post>> GetAllPosts(int sortOptions = (int)PostSortOptions.Date,
            int orderOption = (int)SortOrderOption.Descending, int? accomodationOptions = null,
            string minPrice = null, string maxPrice = null, string city = null, string keyword = null);

        [Get("/post/current/page")]
        Task<List<Post>> GetAllPostsPart(string date, string pagingModifier, int numberToTake, int sortOptions = (int)PostSortOptions.Date,
            int orderOption = (int)SortOrderOption.Descending, int? accomodationOptions = null,
            string minPrice = null, string maxPrice = null, string city = null, string keyword = null);

        [Get("/post/user/{userId}")]
        Task<List<Post>> GetPosts(Guid userId, string keyword);

        [Get("/post/user/{userId}/page")]
        Task<List<Post>> GetPostsPart(Guid userId, string date, int numberToTake, string keyword);

        [Post("/post")]
        Task<Post> CreatePost([Body] Post post);

        [Put("/post")]
        Task<Post> UpdatePost([Body] Post post);

        [Post("/post/like/{postId}")]
        Task<Post> LikeOrDislike(Guid postId);

        [Delete("/post/{id}")]
        Task DeletePost(Guid id);

        [Get("/post-picture")]
        Task<PostPicture> GetPostPicture(Guid id);

        [Delete("/post-picture/{id}")]
        Task DeletePostPicture(Guid id);

        [Get("/comment-profile/{id}")]
        Task<ProfileComment> GetProfileComment(Guid id);

        [Get("/comment-profile/user/{userId}")]
        Task<List<ProfileComment>> GetAllProfileCommentsByUser(Guid userId);

        [Get("/comment-profile/user/{userId}/page")]
        Task<List<ProfileComment>> GetAllProfileCommentsByUserPart(Guid userId, string date, int numberToTake);

        [Get("/comment-profile/for-user/{userId}")]
        Task<List<ProfileComment>> GetAllCommentsForUser(Guid userId);

        [Get("/comment-profile/for-user/{userId}/page")]
        Task<List<ProfileComment>> GetAllCommentsForUserPart(Guid userId, string date, int numberToTake);

        [Put("/comment-profile")]
        Task<ProfileComment> CreateProfileComment([Body] ProfileComment comment);

        [Delete("/comment-profile/{id}")]
        Task DeleteProfileComment(Guid id);

        [Put("/user")]
        Task<User> CreateOrUpdateUser([Body] User model);

        [Put("/user/send-request/{userId}")]
        Task SendRoommateRequest(Guid userId);

        [Put("/user/accept-request/{userId}")]
        Task AcceptRoommateRequest(Guid userId);

        [Put("/user/decline-request/{userId}")]
        Task DeclineRoommateRequest(Guid userId);

        [Get("/user/status/{userId}")]
        Task<RoommateStatus> CheckRoommateStatus(Guid userId);

        [Get("/user/current")]
        Task<User> GetCurrentUser();

        [Get("/user/{id}")]
        Task<User> GetUser(Guid id);

        [Get("/user/search/{name}")]
        Task<List<User>> SearchUserByName(string name);

        [Get("/user/search/{name}/page")]
        Task<List<User>> SearchUserByNamePart(string name, string date, int numberToTake);

        [Get("/user")]
        Task<List<User>> GetAllUsers();

        [Get("/user/page")]
        Task<List<User>> GetAllUsersPart(string date, int numberToTake);

        [Get("/user-report/all")]
        Task<List<UserReport>> GetAllUserReports();

        [Get("/user-report/all-part")]
        Task<List<UserReport>> GetAllUserReportsPart(int numberPerPage, DateTimeOffset oldestReport);

        [Get("/user-report/processed")]
        Task<List<UserReport>> GetAllProcessedUserReports();

        [Get("/user-report/processed-part")]
        Task<List<UserReport>> GetAllProcessedUserReportsPart(int numberPerPage, DateTimeOffset oldestReport);

        [Post("/user-report")]
        Task ReportUser([Body] UserReport model);

        [Put("/user-report/process")]
        Task<UserReport> ProcessReport([Body] UserReport model);

        [Post("/connect/token")]
        Task<TokenApiModel> GetTokenByPassword([Body(BodySerializationMethod.UrlEncoded)] OfflineFlowRequest request);

        [Post("/connect/token")]
        Task<TokenApiModel> GetTokenByRefresh([Body(BodySerializationMethod.UrlEncoded)] RefreshFlowRequest request);

        [Multipart]
        [Post("/image")]
        Task<string> UploadPicture(StreamPart image);

        [Delete("/image/{filename}")]
        Task DeletePicture(string filename);
    }
}