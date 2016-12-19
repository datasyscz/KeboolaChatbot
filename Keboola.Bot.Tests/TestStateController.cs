using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Keboola.Bot;
using Keboola.Bot.Controllers;
using Keboola.Shared;
using Keboola.Shared.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Tests
{
    [TestClass]
    public class TestState
    {
        public static Mock<DbSet<T>> GenericSetupAsyncQueryableMockSet<T>(IQueryable<T> data) where T : class
        {
            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IDbAsyncEnumerable<T>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestDbAsyncEnumerator<T>(data.GetEnumerator()));
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(new TestDbAsyncQueryProvider<T>(data.Provider));
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSet;
        }

        [TestMethod]
        public void TestPostAddNewState()
        {
            StateModel model = new StateModel()
            {
                Active = true,
                Token = "sa54d6+5GHs4d65"
            };

            var dbContext = new TestDatabaseContext();
            dbContext.KeboolaUser = new TestDbSet<KeboolaUser>();
            dbContext.KeboolaToken = new TestDbSet<KeboolaToken>();
            var controller = new StateController(dbContext);

            var result = controller.Post(model);
            var ress = result as StatusCodeResult;
            Assert.AreEqual(dbContext.KeboolaUser.Count(),1);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(StatusCodeResult));
            Assert.AreEqual(HttpStatusCode.OK, ress.StatusCode);
            var savedUse = dbContext.KeboolaUser.FirstOrDefault(a => a.Token.Value == model.Token);
            Assert.IsNotNull(savedUse);
            Assert.AreEqual(savedUse.Token.Value, model.Token);
            Assert.IsTrue(savedUse.Active);
        }

        [TestMethod]
        public void TestPostConflictState()
        {
            StateModel model = GenerateRandomModel();
          

            var dbContext = new TestDatabaseContext();
            dbContext.KeboolaUser = new TestDbSet<KeboolaUser>();
            dbContext.KeboolaToken = new TestDbSet<KeboolaToken>();
            var controller = new StateController(dbContext);

            controller.Post(model);
            for (int i = 0; i < 100; i++)
                controller.Post(model);

            var result = controller.Post(model) as StatusCodeResult;
            Assert.AreEqual(HttpStatusCode.Conflict, result.StatusCode);
            var savedUse = dbContext.KeboolaUser.Count(a => a.Token.Value == model.Token);
            Assert.AreEqual(savedUse, 1);
        }

        [TestMethod]
        public void TestPutState()
        {
            StateModel model = GenerateRandomModel();
            model.Active = true;
            var dbContext = new TestDatabaseContext();
            dbContext.KeboolaUser = new TestDbSet<KeboolaUser>();
            dbContext.KeboolaToken = new TestDbSet<KeboolaToken>();
            var controller = new StateController(dbContext);
            controller.Post(model);
            var changedUser = dbContext.KeboolaUser.FirstOrDefault(a => a.Token.Value == model.Token);
            Assert.IsTrue(changedUser.Active);
            string token = model.Token;
            model.Token = null;
            model.Active = false;
            var result2 = controller.Put(token, model) as StatusCodeResult;
            Assert.AreEqual(HttpStatusCode.OK, result2.StatusCode);
            changedUser = dbContext.KeboolaUser.FirstOrDefault(a => a.Token.Value == token);
            Assert.IsFalse(changedUser.Active);
        }

        [TestMethod]
        public void TestPutExpiredState()
        {
            StateModel model = GenerateRandomModel();
            model.Active = true;
            StateController controller;
            var dbContext = FillRandomData(out controller);

            controller.Post(model);

            //Fake expiration
            dbContext.KeboolaToken.FirstOrDefault(a=> a.Value == model.Token).Expiration = DateTime.Now - new TimeSpan(40,0,0,0);
            
            model.Active = false;
            var result = controller.Put(model.Token, model) as StatusCodeResult;

            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
            var unchangedState = dbContext.KeboolaUser.FirstOrDefault(a => a.Token.Value == model.Token);
            Assert.IsTrue(unchangedState.Active);
        }

        [TestMethod]
        public void TestPutDoesntExist()
        {
            StateController controller;
            var dbContext = FillRandomData(out controller);

            StateModel model = GenerateRandomModel();
            var result2 = controller.Put(model.Token, model) as StatusCodeResult;
            Assert.AreEqual(HttpStatusCode.NotFound, result2.StatusCode);
            var nullToken = dbContext.KeboolaUser.FirstOrDefault(a => a.Token.Value == model.Token);
            Assert.IsNull(nullToken);
        }

        private TestDatabaseContext FillRandomData(out StateController controller)
        {
            var dbContext = new TestDatabaseContext();
            dbContext.KeboolaUser = new TestDbSet<KeboolaUser>();
            dbContext.KeboolaToken = new TestDbSet<KeboolaToken>();
            controller = new StateController(dbContext);
            for (int i = 0; i < 100; i++)
                controller.Post(GenerateRandomModel());
            return dbContext;
        }

        [TestMethod]
        public void TestPostAddNewStateTwice()
        {
            StateModel model = new StateModel()
            {
                Active = true,
                Token = "sa54d6+5ADs4d65"
            };

            var dbContext = new TestDatabaseContext();
            dbContext.KeboolaUser = new TestDbSet<KeboolaUser>();
            dbContext.KeboolaToken = new TestDbSet<KeboolaToken>();
            var controller = new StateController(dbContext);
            
            controller.Post(model);
            var result = controller.Post(model);
            var result2 = result as StatusCodeResult;
            Assert.IsNotNull(result2);
            Assert.IsInstanceOfType(result2, typeof(StatusCodeResult));
            Assert.AreEqual(HttpStatusCode.Conflict, result2.StatusCode);
        }

        [TestMethod]
        public void TestPostAddMoreNewStates()
        {
            var dbContext = new TestDatabaseContext();
            dbContext.KeboolaUser = new TestDbSet<KeboolaUser>();
            dbContext.KeboolaToken = new TestDbSet<KeboolaToken>();
            var controller = new StateController(dbContext);
            for (int i = 0; i < 20; i++)
            {
                StateModel model = GenerateRandomModel();
                var result = controller.Post(model) as StatusCodeResult;
                Assert.AreEqual(dbContext.KeboolaUser.Count(), i+1);
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(StatusCodeResult));
                Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
                var savedUse = dbContext.KeboolaUser.FirstOrDefault(a => a.Token.Value == model.Token);
                Assert.IsNotNull(savedUse);
                Assert.AreEqual(savedUse.Token.Value, model.Token);
                Assert.IsTrue(savedUse.Active == model.Active);
            }
        }

        public StateModel GenerateRandomModel()
        {
            return new StateModel()
            {
                Active = (new Random()).Next(0,1) == 1,
                Token = Guid.NewGuid().ToString()
            };
        }
    }
}
