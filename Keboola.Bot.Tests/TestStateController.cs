using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Keboola.Bot;
using Keboola.Bot.Controllers;
using Keboola.Bot.Service;
using Keboola.Shared.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Telerik.JustMock.EntityFramework;

namespace Tests
{
    [TestClass]
    public class TestState
    {
        [TestMethod]
        public async Task TestPostAddNewState()
        {
            var model = new StateModel
            {
                Active = true,
                Token = "sa54d6+5GHs4d65",
                Id = 5
            };

            var dbContext = new Mock<IDatabaseContext>();
            var service = FakeDbContext.GetService(dbContext);
            var controller = new StateController(dbContext.Object);

            var result = await controller.Post(model);
            var ress = result as StatusCodeResult;
            Assert.AreEqual(dbContext.Object.KeboolaUser.Count(), 1);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(StatusCodeResult));
            Assert.AreEqual(HttpStatusCode.OK, ress.StatusCode);
            var savedUse = dbContext.Object.KeboolaUser.FirstOrDefault(a => a.Token.Value == model.Token);
            Assert.IsNotNull(savedUse);
            Assert.AreEqual(savedUse.Token.Value, model.Token);
            Assert.AreEqual(savedUse.KeboolaId, model.Id);
            Assert.IsTrue(savedUse.Active);
        }

        [TestMethod]
        public async Task TestPostConflictState()
        {
            var model = GenerateRandomModel();

            var dbContext = EntityFrameworkMock.Create<DatabaseContext>();
            var service = new DatabaseService(dbContext);
            var controller = new StateController(dbContext);

            await controller.Post(model);
            for (var i = 0; i < 100; i++)
                await controller.Post(GenerateRandomModel(i));

            var result = await controller.Post(model) as StatusCodeResult;
            Assert.AreEqual(HttpStatusCode.Conflict, result.StatusCode);
            var savedUse = dbContext.KeboolaUser.Count(a => a.Token.Value == model.Token);
            Assert.AreEqual(savedUse, 1);
        }

        [TestMethod]
        public async Task TestPostConflictSameIdDifferentTokenState()
        {
            var model = GenerateRandomModel();

            var dbContext = EntityFrameworkMock.Create<DatabaseContext>();
            var service = new DatabaseService(dbContext);
            var controller = new StateController(dbContext);

            await controller.Post(model);
            for (var i = 1; i < 100; i++)
                await controller.Post(GenerateRandomModel(i));

            var oldToken = model.Token;
            model.Token = "DifferentToken";
            var result = await controller.Post(model) as StatusCodeResult;
            Assert.AreEqual(HttpStatusCode.Conflict, result.StatusCode);
            var savedUse = dbContext.KeboolaUser.Count(a => a.Token.Value == oldToken);
            Assert.AreEqual(savedUse, 1);

            var anyNewRecord = dbContext.KeboolaUser.Any(a => a.Token.Value == model.Token);
            Assert.IsFalse(anyNewRecord);

            var numberIdRecords = dbContext.KeboolaUser.Count(a => a.KeboolaId == model.Id);
            Assert.AreEqual(numberIdRecords, 1);
        }

        [TestMethod]
        public async Task TestPutState()
        {
            var model = GenerateRandomModel();
            model.Active = true;
            //  var dbContext = new Mock<IDatabaseContext>();
            var dbContext = EntityFrameworkMock.Create<DatabaseContext>();
            //    var service = FakeDbContext.GetService(dbContext);
            var service = new DatabaseService(dbContext);
            var controller = new StateController(dbContext);


            await controller.Post(model);
            var changedUser = dbContext.KeboolaUser.FirstOrDefault(a => a.Token.Value == model.Token);
            Assert.IsTrue(changedUser.Active);
            var id = 0;
            var newToken = model.Token;
            model.Token = model.Token;
            model.Active = false;
            var result2 = await controller.Put(id, model) as StatusCodeResult;
            Assert.AreEqual(HttpStatusCode.OK, result2.StatusCode);
            changedUser = dbContext.KeboolaUser.FirstOrDefault(a => a.KeboolaId == id);
            Assert.IsFalse(changedUser.Active);
        }

        [TestMethod]
        public async Task TestPutExpiredState()
        {
            var model = GenerateRandomModel(1);
            model.Active = true;
            StateController controller;
            var dbContext = await FillRandomData();
            controller = new StateController(dbContext);

            await controller.Post(model);
            //Fake expiration
            var user = await dbContext.KeboolaUser.FirstOrDefaultAsync(a => a.Token.Value == model.Token);
            user.Token.Expiration = DateTime.Now - new TimeSpan(40, 0, 0, 0);

            model.Active = false;
            var result = await controller.Put((int) model.Id, model) as StatusCodeResult;

            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
            var unchangedState = dbContext.KeboolaUser.FirstOrDefault(a => a.Token.Value == model.Token);
            Assert.IsTrue(unchangedState.Active);
        }

        [TestMethod]
        public async Task TestPutDoesntExist()
        {
            StateController controller;
            var dbContext = await FillRandomData(1);
            controller = new StateController(dbContext);

            var model = GenerateRandomModel(65544);
            var result2 = await controller.Put((int) model.Id, model) as StatusCodeResult;
            Assert.AreEqual(HttpStatusCode.NotFound, result2.StatusCode);
            var nullToken = dbContext.KeboolaUser.FirstOrDefault(a => a.Token.Value == model.Token);
            Assert.IsNull(nullToken);
        }

        private async Task<DatabaseContext> FillRandomData(int startId = 0)
        {
            var dbContext = EntityFrameworkMock.Create<DatabaseContext>();
            //     var dbContext = new Mock<IDatabaseContext>();
            //   var service = FakeDbContext.GetService(dbContext);
            var controller = new StateController(dbContext);
            for (var i = startId; i < 100; i++)
                await controller.Post(GenerateRandomModel());
            return dbContext;
        }

        [TestMethod]
        public async Task TestPostAddNewStateTwice()
        {
            var model = new StateModel
            {
                Active = true,
                Token = "sa54d6+5ADs4d65",
                Id = 6006
            };

            var dbContext = EntityFrameworkMock.Create<DatabaseContext>();
            var service = new DatabaseService(dbContext);
            var realContext = dbContext;
            var controller = new StateController(realContext);

            await controller.Post(model);

            var result = await controller.Post(model);
            var result2 = result as StatusCodeResult;
            Assert.IsNotNull(result2);
            Assert.IsInstanceOfType(result2, typeof(StatusCodeResult));
            Assert.AreEqual(HttpStatusCode.Conflict, result2.StatusCode);
        }

        [TestMethod]
        public async Task TestPostAddMoreNewStates()
        {
            var dbContext = new Mock<IDatabaseContext>();
            var service = FakeDbContext.GetService(dbContext);
            var controller = new StateController(dbContext.Object);

            for (var i = 0; i < 20; i++)
            {
                var model = GenerateRandomModel(i);
                var result = await controller.Post(model) as StatusCodeResult;
                Assert.AreEqual(dbContext.Object.KeboolaUser.Count(), i + 1);
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(StatusCodeResult));
                Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
                var savedUse = dbContext.Object.KeboolaUser.FirstOrDefault(a => a.Token.Value == model.Token);
                Assert.IsNotNull(savedUse);
                Assert.AreEqual(savedUse.Token.Value, model.Token);
                Assert.AreEqual(savedUse.KeboolaId, model.Id);
                Assert.IsTrue(savedUse.Active == model.Active);
            }
        }

        public StateModel GenerateRandomModel(int id = 0)
        {
            return new StateModel
            {
                Active = new Random().Next(0, 1) == 1,
                Token = Guid.NewGuid().ToString(),
                Id = id
            };
        }
    }
}