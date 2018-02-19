using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RoomMateExpressWebApi.EF.Models
{
    public class User : Entity
    {
        [Required]
        [StringLength(maximumLength: 35, MinimumLength = 3)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(maximumLength: 35, MinimumLength = 3)]
        public string LastName { get; set; }

        [Required]
        [StringLength(maximumLength: 255)]
        public string ProfilePictureUrl { get; set; }

        public DateTimeOffset BirthDate { get; set; }

        public DateTimeOffset CreationDate { get; set; }

        public bool HasFaculty { get; set; }

        [Required]
        public string DescriptionOfStudyOrWork { get; set; }

        public bool IsSmoker { get; set; }

        public Gender Gender { get; set; }

        public List<UserReport> UserReports { get; set; }

        public List<UserReport> UsersReported { get; set; }

        public List<User> RoommatesWithMe { get; set; }

        public List<User> MyRoommates { get; set; }

        public List<ProfileComment> ProfileComments { get; set; }

        public List<ProfileComment> CommentsOnProfile { get; set; }

        public List<Chat> Chats { get; set; }

        public List<Message> SentMessages { get; set; }

        public List<PostComment> PostComments { get; set; }

        public List<Post> Posts { get; set; }

        public List<Post> Likes { get; set; }

        public List<User> SentRoommateRequests { get; set; }

        public List<User> RecievedRoommateRequests { get; set; } 

        public User()
        {
            UserReports = new List<UserReport>();
            UsersReported = new List<UserReport>();
            MyRoommates = new List<User>();
            RoommatesWithMe = new List<User>();
            PostComments = new List<PostComment>();
            ProfileComments = new List<ProfileComment>();
            Chats = new List<Chat>();
            SentMessages = new List<Message>();
            CommentsOnProfile = new List<ProfileComment>();
            Posts = new List<Post>();
            Likes = new List<Post>();
            SentRoommateRequests = new List<User>();
            RecievedRoommateRequests = new List<User>();
        }

        public Guid AccountId { get; set; }
    }
}
