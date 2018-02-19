using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BackendTest.Mockings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoomMateExpressWebApi.EF.Models;
using RoomMateExpressWebApi.EF.Services;

namespace BackendTest
{
    /// <summary>
    /// Summary description for AdminTest
    /// </summary>
    [TestClass]
    public class AdminTest
    {
        private const string ConnectionString = "Server=(localdb)\\mssqllocaldb;Database=RMEDatabase;Trusted_Connection=True;MultipleActiveResultSets=true";

        public AdminTest()
        {

        }

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        public RoomMateExpressDbContext DbContext { get; set; }

        public IAdminService AdminService { get; set; }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize]
        public static void AdminClassInitialize(TestContext testContext)
        {
            AdminMockings.Initalize();
        }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        [TestInitialize]
        public void AdminTestInitialize()
        {
            DbContext = new RoomMateExpressDbContext(ConnectionString);
            AdminService = new AdminService(DbContext);
        }
        //
        // Use TestCleanup to run code after each test has run
        [TestCleanup]
        public void AdminTestCleanup()
        {
            DbContext.Dispose();
            DbContext = null;
            AdminService = null;
        }
        #endregion

        [TestMethod]
        public void TestAddOrUpdateAdmins()
        {
            foreach (var admin in AdminMockings.Admins)
            {
                var result = AdminService.AddOrUpdateAdmin(admin).GetAwaiter().GetResult();
                //Thread.Sleep(1000);
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public void TestGetAllAdmins()
        {
            var admins = AdminService.GetAllAdmins().GetAwaiter().GetResult();
            foreach (var admin in admins)
            {
                var adminRes = AdminMockings.Admins.FirstOrDefault(ad => ad.Id == admin.Id);
                Assert.IsNotNull(adminRes);
                adminRes.CreationDate = admin.CreationDate;
            }
            Assert.AreEqual(30, admins.Count);
        }

        [TestMethod]
        public void TestGetAllAdminsPart()
        {
            var admins = new List<Admin>();
            for (int i = 1; i < 6; i++)
            {
                admins.AddRange(AdminService
                    .GetAllAdmins(admins.Count == 0 ? DateTimeOffset.Now : admins.Last().CreationDate, 5)
                    .GetAwaiter().GetResult());
                Assert.AreEqual(admins.Min(admin => admin.CreationDate), admins.Last().CreationDate);
                Assert.AreEqual(i * 5, admins.Count);
            }
        }

        [TestMethod]
        public void TestGetAdmin()
        {
            var admin = AdminService.GetAdmin(AdminMockings.GetGuid(10)).GetAwaiter().GetResult();
            Assert.AreEqual(AdminMockings.Admins[10], admin);
        }

        [TestMethod]
        public void TestDeleteAdmins()
        {
            foreach (var admin in AdminMockings.Admins)
            {
                Assert.IsTrue(AdminService.DeleteAdmin(admin.Id).GetAwaiter().GetResult());
            }
        }
    }
}
