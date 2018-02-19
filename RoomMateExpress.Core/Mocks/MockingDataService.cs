using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvvmCross.Core.ViewModels;
using RoomMateExpress.Core.Helpers;
using RoomMateExpress.Core.Helpers.Enums;
using RoomMateExpress.Core.Models;
using RoomMateExpress.Core.ViewModels.BaseViewModels;

namespace RoomMateExpress.Core.Mocks
{
    public static class MockingDataService
    {
        public static List<User> Users { get; set; } = new List<User>(GetUsers());

        public static List<Admin> Admins { get; set; } = new List<Admin>(GetAdmins());

        public static List<Chat> Chats { get; set; } = new List<Chat>(GetChats());

        public static List<City> Cities { get; set; } = new List<City>(GetCities());

        public static List<Neighborhood> Neighborhoods { get; set; } = new List<Neighborhood>(Cities.SelectMany(c => c.Neighborhoods, (city, neighborhood) => neighborhood));

        public static List<Post> Posts { get; set; } = new List<Post>(GetPost());

        public static List<PostComment> CommentForPosts { get; set; } = new List<PostComment>(GetCommentForPosts());

        public static List<ProfileComment> CommentForProfiles { get; set; } = new List<ProfileComment>(GetCommentForProfiles());

        public static List<Message> Messages { get; set; } = new List<Message>(GetMessages());

        public static List<PostPicture> PostPictures { get; set; } = new List<PostPicture>(GetPostPictures());

        public static List<UserReport> UserReports { get; set; } = new List<UserReport>(GetUserReports());

        public static IEnumerable<Admin> GetAdmins()
        {
            return new[]
            {
                new Admin
                {
                    FirstName = "Admin",
                    LastName = "Admenko",
                    UserReports = GetUserReports()?.ToList()
                }
            };
        }

        public static IEnumerable<Chat> GetChats()
        {
            return new[]
            {
                new Chat
                {
                    PictureUrl = Users.Skip(1).FirstOrDefault().ProfilePictureUrl,
                    Users = new List<User>
                    {
                        Users.FirstOrDefault(),
                        Users.Skip(1).FirstOrDefault()
                    },
                    Name = $"{Users.Skip(1).FirstOrDefault().FirstName} {Users.Skip(1).FirstOrDefault().LastName}"
                },
                new Chat
                {
                    PictureUrl = Users.Skip(2).FirstOrDefault().ProfilePictureUrl,
                    Users = new List<User>
                    {
                        Users.FirstOrDefault(),
                        Users.Skip(2).FirstOrDefault()
                    },
                    Name = $"{Users.Skip(2).FirstOrDefault().FirstName} {Users.Skip(2).FirstOrDefault().LastName}"
                }
            };
        }

        public static IEnumerable<City> GetCities()
        {
            return new[]
            {
                new City
                {
                    Name = "Zagreb",
                    Neighborhoods = GetNeighborhoods().Where(n => n.City.Name == "Zagreb").ToList()
                },
                new City
                {
                    Name = "Split",
                    Neighborhoods = GetNeighborhoods().Where(n => n.City.Name == "Split").ToList()
                },
                new City
                {
                    Name = "Osijek",
                    Neighborhoods = GetNeighborhoods().Where(n => n.City.Name == "Osijek").ToList()
                },
            };
        }

        public static IEnumerable<PostComment> GetCommentForPosts()
        {
            return new[]
            {
                new PostComment
                {
                    User = Users.Skip(0).FirstOrDefault(),
                    CommentedAt = DateTimeOffset.Now.Subtract(new TimeSpan(0, 1, 1, 1)),
                    Text = "Komentar1",
                    Post = Posts.Skip(0).FirstOrDefault()
                },
                new PostComment
                {
                    User = Users.Skip(1).FirstOrDefault(),
                    CommentedAt = DateTimeOffset.Now.Subtract(new TimeSpan(0, 2, 1, 1)),
                    Text = "Komentar2",
                    Post = Posts.Skip(0).FirstOrDefault()
                },
                new PostComment
                {
                    User = Users.Skip(2).FirstOrDefault(),
                    CommentedAt = DateTimeOffset.Now.Subtract(new TimeSpan(0, 3, 1, 1)),
                    Text = "Komentar3",
                    Post = Posts.Skip(0).FirstOrDefault()
                },
                new PostComment
                {
                    User = Users.Skip(0).FirstOrDefault(),
                    CommentedAt = DateTimeOffset.Now.Subtract(new TimeSpan(0, 1, 1, 1)),
                    Text = "Komentar1",
                    Post = Posts.Skip(1).FirstOrDefault()
                },
                new PostComment
                {
                    User = Users.Skip(1).FirstOrDefault(),
                    CommentedAt = DateTimeOffset.Now.Subtract(new TimeSpan(0, 2, 1, 1)),
                    Text = "Komentar2",
                    Post = Posts.Skip(1).FirstOrDefault()
                },
                new PostComment
                {
                    User = Users.Skip(2).FirstOrDefault(),
                    CommentedAt = DateTimeOffset.Now.Subtract(new TimeSpan(0, 3, 1, 1)),
                    Text = "Komentar3",
                    Post = Posts.Skip(1).FirstOrDefault()
                },
                new PostComment
                {
                    User = Users.Skip(0).FirstOrDefault(),
                    CommentedAt = DateTimeOffset.Now.Subtract(new TimeSpan(0, 1, 1, 1)),
                    Text = "Komentar1",
                    Post = Posts.Skip(2).FirstOrDefault()
                },
                new PostComment
                {
                    User = Users.Skip(1).FirstOrDefault(),
                    CommentedAt = DateTimeOffset.Now.Subtract(new TimeSpan(0, 2, 1, 1)),
                    Text = "Komentar2",
                    Post = Posts.Skip(2).FirstOrDefault()
                },
                new PostComment
                {
                    User = Users.Skip(2).FirstOrDefault(),
                    CommentedAt = DateTimeOffset.Now.Subtract(new TimeSpan(0, 3, 1, 1)),
                    Text = "Komentar3",
                    Post = Posts.Skip(2).FirstOrDefault()
                },
            }.OrderBy(p => p.CommentedAt);
        }

        public static IEnumerable<ProfileComment> GetCommentForProfiles()
        {
            return new[]
            {
                new ProfileComment
                {
                    Text = "Komentar",
                    CommentedAt = DateTimeOffset.Now.Subtract(new TimeSpan(0,0,10,0)),
                    Grade = 4,
                    UserCommentator = Users.Skip(2).FirstOrDefault(),
                    UserProfile = Users.Skip(1).FirstOrDefault()
                },
                new ProfileComment
                {
                    Text = "Komentar",
                    CommentedAt = DateTimeOffset.Now.Subtract(new TimeSpan(0,0,10,0)),
                    Grade = 4,
                    UserCommentator = Users.Skip(1).FirstOrDefault(),
                    UserProfile = Users.Skip(2).FirstOrDefault()
                },
                new ProfileComment
                {
                    Text = "Komentar",
                    CommentedAt = DateTimeOffset.Now.Subtract(new TimeSpan(0,0,10,0)),
                    Grade = 4,
                    UserCommentator = Users.Skip(2).FirstOrDefault(),
                    UserProfile = Users.Skip(0).FirstOrDefault()
                },
            }.OrderBy(p => p.CommentedAt);
        }

        public static IEnumerable<Message> GetMessages()
        {
            return new[]
            {
                new Message
                {
                    Chat = Chats.Skip(1).FirstOrDefault(),
                    UserSender = Users.Skip(0).FirstOrDefault(),
                    Text = "Bok",
                    SentAt = DateTimeOffset.Now.Subtract(new TimeSpan(0, 0, 10))
                },
                new Message
                {
                    Chat = Chats.Skip(0).FirstOrDefault(),
                    UserSender = Users.Skip(1).FirstOrDefault(),
                    Text = "Bok1",
                    SentAt = DateTimeOffset.Now.Subtract(new TimeSpan(0, 0, 13))
                },
                new Message
                {
                    Chat = Chats.Skip(1).FirstOrDefault(),
                    UserSender = Users.Skip(2).FirstOrDefault(),
                    Text = "Bok2",
                    SentAt = DateTimeOffset.Now.Subtract(new TimeSpan(0, 0, 15))
                },
                new Message
                {
                    Chat = Chats.Skip(0).FirstOrDefault(),
                    UserSender = Users.Skip(0).FirstOrDefault(),
                    Text = "Bok3",
                    SentAt = DateTimeOffset.Now.Subtract(new TimeSpan(0, 0, 16))
                },
                new Message
                {
                    Chat = Chats.Skip(0).FirstOrDefault(),
                    UserSender = Users.Skip(1).FirstOrDefault(),
                    Text = "Bok4",
                    SentAt = DateTimeOffset.Now.Subtract(new TimeSpan(0, 0, 17))
                },
                new Message
                {
                    Chat = Chats.Skip(1).FirstOrDefault(),
                    UserSender = Users.Skip(2).FirstOrDefault(),
                    Text = "Bok5",
                    SentAt = DateTimeOffset.Now.Subtract(new TimeSpan(0, 0, 18))
                },
                new Message
                {
                    Chat = Chats.Skip(0).FirstOrDefault(),
                    UserSender = Users.Skip(0).FirstOrDefault(),
                    Text = "Bok6",
                    SentAt = DateTimeOffset.Now.Subtract(new TimeSpan(0, 0, 19))
                },
            }.OrderBy(p => p.SentAt);
        }

        public static IEnumerable<Neighborhood> GetNeighborhoods()
        {
            return new[]
            {
                new Neighborhood
                {
                    City = new City{Name = "Zagreb"},
                    Name = "KvartZagreb1"
                },
                new Neighborhood
                {
                    City = new City{Name = "Zagreb"},
                    Name = "KvartZagreb2"
                },
                new Neighborhood
                {
                    City = new City{Name = "Split"},
                    Name = "KvartSplit1"
                },
                new Neighborhood
                {
                    City = new City{Name = "Split"},
                    Name = "KvartSplit1"
                },
                new Neighborhood
                {
                    City = new City{Name = "Osijek"},
                    Name = "KvartOsijek1"
                },
                new Neighborhood
                {
                    City = new City{Name = "Osijek"},
                    Name = "KvartOsijek1"
                },
            };
        }

        public static IEnumerable<Post> GetPost()
        {
            return new[]
            {
                new Post
                {
                    User = Users.Skip(0).FirstOrDefault(),
                    AccomodationType = AccomodationType.None,
                    AccomodationOption = AccomodationOptions.Without,
                    ArePetsAllowed = true,
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer erat nisi, egestas sed consequat pulvinar, suscipit at ipsum. Praesent volutpat enim sit amet ex mattis semper. Curabitur blandit non sapien eu vestibulum. In tincidunt nunc dapibus ultricies sollicitudin. Morbi sit amet velit ex. Phasellus egestas eu purus a semper. Vivamus interdum ex vitae pharetra malesuada. Praesent ullamcorper porttitor libero id blandit. Donec neque nisl, dignissim in tellus ut, interdum vestibulum ex. Nulla vel tincidunt erat, ac scelerisque ex. Nam sit amet tristique velit, sit amet congue nulla. Mauris hendrerit ut orci nec viverra. Praesent dictum viverra est ut viverra. Vivamus finibus fermentum nisi, non aliquam neque mattis at.",
                    IsSmokerAllowed = true,
                    Neighborhoods = Neighborhoods.Where(n => n.City.Name == "Zagreb").ToList(),
                    Likes = Users.ToList(),
                    NumberOfRoommates = 2,
                    PetOptions = PetOptions.Bird | PetOptions.SmallAnimal,
                    PostDate = DateTimeOffset.Now.Subtract(new TimeSpan(1,1,1,1)),
                    Title = "Tražim cimera u Zagrebu",
                    WantedGender = Gender.Any,
                    CommentsCount = 3,
                    LikesCount = 3,
                    IsLiked = true
                },
                new Post
                {
                    User = Users.Skip(1).FirstOrDefault(),
                    AccomodationType = AccomodationType.House,
                    AccomodationOption = AccomodationOptions.With,
                    ArePetsAllowed = false,
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer erat nisi, egestas sed consequat pulvinar, suscipit at ipsum. Praesent volutpat enim sit amet ex mattis semper. Curabitur blandit non sapien eu vestibulum. In tincidunt nunc dapibus ultricies sollicitudin. Morbi sit amet velit ex. Phasellus egestas eu purus a semper. Vivamus interdum ex vitae pharetra malesuada. Praesent ullamcorper porttitor libero id blandit. Donec neque nisl, dignissim in tellus ut, interdum vestibulum ex. Nulla vel tincidunt erat, ac scelerisque ex. Nam sit amet tristique velit, sit amet congue nulla. Mauris hendrerit ut orci nec viverra. Praesent dictum viverra est ut viverra. Vivamus finibus fermentum nisi, non aliquam neque mattis at.",
                    IsSmokerAllowed = false,
                    Neighborhoods = Neighborhoods.Where(n => n.City.Name == "Split").ToList(),
                    Likes = Users.ToList(),
                    NumberOfRoommates = 3,
                    PostDate = DateTimeOffset.Now.Subtract(new TimeSpan(2,1,1,1)),
                    Title = "Tražim cimera u Splitu",
                    WantedGender = Gender.Female,
                    PostPictures = GetPostPictures().ToList(),
                    Price = 1500.00m,
                    CommentsCount = 3,
                    LikesCount = 3,
                    IsLiked = true
                },
                new Post
                {
                    User = Users.Skip(2).FirstOrDefault(),
                    AccomodationType = AccomodationType.Apartment,
                    AccomodationOption = AccomodationOptions.With,
                    ArePetsAllowed = true,
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer erat nisi, egestas sed consequat pulvinar, suscipit at ipsum. Praesent volutpat enim sit amet ex mattis semper. Curabitur blandit non sapien eu vestibulum. In tincidunt nunc dapibus ultricies sollicitudin. Morbi sit amet velit ex. Phasellus egestas eu purus a semper. Vivamus interdum ex vitae pharetra malesuada. Praesent ullamcorper porttitor libero id blandit. Donec neque nisl, dignissim in tellus ut, interdum vestibulum ex. Nulla vel tincidunt erat, ac scelerisque ex. Nam sit amet tristique velit, sit amet congue nulla. Mauris hendrerit ut orci nec viverra. Praesent dictum viverra est ut viverra. Vivamus finibus fermentum nisi, non aliquam neque mattis at.",
                    IsSmokerAllowed = true,
                    Neighborhoods = Neighborhoods.Where(n => n.City.Name == "Osijek").ToList(),
                    Likes = Users.ToList(),
                    NumberOfRoommates = 2,
                    PetOptions = PetOptions.Dog | PetOptions.Other,
                    PostDate = DateTimeOffset.Now.Subtract(new TimeSpan(3,1,1,1)),
                    Title = "Tražim cimera u Osijeku",
                    WantedGender = Gender.Any,
                    PostPictures = GetPostPictures().ToList(),
                    Price = 2000.00m,
                    CommentsCount = 3,
                    LikesCount = 3,
                    IsLiked = true
                },
            }.OrderBy(p => p.PostDate);
        }

        public static IEnumerable<PostPicture> GetPostPictures()
        {
            return new[]
            {
                new PostPicture
                {
                    PictureUrl = "http://images.oakwood.com/photogallery/Falls-Church-Living-Room-Low-Res.jpg"
                },
                new PostPicture
                {
                    PictureUrl =
                        "http://www.avaloncommunities.com/~/media/Images/Regional%20Landing%20Pages/VA%20Avalon%20Park%20Crest.jpg?h=350&la=en&w=600"
                },
                new PostPicture
                {
                    PictureUrl =
                        "http://portal.oakwood.com/profiles/images/0810/7756/Photos/ORNR_living_low_res.jpg?width=600&height=600"
                },
                new PostPicture
                {
                    PictureUrl =
                        "http://cdn3.sacostatic.com/media/26420/bedroom.jpg?anchor=center&mode=crop&quality=80&width=800&heightratio=0.6666666666666666666666666667&slimmage=true&rnd=131443409490000000"
                },
            };
        }

        public static IEnumerable<User> GetUsers()
        {
            return new[]
            {
                new User
                {
                    Age = 20,
                    BirthDate = DateTimeOffset.Now.Subtract(new TimeSpan(365 * 21, 0,0,0)),
                    FirstName = "Pero",
                    LastName = "Perić",
                    ProfilePictureUrl = "http://photodoto.com/wp-content/uploads/2013/01/54-self-portrait.jpg",
                    Gender = Gender.Male,
                    AverageGrade = 4.0m,
                    DescriptionOfStudyOrWork = "Student",
                    HasFaculty = true,
                    IsSmoker = true,
                },
                new User
                {
                    Age = 20,
                    BirthDate = DateTimeOffset.Now.Subtract(new TimeSpan(365 * 21, 0,0,0)),
                    FirstName = "Jadranka",
                    LastName = "Kovačević",
                    ProfilePictureUrl = "http://i.pinimg.com/736x/d0/b1/c1/d0b1c100c871ee188bfb7e6357c61a38--profile-photography-white-photography.jpg",
                    Gender = Gender.Female,
                    AverageGrade = 4.0m,
                    DescriptionOfStudyOrWork = "Student",
                    HasFaculty = true,
                    IsSmoker = true,
                },
                new User
                {
                    Age = 20,
                    BirthDate = DateTimeOffset.Now.Subtract(new TimeSpan(365 * 21, 0,0,0)),
                    FirstName = "Ivo",
                    LastName = "Ivić",
                    ProfilePictureUrl = "http://i2.cdn.turner.com/cnnnext/dam/assets/140926165711-john-sutter-profile-image-large-169.jpg",
                    Gender = Gender.Male,
                    AverageGrade = 4.0m,
                    DescriptionOfStudyOrWork = "Bartender",
                    HasFaculty = false,
                    IsSmoker = false,
                },
            };
        }

        public static IEnumerable<UserReport> GetUserReports()
        {
            return new[]
            {
                new UserReport
                {
                    //Text = "isus krist je ovo",
                    //UserReported = Users.Skip(2).FirstOrDefault(),
                    //UserReporting = Users.Skip(3).FirstOrDefault(),
                    //Admin = Admins.FirstOrDefault(),
                    //DateReporting = System.DateTime.Now,
                }
            };
        }
    }
}
