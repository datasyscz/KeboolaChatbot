using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Keboola.Bot.Job;
using Keboola.Bot.Service;
using Keboola.Shared.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Telerik.JustMock.EntityFramework;

namespace Keboola.Bot.Keboola.Tests
{
    [TestClass]
    public class KeboolaClientTests
    {
        [TestMethod]
        public async Task RefreshTokenAsyncTest()
        {
            var dbContext = EntityFrameworkMock.Create<DatabaseContext>();
            var service = new DatabaseService(dbContext);
            dbContext.KeboolaUser.Add(new KeboolaUser
            {
                Id = 0,
                Active = true,
                Token = new KeboolaToken
                {
                    Expiration = DateTime.Now + TimeSpan.FromDays(30),
                    Value = "newTokenUnchanged"
                }
            });

            dbContext.KeboolaUser.Add(new KeboolaUser
            {
                Id = 1,
                Active = true,
                Token = new KeboolaToken
                {
                    Expiration = DateTime.Now - TimeSpan.FromDays(-20),
                    Value = "oldToken"
                }
            });
            var mock = new Mock<IKeboolaClient>();
         //   mock.Setup(foo => foo.RefreshTokenAsync("oldToken")).Returns(Task.FromResult("newToken"));

            var tokenSheduler = new TokenShedulerJob(dbContext, mock.Object);
            tokenSheduler.Execute(null);
            var ssdsd = dbContext.KeboolaUser.Where(a => a.Token.Expiration.Ticks < DateTime.Now.Ticks).ToList();
            var userUnchaged = await dbContext.KeboolaUser.FirstOrDefaultAsync(a => a.Id == 0);
            //Fail if change this
            Assert.AreEqual(userUnchaged.Token.Value, "newTokenUnchanged");

            var userChanged = await dbContext.KeboolaUser.FirstOrDefaultAsync(a => a.Id == 1);
            Assert.AreEqual(userChanged.Token.Value, "newToken");
        }
    }
}