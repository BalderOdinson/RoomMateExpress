using System;
using System.Text;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using BackendTest.Mockings;
using Dapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoomMateExpressWebApi.EF.Exceptions;
using RoomMateExpressWebApi.EF.Models;
using RoomMateExpressWebApi.EF.Services;

namespace BackendTest
{
    /// <summary>
    /// Summary description for MessageTest
    /// </summary>
    [TestClass]
    public class MessageTest
    {
        private const string ConnectionString = "Server=(localdb)\\mssqllocaldb;Database=RMEDatabase;Trusted_Connection=True;MultipleActiveResultSets=true";

        public MessageTest()
        {
            
        }

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        public RoomMateExpressDbContext DbContext { get; set; }

        public IMessageService MessageService { get; set; }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize]
        public static void MessageClassInitialize(TestContext testContext)
        {
            MessageMockings.Initalize();
        }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        [TestInitialize]
        public void MessageTestInitialize()
        {
            DbContext = new RoomMateExpressDbContext(ConnectionString);
            MessageService = new MessageService(DbContext);
        }
        //
        // Use TestCleanup to run code after each test has run
        [TestCleanup]
        public void MessageTestCleanup()
        {
            DbContext.Dispose();
            DbContext = null;
            MessageService = null;
        }
        #endregion

        [TestMethod]
        public void TestSendMessage()
        {
            foreach (var message in MessageMockings.Messages)
            {
                var result = MessageService.SendMessage(message).GetAwaiter().GetResult();
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(UserNotFoundException))]
        public void TestSendMessageUserNotFound()
        {
            var message = MessageMockings.Messages.FirstOrDefault();
            message.UserSender.Id = Guid.Empty;
            var result = MessageService.SendMessage(message).GetAwaiter().GetResult();
        }

        [TestMethod]
        [ExpectedException(typeof(ChatNotFoundException))]
        public void TestSendMessageChatNotFound()
        {
            var message = MessageMockings.Messages.FirstOrDefault();
            message.Chat.Id = Guid.Empty;
            var result = MessageService.SendMessage(message).GetAwaiter().GetResult();
        }

        [TestMethod]
        public void TestGetMessages()
        {
            var guid = MessageMockings.GetGuid(0);
            var messagesCount = (DbContext.Database.Connection as SqlConnection).QuerySingle<int>("SELECT COUNT(*) FROM Messages WHERE Messages.Chat_Id = @ChatId", new { ChatId = guid });
            var messages = MessageService.GetMessages(MessageMockings.GetGuid(0)).GetAwaiter().GetResult();
            Assert.AreEqual(messagesCount, messages.Count);
        }

        [TestMethod]
        public void TestGetMessagesPart()
        {
            var guid = MessageMockings.GetGuid(0);
            var messagesCount = (DbContext.Database.Connection as SqlConnection).QuerySingle<int>("SELECT COUNT(*) FROM Messages WHERE Messages.Chat_Id = @ChatId", new { ChatId = guid });
            var messages = new List<Message>();
            while (true)
            {
                var newMessages = MessageService
                    .GetMessages(MessageMockings.GetGuid(0),
                        messages.Count == 0 ? DateTimeOffset.Now : messages.Last().SentAt, 5)
                    .GetAwaiter().GetResult();
                if(newMessages.Count == 0)
                    break;
                messages.AddRange(newMessages);
                Assert.AreEqual(messages.Min(message => message.SentAt), messages.Last().SentAt);
            }
            Assert.AreEqual(messagesCount, messages.Count);
        }

        [TestMethod]
        public void TestGetNewMessages()
        {
            var messages = MessageService.GetNewMessages(MessageMockings.GetGuid(0), DateTimeOffset.MinValue, 20).GetAwaiter().GetResult();
            Assert.AreEqual(20, messages.Count);
        }

        [TestMethod]
        public void TestGetMessage()
        {
            var guid = (DbContext.Database.Connection as SqlConnection).QueryFirst<Guid>("SELECT Messages.Id FROM Messages");
            var message = MessageService.GetMessage(guid).GetAwaiter().GetResult();
            Assert.AreEqual(guid, message.Id);
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
