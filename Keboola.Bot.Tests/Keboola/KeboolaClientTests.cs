using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Keboola.Bot.Keboola.Tests
{
    [TestClass]
    public class KeboolaClientTests
    {
        // [TestMethod]
        //  public async Task RefreshTokenAsyncTest()
        //  {
        //   var dbContext = EntityFrameworkMock.Create<DatabaseContext>();
        //   var service = new DatabaseService(dbContext);
        //   dbContext.KeboolaUser.Add(new KeboolaUser
        //   {
        //       KeboolaId = 0,
        //       Active = true,
        //       Token = new KeboolaToken
        //       {
        //           Expiration = DateTime.Now + TimeSpan.FromDays(30),
        //           Value = "newTokenUnchanged"
        //       }
        //   });

        //   dbContext.KeboolaUser.Add(new KeboolaUser
        //   {
        //       KeboolaId = 1,
        //       Active = true,
        //       Token = new KeboolaToken
        //       {
        //           Expiration = DateTime.Now - TimeSpan.FromDays(-20),
        //           Value = "oldToken"
        //       }
        //   });
        //   var mock = new Mock<IKeboolaClient>();
        ////   mock.Setup(foo => foo.RefreshTokenAsync("oldToken")).Returns(Task.FromResult("newToken"));

        //   var tokenSheduler = new TokenShedulerJob(dbContext, mock.Object);
        //   tokenSheduler.Execute(null);
        //   var ssdsd = dbContext.KeboolaUser.Where(a => a.Token.Expiration.Ticks < DateTime.Now.Ticks).ToList();
        //   var userUnchaged = await dbContext.KeboolaUser.FirstOrDefaultAsync(a => a.KeboolaId == 0);
        //   //Fail if change this
        //   Assert.AreEqual(userUnchaged.Token.Value, "newTokenUnchanged");

        //   var userChanged = await dbContext.KeboolaUser.FirstOrDefaultAsync(a => a.KeboolaId == 1);
        //   Assert.AreEqual(userChanged.Token.Value, "newToken");
        //   }
    }
}