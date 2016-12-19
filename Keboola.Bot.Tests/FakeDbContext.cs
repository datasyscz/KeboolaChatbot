using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using Keboola.Bot;
using Keboola.Bot.Service;
using Microsoft.Bot.Builder.Dialogs;
using Moq;
using Tests;

namespace Keboola.Shared.Models
{
    [TestClass]
    public static class FakeDbContext
    {
       private static Mock<DbSet<T>> MockDbSet<T>(List<T> list ) where T : class
        {
            var data = list.AsQueryable();
            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IDbAsyncEnumerable<T>>().Setup(m => m.GetAsyncEnumerator()).Returns(new TestDbAsyncEnumerator<T>(data.GetEnumerator()));
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(new TestDbAsyncQueryProvider<T>(data.Provider));
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            mockSet.Setup(m => m.Add(It.IsAny<T>())).Callback<T>(list.Add);
            return mockSet;
        }

        public static DatabaseService GetService(Mock<IDatabaseContext> mockContext)
        {
            List<KeboolaUser> users = new List<KeboolaUser>();
            List<KeboolaToken> tokens = new List<KeboolaToken>();
            mockContext.Setup(c => c.KeboolaUser).Returns(MockDbSet<KeboolaUser>(users).Object);
            mockContext.Setup(c => c.KeboolaToken).Returns(MockDbSet<KeboolaToken>(tokens).Object);
       //     mockContext.Setup(c => c.SaveChangesAsync()).Returns(() => Task.Run(() => { return 1; })).Verifiable();
       //     mockContext.Setup(c => c.SaveChanges()).Verifiable();
            //mockContext.Setup(c => c.Channel).Returns(MockDbSet<Channel>().Object);
            // mockContext.Setup(c => c.Conversation).Returns(MockDbSet<Conversation>().Object);

            var service = new DatabaseService(mockContext.Object);
            return service;
        }
    }
}
