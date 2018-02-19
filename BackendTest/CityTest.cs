using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoomMateExpressWebApi.EF.Models;
using RoomMateExpressWebApi.EF.Services;

namespace BackendTest
{
    /// <summary>
    /// Summary description for CityTest
    /// </summary>
    [TestClass]
    public class CityTest
    {
        private const string ConnectionString = "Server=(localdb)\\mssqllocaldb;Database=RMEDatabase;Trusted_Connection=True;MultipleActiveResultSets=true";

        public CityTest()
        {

        }

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        public RoomMateExpressDbContext DbContext { get; set; }

        public ICityService CityService { get; set; }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        [TestInitialize]
        public void CityTestInitialize()
        {
            DbContext = new RoomMateExpressDbContext(ConnectionString);
            CityService = new CityService(DbContext);
        }
        //
        // Use TestCleanup to run code after each test has run
        [TestCleanup]
        public void CityTestCleanup()
        {
            DbContext.Dispose();
            DbContext = null;
            CityService = null;
        }
        #endregion

        [TestMethod]
        public void TestGetAllCities()
        {
            var cities = CityService.GetAllCities().GetAwaiter().GetResult();
            Assert.AreEqual(3, cities.Count);
            foreach (var city in cities)
            {
                Assert.IsTrue(city.Neighborhoods.Count > 0);
            }
        }

        [TestMethod]
        public void TestGetCity()
        {
            var city = CityService.GetCity(Guid.Parse("6b8a944c-5aca-4f72-962e-1284324fa1b8")).GetAwaiter().GetResult();
            Assert.AreEqual("Zagreb", city.Name);
        }
    }
}
