using System;
using System.Text;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BackendTest.Mockings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoomMateExpressWebApi.EF.Exceptions;
using RoomMateExpressWebApi.EF.Extensions;
using RoomMateExpressWebApi.EF.Models;
using RoomMateExpressWebApi.EF.Services;
using RoomMateExpressWebApi.EF.ViewModels;

namespace BackendTest
{
    /// <summary>
    /// Summary description for PostTest
    /// </summary>
    [TestClass]
    public class PostTest
    {
        private const string ConnectionString = "Server=(localdb)\\mssqllocaldb;Database=RMEDatabase;Trusted_Connection=True;MultipleActiveResultSets=true";

        public PostTest()
        {

        }

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        public RoomMateExpressDbContext DbContext { get; set; }

        public IPostService PostService { get; set; }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize]
        public static void PostClassInitialize(TestContext testContext)
        {
            PostMockings.Initalize();
        }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        [TestInitialize]
        public void PostTestInitialize()
        {
            DbContext = new RoomMateExpressDbContext(ConnectionString);
            PostService = new PostService(DbContext);
        }
        //
        // Use TestCleanup to run code after each test has run
        [TestCleanup]
        public void PostTestCleanup()
        {
            DbContext.Dispose();
            DbContext = null;
            PostService = null;
        }
        #endregion

        [TestMethod]
        public void TestCreatePost()
        {
            foreach (var post in PostMockings.Posts)
            {
                var postResult = PostService.CreatePost(post).GetAwaiter().GetResult();
                Assert.IsNotNull(postResult);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(UserNotFoundException))]
        public void TestCreatePostUserProfileNotFound()
        {
            var post = PostMockings.Posts.FirstOrDefault();
            post.User.Id = Guid.Empty;
            var result = PostService.CreatePost(post).GetAwaiter().GetResult();
        }

        [TestMethod]
        [ExpectedException(typeof(NeighborhoodNotFoundException))]
        public void TestCreatePostNeighborhoodNotFound()
        {
            var post = PostMockings.Posts.FirstOrDefault();
            post.Neighborhoods.FirstOrDefault().Id = Guid.Empty;
            var result = PostService.CreatePost(post).GetAwaiter().GetResult();
        }

        [TestMethod]
        public void TestLikeDislike()
        {
            var post = PostService.LikeDislikePost(PostMockings.GetGuid(0), PostMockings.GetGuid(1)).GetAwaiter()
                .GetResult();
            Assert.IsTrue(post.IsLiked);
            post = PostService.LikeDislikePost(PostMockings.GetGuid(0), PostMockings.GetGuid(1)).GetAwaiter()
                .GetResult();
            Assert.IsFalse(post.IsLiked);
        }

        [TestMethod]
        [ExpectedException(typeof(UserNotFoundException))]
        public void TestLikeDislikeUserNotFound()
        {
            var post = PostService.LikeDislikePost(PostMockings.GetGuid(0), PostMockings.GetGuid(31)).GetAwaiter()
                .GetResult();
        }

        [TestMethod]
        [ExpectedException(typeof(PostNotFoundException))]
        public void TestLikeDislikePostNotFound()
        {
            var post = PostService.LikeDislikePost(PostMockings.GetGuid(31), PostMockings.GetGuid(1)).GetAwaiter()
                .GetResult();
        }

        [TestMethod]
        public void TestUpdatePost()
        {
            var mock = PostMockings.Posts.FirstOrDefault();
            mock.Title = "Changed title";
            mock.Neighborhoods.RemoveAt(0);
            mock.PostPictures.Add(new PostPicture { PictureUrl = "NewPicture" });
            var post = PostService.UpdatePost(mock, PostMockings.GetGuid(0)).GetAwaiter().GetResult();
            Assert.AreEqual(mock.Title, post.Title);
        }

        [TestMethod]
        public void TestGetPost()
        {
            var post = PostService.GetPost(PostMockings.GetGuid(1), PostMockings.GetGuid(2)).GetAwaiter().GetResult();
            Assert.AreEqual(PostMockings.GetGuid(0), post.Id);
        }

        [TestMethod]
        public void TestGetAllPosts()
        {
            var posts = PostService.GetAllPosts().GetAwaiter().GetResult();
            Assert.AreEqual(30, posts.Count);
        }

        [TestMethod]
        public void TestGetAllPostsSorted()
        {
            var posts = PostService.GetAllPosts(PostSortOptions.Popularity, SortOrderOption.Descending).GetAwaiter().GetResult();
            Assert.IsTrue(posts.Last().Price == posts.Max(p => p.Price));
            Assert.AreEqual(30, posts.Count);
        }

        [TestMethod]
        public void TestGetAllPostsFilter()
        {
            var posts = PostService.GetAllPosts(PostSortOptions.Date, SortOrderOption.Descending,
                AccomodationOptions.With, 0.00m, 600.00m, "Zagreb", "Changed title").GetAwaiter().GetResult();
            Assert.AreEqual(1, posts.Count);
        }

        [TestMethod]
        public void TestGetAllPostsPart()
        {
            var posts = new List<PostViewModel>();
            for (int i = 1; i < 6; i++)
            {
                posts.AddRange(PostService
                    .GetAllPosts(posts.Count == 0 ? DateTimeOffset.Now : posts.Last().PostDate, null, 5)
                    .GetAwaiter().GetResult());
                Assert.AreEqual(posts.Min(post => post.PostDate), posts.Last().PostDate);
                Assert.AreEqual(i * 5, posts.Count);
            }
        }

        [TestMethod]
        public void TestGetAllPostsSortPart()
        {
            var posts = new List<PostViewModel>();
            for (int i = 1; i < 3; i++)
            {
                posts.AddRange(PostService
                    .GetAllPosts(posts.Count == 0 ? DateTimeOffset.Now : posts.Last().PostDate,
                        posts.Count == 0 ? 6 : posts.Last().User.AverageGrade, 5, PostSortOptions.UserRating,
                        SortOrderOption.Descending, AccomodationOptions.With, city: "Zagreb")
                    .GetAwaiter().GetResult());
                Assert.AreEqual(posts.Min(post => post.User.AverageGrade), posts.Last().User.AverageGrade);
                Assert.IsTrue(posts.Last().Neighborhoods.Any(n => n.Name.Equals("Središće")));
                Assert.AreEqual(i * 5, posts.Count);
            }
        }

        [TestMethod]
        public void TestGetAllPostsWithCurrentUser()
        {
            var posts = PostService.GetAllPosts(PostMockings.GetGuid(0)).GetAwaiter().GetResult();
            Assert.AreEqual(27, posts.Count);
        }

        [TestMethod]
        public void TestGetAllPostsWithCurrentUserPart()
        {
            var posts = new List<PostViewModel>();
            for (int i = 1; i < 7; i++)
            {
                posts.AddRange(PostService
                    .GetAllPosts(PostMockings.GetGuid(0), posts.Count == 0 ? DateTimeOffset.Now : posts.Last().PostDate, null, 5)
                    .GetAwaiter().GetResult());
                Assert.AreEqual(posts.Min(post => post.PostDate), posts.Last().PostDate);
            }
            Assert.AreEqual(27, posts.Count);
        }

        [TestMethod]
        public void TestGetPostsByUser()
        {
            var posts = PostService.GetPostsByUser(PostMockings.GetGuid(0)).GetAwaiter().GetResult();
            Assert.AreEqual(3, posts.Count);
        }

        [TestMethod]
        public void TestGetPostsByUserPart()
        {
            var posts = new List<PostViewModel>();
            for (int i = 1; i < 3; i++)
            {
                posts.AddRange(PostService
                    .GetPostsByUser(PostMockings.GetGuid(0), posts.Count == 0 ? DateTimeOffset.Now : posts.Last().PostDate, 1)
                    .GetAwaiter().GetResult());
                Assert.AreEqual(posts.Min(post => post.PostDate), posts.Last().PostDate);
                Assert.AreEqual(i, posts.Count);
            }
        }

        [TestMethod]
        public void TestGetPostsByUserWithCurrentUser()
        {
            var posts = PostService.GetPostsByUser(PostMockings.GetGuid(0), PostMockings.GetGuid(1)).GetAwaiter().GetResult();
            Assert.AreEqual(3, posts.Count);
        }

        [TestMethod]
        public void TestGetPostsByUserWithCurrentUserPart()
        {
            var posts = new List<PostViewModel>();
            for (int i = 1; i < 3; i++)
            {
                posts.AddRange(PostService
                    .GetPostsByUser(PostMockings.GetGuid(0), PostMockings.GetGuid(1), posts.Count == 0 ? DateTimeOffset.Now : posts.Last().PostDate, 1)
                    .GetAwaiter().GetResult());
                Assert.AreEqual(posts.Min(post => post.PostDate), posts.Last().PostDate);
                Assert.AreEqual(i, posts.Count);
            }
        }

        [TestMethod]
        public void TestDeletePost()
        {
            foreach (var post in PostMockings.Posts)
            {
                Assert.IsTrue(PostService.DeletePost(post.Id).GetAwaiter().GetResult());
            }
        }

        private void LikePosts()
        {
            for (int i = 0; i < 10; i++)
            {
                PostService.LikeDislikePost(PostMockings.GetGuid(i % 2), PostMockings.GetGuid(i + 2)).GetAwaiter().GetResult();
            }
        }

        private void DetachAllEntities()
        {
            var changedEntriesCopy = DbContext.ChangeTracker.Entries()
                .Where(e => e.State != EntityState.Detached)
                .ToList();
            foreach (var entity in changedEntriesCopy)
            {
                DbContext.Entry(entity.Entity).State = EntityState.Detached;
            }
        }
    }
}
