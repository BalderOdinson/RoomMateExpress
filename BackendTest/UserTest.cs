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
using RoomMateExpressWebApi.EF.ViewModels;

namespace BackendTest
{
    /// <summary>
    /// Summary description for UserTest
    /// </summary>
    [TestClass]
    public class UserTest
    {
        private const string ConnectionString = "Server=(localdb)\\mssqllocaldb;Database=RMEDatabase;Trusted_Connection=True;MultipleActiveResultSets=true";

        public UserTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get => testContextInstance;
            set => testContextInstance = value;
        }

        public RoomMateExpressDbContext DbContext { get; set; }

        public IUserService UserService { get; set; }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize]
        public static void UserClassInitialize(TestContext testContext)
        {
            UserMockings.Initalize();
        }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        [TestInitialize]
        public void UserTestInitialize()
        {
            DbContext = new RoomMateExpressDbContext(ConnectionString);
            UserService = new UserService(DbContext);
        }
        //
        // Use TestCleanup to run code after each test has run
        [TestCleanup]
        public void UserTestCleanup()
        {
            DbContext.Dispose();
            DbContext = null;
            UserService = null;
        }
        #endregion

        [TestMethod]
        public void TestUserAddOrUpdate()
        {
            foreach (var user in UserMockings.Users)
            {
                var result = UserService.CreateOrUpdateUser(user).GetAwaiter().GetResult();
                //Thread.Sleep(1000);
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public void TestGetAllUsers()
        {
            var users = UserService.GetAllUsers().GetAwaiter().GetResult();
            foreach (var user in users)
            {
                var userRes = UserMockings.Users.FirstOrDefault(u => u.Id == user.Id);
                Assert.IsNotNull(userRes);
                userRes.CreationDate = user.CreationDate;
            }
            Assert.AreEqual(30, users.Count);
        }

        [TestMethod]
        public void TestGetAllUsersPart()
        {
            var users = new List<UserViewModel>();
            for (int i = 1; i < 6; i++)
            {
                users.AddRange(UserService
                    .GetAllUsers(users.Count == 0 ? DateTimeOffset.Now : users.Last().CreationDate, 5)
                    .GetAwaiter().GetResult());
                Assert.AreEqual(users.Min(user => user.CreationDate), users.Last().CreationDate);
                Assert.AreEqual(i * 5, users.Count);
            }
        }

        [TestMethod]
        public void TestGetUser()
        {
            var user = UserService.GetUser(UserMockings.GetGuid(0)).GetAwaiter().GetResult();
            Assert.AreEqual(UserMockings.Users[0].Id, user.Id);
        }

        [TestMethod]
        public void TestRoommateRequest()
        {
            Assert.AreEqual(RoommateStatus.None, UserService.CheckRoommateStatus(UserMockings.GetGuid(0), UserMockings.GetGuid(1)).GetAwaiter().GetResult());
            Assert.AreEqual(RoommateStatus.None, UserService.CheckRoommateStatus(UserMockings.GetGuid(1), UserMockings.GetGuid(0)).GetAwaiter().GetResult());
            DetachAllEntities();
            UserService.SendRoommateRequest(UserMockings.GetGuid(0), UserMockings.GetGuid(1)).GetAwaiter().GetResult();
            Assert.AreEqual(RoommateStatus.RequestSent, UserService.CheckRoommateStatus(UserMockings.GetGuid(0), UserMockings.GetGuid(1)).GetAwaiter().GetResult());            
            Assert.AreEqual(RoommateStatus.RequestRecieved, UserService.CheckRoommateStatus(UserMockings.GetGuid(1), UserMockings.GetGuid(0)).GetAwaiter().GetResult());
            DetachAllEntities();
            Assert.ThrowsExceptionAsync<UserNotFoundException>(() =>
                    UserService.AcceptRoommateRequest(UserMockings.GetGuid(0), UserMockings.GetGuid(1))).GetAwaiter()
                .GetResult();
            DetachAllEntities();
            Assert.ThrowsExceptionAsync<DuplicateRequestException>(() =>
                    UserService.SendRoommateRequest(UserMockings.GetGuid(1), UserMockings.GetGuid(0))).GetAwaiter()
                .GetResult();
            DetachAllEntities();
            UserService.AcceptRoommateRequest(UserMockings.GetGuid(1), UserMockings.GetGuid(0)).GetAwaiter().GetResult();
            Assert.AreEqual(RoommateStatus.Roommates, UserService.CheckRoommateStatus(UserMockings.GetGuid(0), UserMockings.GetGuid(1)).GetAwaiter().GetResult());
            Assert.AreEqual(RoommateStatus.Roommates, UserService.CheckRoommateStatus(UserMockings.GetGuid(1), UserMockings.GetGuid(0)).GetAwaiter().GetResult());
            DetachAllEntities();
            Assert.ThrowsExceptionAsync<DuplicateRequestException>(() =>
                    UserService.SendRoommateRequest(UserMockings.GetGuid(1), UserMockings.GetGuid(0))).GetAwaiter()
                .GetResult();
            DetachAllEntities();
            UserService.SendRoommateRequest(UserMockings.GetGuid(2), UserMockings.GetGuid(1)).GetAwaiter().GetResult();
            Assert.AreEqual(RoommateStatus.RequestSent, UserService.CheckRoommateStatus(UserMockings.GetGuid(2), UserMockings.GetGuid(1)).GetAwaiter().GetResult());
            Assert.AreEqual(RoommateStatus.RequestRecieved, UserService.CheckRoommateStatus(UserMockings.GetGuid(1), UserMockings.GetGuid(2)).GetAwaiter().GetResult());
            DetachAllEntities();
            Assert.ThrowsExceptionAsync<UserNotFoundException>(() =>
                    UserService.DeclineRoommateRequest(UserMockings.GetGuid(2), UserMockings.GetGuid(1))).GetAwaiter()
                .GetResult();
            DetachAllEntities();
            UserService.DeclineRoommateRequest(UserMockings.GetGuid(1), UserMockings.GetGuid(2)).GetAwaiter().GetResult();
            Assert.AreEqual(RoommateStatus.None, UserService.CheckRoommateStatus(UserMockings.GetGuid(2), UserMockings.GetGuid(1)).GetAwaiter().GetResult());
            Assert.AreEqual(RoommateStatus.None, UserService.CheckRoommateStatus(UserMockings.GetGuid(1), UserMockings.GetGuid(2)).GetAwaiter().GetResult());
        }

        [TestMethod]
        public void TestGetUserRoomates()
        {
            var userRoommates = UserService.GetUserRoomates(UserMockings.GetGuid(0)).GetAwaiter().GetResult();
            Assert.AreEqual(1, userRoommates.Count);
            DetachAllEntities();
            var userRoommates1 = UserService.GetUserRoomates(UserMockings.GetGuid(1)).GetAwaiter().GetResult();
            Assert.AreEqual(1, userRoommates1.Count);
        }

        [TestMethod]
        public void TestSearchUser()
        {
            var users = UserService.SearchUserByName(UserMockings.GetGuid(0), "User").GetAwaiter().GetResult();
            Assert.AreEqual(29, users.Count);
        }

        [TestMethod]
        public void TestSearchUserPart()
        {
            var users = new List<UserViewModel>();
            var i = 0;
            do
            {
                Assert.AreEqual(i * 5, users.Count);
                users.AddRange(UserService
                    .SearchUserByName(UserMockings.GetGuid(0),users.Count == 0 ? DateTimeOffset.Now : users.Last().CreationDate, 5, "User")
                    .GetAwaiter().GetResult());
                Assert.AreEqual(users.Min(user => user.CreationDate), users.Last().CreationDate);
            } while (++i < 6);
            Assert.AreEqual(29, users.Count);
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
