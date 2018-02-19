using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using BackendTest.Mockings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoomMateExpressWebApi.EF.Exceptions;
using RoomMateExpressWebApi.EF.Models;
using RoomMateExpressWebApi.EF.Services;

namespace BackendTest
{
    /// <summary>
    /// Summary description for PostCommentTest
    /// </summary>
    [TestClass]
    public class PostCommentTest
    {
        private const string ConnectionString = "Server=(localdb)\\mssqllocaldb;Database=RMEDatabase;Trusted_Connection=True;MultipleActiveResultSets=true";

        public PostCommentTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public RoomMateExpressDbContext DbContext { get; set; }

        public IPostCommentService PostCommentService { get; set; }

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize]
        public static void PostCommentClassInitialize(TestContext testContext)
        {
            PostCommentMockings.Initalize();
        }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        [TestInitialize]
        public void PostCommentTestInitialize()
        {
            DbContext = new RoomMateExpressDbContext(ConnectionString);
            PostCommentService = new PostCommentService(DbContext);
        }
        //
        // Use TestCleanup to run code after each test has run
        [TestCleanup]
        public void PostCommentTestCleanup()
        {
            DbContext.Dispose();
            DbContext = null;
            PostCommentService = null;
        }
        #endregion

        [TestMethod]
        public void TestCreatePostComment()
        {
            foreach (var commentForPost in PostCommentMockings.PostComments)
            {
                var comment = PostCommentService.CommentPost(commentForPost).GetAwaiter()
                    .GetResult();
                Assert.IsNotNull(comment);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(UserNotFoundException))]
        public void TestCreatePostCommentUserNotFound()
        {
            var comment = PostCommentMockings.PostComments.FirstOrDefault();
            comment.User.Id = Guid.Empty;
            var result = PostCommentService.CommentPost(comment).GetAwaiter().GetResult();
        }

        [TestMethod]
        [ExpectedException(typeof(PostNotFoundException))]
        public void TestCreatePostCommentPostNotFound()
        {
            var comment = PostCommentMockings.PostComments.FirstOrDefault();
            comment.Post.Id = Guid.Empty;
            var result = PostCommentService.CommentPost(comment).GetAwaiter().GetResult();
        }

        [TestMethod]
        public void TestGetPostComments()
        {
            var comments = PostCommentService.GetPostComments(PostCommentMockings.GetGuid(1))
                .GetAwaiter().GetResult();
            Assert.AreEqual(30, comments.Count);
        }

        [TestMethod]
        public void TestGetPostCommentsPart()
        {
            var comments = new List<PostComment>();
            for (int i = 1; i < 7; i++)
            {
                comments.AddRange(PostCommentService
                    .GetPostComments(PostCommentMockings.GetGuid(1), comments.Count == 0 ? DateTimeOffset.Now : comments.Last().CommentedAt, 5)
                    .GetAwaiter().GetResult());
                Assert.AreEqual(comments.Min(comment => comment.CommentedAt), comments.Last().CommentedAt);
                Assert.AreEqual(i * 5, comments.Count);
            }
        }

        [TestMethod]
        public void TestGetPostComment()
        {
            var comment = PostCommentService.GetPostComment(PostCommentMockings.GetGuid(0)).GetAwaiter()
                .GetResult();
            Assert.AreEqual(PostCommentMockings.GetGuid(0), comment.Id);
        }

        [TestMethod]
        public void TestDeletePostComment()
        {
            foreach (var commentForPost in PostCommentMockings.PostComments)
            {
                Assert.IsTrue(PostCommentService.DeletePostComment(commentForPost.Id).GetAwaiter().GetResult());
            }
        }
    }
}
