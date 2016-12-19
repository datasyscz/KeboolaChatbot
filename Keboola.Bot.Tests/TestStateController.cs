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
using Microsoft.IdentityModel.Protocols;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Tests
{
    [TestClass]
    public class TestState
    {
        [TestMethod]
        public async Task TestPostAddNewState()
        {
            StateModel model = new StateModel()
            {
                Active = true,
                Token = "sa54d6+5GHs4d65"
            };

            var dbContext = new Mock<IDatabaseContext>();
            var service = FakeDbContext.GetService(dbContext);
            var controller = new StateController(dbContext.Object);

            var result =await controller.Post(model);
            var ress = result as StatusCodeResult;
            Assert.AreEqual(dbContext.Object.KeboolaUser.Count(),1);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(StatusCodeResult));
            Assert.AreEqual(HttpStatusCode.OK, ress.StatusCode);
            var savedUse = dbContext.Object.KeboolaUser.FirstOrDefault(a => a.Token.Value == model.Token);
            Assert.IsNotNull(savedUse);
            Assert.AreEqual(savedUse.Token.Value, model.Token);
            Assert.IsTrue(savedUse.Active);
        }

        [TestMethod]
        public async Task TestPostConflictState()
        {
            StateModel model = GenerateRandomModel();
          
            var dbContext = new Mock<IDatabaseContext>();
            var service = FakeDbContext.GetService(dbContext);
            var controller = new StateController(dbContext.Object);

            await controller.Post(model);
            for (int i = 0; i < 100; i++)
                await controller.Post(model);

            var result = await controller.Post(model) as StatusCodeResult;
            Assert.AreEqual(HttpStatusCode.Conflict, result.StatusCode);
            var savedUse = dbContext.Object.KeboolaUser.Count(a => a.Token.Value == model.Token);
            Assert.AreEqual(savedUse, 1);
        }

        [TestMethod]
        public async Task TestPutState()
        {
            StateModel model = GenerateRandomModel();
            model.Active = true;
            var dbContext = new Mock<IDatabaseContext>();
            var service = FakeDbContext.GetService(dbContext);
            var controller = new StateController(dbContext.Object);
            await controller.Post(model);
            var changedUser = dbContext.Object.KeboolaUser.FirstOrDefault(a => a.Token.Value == model.Token);
            Assert.IsTrue(changedUser.Active);
            string token = model.Token;
            model.Token = null;
            model.Active = false;
            var result2 = await controller.Put(token, model) as StatusCodeResult;
            Assert.AreEqual(HttpStatusCode.OK, result2.StatusCode);
            changedUser = dbContext.Object.KeboolaUser.FirstOrDefault(a => a.Token.Value == token);
            Assert.IsFalse(changedUser.Active);
        }

        [TestMethod]
        public async Task TestPutExpiredState()
        {
            StateModel model = GenerateRandomModel();
            model.Active = true;
            StateController controller;
            var dbContext = FillRandomData(out controller);
            await controller.Post(model);
            //Fake expiration
            var user = await dbContext.Object.KeboolaUser.FirstOrDefaultAsync(a => a.Token.Value == model.Token);
            user.Token.Expiration = DateTime.Now - new TimeSpan(40,0,0,0);
            
            model.Active = false;
            var result =await controller.Put(model.Token, model) as StatusCodeResult;

            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
            var unchangedState = dbContext.Object.KeboolaUser.FirstOrDefault(a => a.Token.Value == model.Token);
            Assert.IsTrue(unchangedState.Active);
        }

        [TestMethod]
        public async Task TestPutDoesntExist()
        {
            StateController controller;
            var dbContext = FillRandomData(out controller);

            StateModel model = GenerateRandomModel();
            var result2 =await controller.Put(model.Token, model) as StatusCodeResult;
            Assert.AreEqual(HttpStatusCode.NotFound, result2.StatusCode);
            var nullToken = dbContext.Object.KeboolaUser.FirstOrDefault(a => a.Token.Value == model.Token);
            Assert.IsNull(nullToken);
        }

        private Mock<IDatabaseContext> FillRandomData(out StateController controller)
        {
            var dbContext = new Mock<IDatabaseContext>();
            var service = FakeDbContext.GetService(dbContext);
            controller = new StateController(dbContext.Object);
            for (int i = 0; i < 100; i++)
                controller.Post(GenerateRandomModel());
            return dbContext;
        }

        [TestMethod]
        public async Task TestPostAddNewStateTwice()
        {
            StateModel model = new StateModel()
            {
                Active = true,
                Token = "sa54d6+5ADs4d65"
            };

            var dbContext = new Mock<IDatabaseContext>();
            var service = FakeDbContext.GetService(dbContext);
            var realContext = dbContext.Object;
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


            for (int i = 0; i < 20; i++)
            {
                StateModel model = GenerateRandomModel();
                var result = await controller.Post(model) as StatusCodeResult;
                Assert.AreEqual(dbContext.Object.KeboolaUser.Count(), i+1);
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(StatusCodeResult));
                Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
                var savedUse = dbContext.Object.KeboolaUser.FirstOrDefault(a => a.Token.Value == model.Token);
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
