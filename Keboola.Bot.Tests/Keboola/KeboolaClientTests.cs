using Microsoft.VisualStudio.TestTools.UnitTesting;
using Keboola.Bot.Keboola;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Keboola.Bot.Job;
using Keboola.Shared.Models;
using Moq;

namespace Keboola.Bot.Keboola.Tests
{
    [TestClass()]
    public class KeboolaClientTests
    {
        [TestMethod()]
        public async Task RefreshTokenAsyncTest()
        {
            var dbContext = new Mock<IDatabaseContext>();
            var service = FakeDbContext.GetService(dbContext);
            dbContext.Object.KeboolaUser.Add(new KeboolaUser()
            {
                Id = 0,
                Active = true,
                Token = new KeboolaToken()
                {
                    Expiration = DateTime.Now + TimeSpan.FromDays(30),
                    Value = "newTokenUnchanged"
                }
            });

            dbContext.Object.KeboolaUser.Add(new KeboolaUser()
            {
                Id = 1,
                Active = true,
                Token = new KeboolaToken()
                {
                    Expiration = DateTime.Now - TimeSpan.FromDays(-20),
                    Value = "oldToken"
                }
            });
            var mock = new Mock<IKeboolaClient>();
            mock.Setup(foo => foo.RefreshTokenAsync("oldToken")).Returns(Task.FromResult("newToken"));

            TokenShedulerJob tokenSheduler = new TokenShedulerJob(dbContext.Object, mock.Object);
            tokenSheduler.Execute(null);
            var ssdsd = dbContext.Object.KeboolaUser.Where(a => a.Token.Expiration.Ticks < DateTime.Now.Ticks).ToList();
            var userUnchaged = await dbContext.Object.KeboolaUser.FirstOrDefaultAsync(a => a.Id == 0);
            //Fail if change this
            Assert.AreEqual(userUnchaged.Token.Value, "newTokenUnchanged");

            var userChanged = await dbContext.Object.KeboolaUser.FirstOrDefaultAsync(a => a.Id == 1);
            Assert.AreEqual(userChanged.Token.Value, "newToken");

        }
    }
}