using System;
using System.Text;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BackendTest.Mockings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoomMateExpressWebApi.EF.Exceptions;
using RoomMateExpressWebApi.EF.Models;
using RoomMateExpressWebApi.EF.Services;

namespace BackendTest
{
    /// <summary>
    /// Summary description for ProfileCommentTest
    /// </summary>
    [TestClass]
    public class ProfileCommentTest
    {
        private const string ConnectionString = "Server=(localdb)\\mssqllocaldb;Database=RMEDatabase;Trusted_Connection=True;MultipleActiveResultSets=true";

        public ProfileCommentTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public RoomMateExpressDbContext DbContext { get; set; }

        public IProfileCommentService ProfileCommentService { get; set; }

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
        public static void ProfileCommentClassInitialize(TestContext testContext)
        {
            ProfileCommentMockings.Initalize();
        }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        [TestInitialize]
        public void ProfileCommentTestInitialize()
        {
            DbContext = new RoomMateExpressDbContext(ConnectionString);
            ProfileCommentService = new ProfileCommentService(DbContext);
        }
        //
        // Use TestCleanup to run code after each test has run
        [TestCleanup]
        public void ProfileCommentTestCleanup()
        {
            DbContext.Dispose();
            DbContext = null;
            ProfileCommentService = null;
        }
        #endregion

        [TestMethod]
        public void TestCreateProfileComment()
        {
            foreach (var commentForProfile in ProfileCommentMockings.ProfileComments)
            {
                var comment = ProfileCommentService.CreateProfileComment(commentForProfile).GetAwaiter()
                    .GetResult();
                Assert.IsNotNull(comment);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(UserNotFoundException))]
        public void TestCreateProfileCommentUserProfileNotFound()
        {
            var comment = ProfileCommentMockings.ProfileComments.FirstOrDefault();
            comment.UserProfile.Id = Guid.Empty;
            var result = ProfileCommentService.CreateProfileComment(comment).GetAwaiter().GetResult();
        }

        [TestMethod]
        [ExpectedException(typeof(UserNotFoundException))]
        public void TestCreateProfileCommentUserCommentatorNotFound()
        {
            var comment = ProfileCommentMockings.ProfileComments.FirstOrDefault();
            comment.UserCommentator.Id = Guid.Empty;
            var result = ProfileCommentService.CreateProfileComment(comment).GetAwaiter().GetResult();
        }

        [TestMethod]
        public void TestGetCommentsForUser()
        {
            var comments = ProfileCommentService.GetCommentsForUser(ProfileCommentMockings.GetGuid(0))
                .GetAwaiter().GetResult();
            Assert.AreEqual(15, comments.Count);
        }

        [TestMethod]
        public void TestGetCommentsForUserPart()
        {
            var comments = new List<ProfileComment>();
            for (int i = 1; i < 4; i++)
            {
                comments.AddRange(ProfileCommentService
                    .GetCommentsForUser(ProfileCommentMockings.GetGuid(0),comments.Count == 0 ? DateTimeOffset.Now : comments.Last().CommentedAt, 5)
                    .GetAwaiter().GetResult());
                Assert.AreEqual(comments.Min(comment => comment.CommentedAt), comments.Last().CommentedAt);
                Assert.AreEqual(i * 5, comments.Count);
            }
        }

        [TestMethod]
        public void TestGetCommentsByUser()
        {
            var comments = ProfileCommentService.GetCommentsByUser(ProfileCommentMockings.GetGuid(0))
                .GetAwaiter().GetResult();
            Assert.AreEqual(15, comments.Count);
        }

        [TestMethod]
        public void TestGetProfileComment()
        {
            var comment = ProfileCommentService.GetProfileComment(ProfileCommentMockings.GetGuid(0)).GetAwaiter()
                .GetResult();
            Assert.AreEqual(ProfileCommentMockings.GetGuid(0), comment.Id);
        }

        [TestMethod]
        public void TestGetCommentsByUserPart()
        {
            var comments = new List<ProfileComment>();
            for (int i = 1; i < 4; i++)
            {
                comments.AddRange(ProfileCommentService
                    .GetCommentsByUser(ProfileCommentMockings.GetGuid(0), comments.Count == 0 ? DateTimeOffset.Now : comments.Last().CommentedAt, 5)
                    .GetAwaiter().GetResult());
                Assert.AreEqual(comments.Min(comment => comment.CommentedAt), comments.Last().CommentedAt);
                Assert.AreEqual(i * 5, comments.Count);
            }
        }

        [TestMethod]
        public void TestDeleteProfileComment()
        {
            foreach (var commentForProfile in ProfileCommentMockings.ProfileComments)
            {
                Assert.IsTrue(ProfileCommentService.DeleteProfileComment(commentForProfile.Id).GetAwaiter().GetResult());
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
