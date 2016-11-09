using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using EntityFramework.MoqHelper;
using Keboola.Bot;
using Keboola.Shared;
using Microsoft.Bot.Builder.Tests;
using Microsoft.Bot.Connector;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Tests
{
    [TestClass]
    public class LogConversationTest
    {

        [TestMethod]
        public async Task GetAllBlogs_orders_by_name()
        {
            var conversationSet = EntityFrameworkMoqHelper.CreateMockForDbSet<Conversation>();
           // var channelSet = EntityFrameworkMoqHelper.CreateMockForDbSet<Channel>();
            var messagesSet = EntityFrameworkMoqHelper.CreateMockForDbSet<Message>();
            var userSet = EntityFrameworkMoqHelper.CreateMockForDbSet<User>();


            var data = new List<Channel>
            {
                
            }.AsQueryable();

            var channelSet = new Mock<DbSet<Channel>>();
            channelSet.As<IDbAsyncEnumerable<Channel>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestDbAsyncEnumerator<Channel>(data.GetEnumerator()));

            channelSet.As<IQueryable<Channel>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<Channel>(data.Provider));

            channelSet.As<IQueryable<Channel>>().Setup(m => m.Expression).Returns(data.Expression);
            channelSet.As<IQueryable<Channel>>().Setup(m => m.ElementType).Returns(data.ElementType);
            channelSet.As<IQueryable<Channel>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<IDatabaseContext>();
            mockContext.Setup(c => c.Conversation).Returns(conversationSet.Object);
            mockContext.Setup(c => c.Channel).Returns(channelSet.Object);
            mockContext.Setup(c => c.Messages).Returns(messagesSet.Object);
            mockContext.Setup(c => c.Customer).Returns(userSet.Object);


            ConversationLogger logger = new ConversationLogger(mockContext.Object);
            IMessageActivity activity = DialogTestBase.MakeTestMessage();
            var conversation = await logger.AddOrUpdateConversation(activity);
            activity = DialogTestBase.MakeTestMessage();
            var conversation2 = await logger.AddOrUpdateConversation(activity);
            int vv = mockContext.Object.Conversation.Count();

            activity = DialogTestBase.MakeTestMessage();
            var conversation3 = await logger.AddOrUpdateConversation(activity);

            //Assert.AreEqual(3, blogs.Count);
            //Assert.AreEqual("AAA", blogs[0].Name);
            //Assert.AreEqual("BBB", blogs[1].Name);
            //Assert.AreEqual("ZZZ", blogs[2].Name);
        }
    }
}
