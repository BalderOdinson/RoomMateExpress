using System;
using System.Text;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using BackendTest.Mockings;
using Dapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoomMateExpressWebApi.EF.Models;
using RoomMateExpressWebApi.EF.Services;
using RoomMateExpressWebApi.EF.ViewModels;

namespace BackendTest
{
    /// <summary>
    /// Summary description for ChatTest
    /// </summary>
    [TestClass]
    public class ChatTest
    {
        private const string ConnectionString = "Server=(localdb)\\mssqllocaldb;Database=RMEDatabase;Trusted_Connection=True;MultipleActiveResultSets=true";

        public ChatTest()
        {

        }

        private TestContext _testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get => _testContextInstance;
            set => _testContextInstance = value;
        }

        public RoomMateExpressDbContext DbContext { get; set; }

        public IChatService ChatService { get; set; }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize]
        public static void ChatClassInitialize(TestContext testContext)
        {
            ChatMockings.Initalize();
        }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        [TestInitialize]
        public void ChatTestInitialize()
        {
            DbContext = new RoomMateExpressDbContext(ConnectionString);
            ChatService = new ChatService(DbContext);
        }
        //
        // Use TestCleanup to run code after each test has run
        [TestCleanup]
        public void ChatTestCleanup()
        {
            DbContext.Dispose();
            DbContext = null;
            ChatService = null;
        }
        #endregion

        [TestMethod]
        public void TestCreateChat()
        {
            foreach (var chat in ChatMockings.Chats)
            {
                var result = ChatService.CreateChat(chat.Users.First().Id, chat).GetAwaiter().GetResult();
                DetachAllEntities();
                //Thread.Sleep(1000);
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public void TestGetChats()
        {
            var chats = ChatService.GetChats(ChatMockings.GetGuid(0)).GetAwaiter().GetResult();
            foreach (var chat in chats)
            {
                var chatRes = ChatMockings.Chats.FirstOrDefault(ad => ad.Id == chat.Id);
                Assert.IsNotNull(chatRes);
                chatRes.LastModified = chat.LastModified;
            }
            Assert.AreEqual(2, chats.Count);
        }

        [TestMethod]
        public void TestGetChatByUsers()
        {
            var chat = ChatService.GetChatByAnotherUser(ChatMockings.GetGuid(1), ChatMockings.GetGuid(2)).GetAwaiter().GetResult();
            Assert.IsNotNull(chat);
            Assert.AreEqual(ChatMockings.GetGuid(1), chat.Id);
        }

        [TestMethod]
        public void TestGetChatsPart()
        {
            var chats = new List<ChatViewModel>();
            for (int i = 1; i < 3; i++)
            {
                chats.AddRange(ChatService
                    .GetChats(ChatMockings.GetGuid(0), chats.Count == 0 ? DateTimeOffset.Now : chats.Last().LastModified, 1)
                    .GetAwaiter().GetResult());
                Assert.AreEqual(chats.Min(user => user.LastModified), chats.Last().LastModified);
                Assert.AreEqual(i, chats.Count);
            }
        }

        [TestMethod]
        public void TestGetChatsByName()
        {
            var chats = ChatService.GetChatsByName(ChatMockings.GetGuid(0), "User").GetAwaiter().GetResult();
            foreach (var chat in chats)
            {
                var chatRes = ChatMockings.Chats.FirstOrDefault(ad => ad.Id == chat.Id);
                Assert.IsNotNull(chatRes);
                chatRes.LastModified = chat.LastModified;
            }
            Assert.AreEqual(2, chats.Count);
        }

        [TestMethod]
        public void TestGetChatsByNamePart()
        {
            var chats = new List<ChatViewModel>();
            for (int i = 1; i < 3; i++)
            {
                chats.AddRange(ChatService
                    .GetChatsByName(ChatMockings.GetGuid(0), "User" ,chats.Count == 0 ? DateTimeOffset.Now : chats.Last().LastModified, 1)
                    .GetAwaiter().GetResult());
                Assert.AreEqual(chats.Min(user => user.LastModified), chats.Last().LastModified);
                Assert.AreEqual(i, chats.Count);
            }
        }

        [TestMethod]
        public void TestGetChat()
        {
            var chat = ChatService.GetChat(ChatMockings.GetGuid(0), ChatMockings.GetGuid(0)).GetAwaiter().GetResult();
            Assert.IsNotNull(chat);
            Assert.AreEqual(2, chat.Messages.Count);
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
