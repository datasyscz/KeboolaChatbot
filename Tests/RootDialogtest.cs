using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

using Microsoft.Bot.Builder.Tests;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Autofac;
using System.Threading;
using KeboolaChatbot.Dialogs;


namespace Tests
{
    [TestClass]
    public class RootDialogtest : DialogTestBase
    {
        [TestMethod]
        public async Task RootDialogAccesDocumentClient()
        {
            Func<IDialog<object>> MakeRoot;
            IContainer container;
            var toBot = InitDialog(out MakeRoot, out container);

            //Y0
            // act: sending the message
            var responses = await GetResponse(container, MakeRoot, toBot);
            Assert.IsTrue(responses.Count == 2);
            Assert.IsTrue(responses.Dequeue().Text.Contains("Hello"));
            Assert.IsTrue(responses.Dequeue().Text.Contains("name"));

            toBot.Text = "David";
            responses = await GetResponse(container, MakeRoot, toBot);
            Assert.IsTrue(responses.Count == 1);
            string response = ((HeroCard) responses.Dequeue().Attachments[0].Content).Text;
            Assert.IsTrue(response.Contains("access to REST"));

            //Y1
            toBot.Text = "yes";
            responses = await GetResponse(container, MakeRoot, toBot);
            Assert.IsTrue(responses.Count == 2);
            response = responses.Dequeue().Text;
            Assert.IsTrue(response.Contains("Try to call"));
            response = ((HeroCard) responses.Dequeue().Attachments[0].Content).Text;
            Assert.IsTrue(response.Contains("documentation"));

            //Y2
            toBot.Text = "yes";
            responses = await GetResponse(container, MakeRoot, toBot);
            Assert.IsTrue(responses.Count == 2);
            response = responses.Dequeue().Text;
            Assert.IsTrue(response.Contains("need to find out some information"));
            response = ((HeroCard)responses.Dequeue().Attachments[0].Content).Text;
            Assert.IsTrue(response.Contains("REST client?"));

            //Y3
            toBot.Text = "yes";
            responses = await GetResponse(container, MakeRoot, toBot);
            Assert.IsTrue(responses.Count == 1);
            response = responses.Dequeue().Text;
            Assert.IsTrue(response.Contains("Did you try to get some data via REST Client?"));
        }

        [TestMethod]
        public async Task RootDialogNoDocum()
        {
            Func<IDialog<object>> MakeRoot;
            IContainer container;
            var toBot = InitDialog(out MakeRoot, out container);

            //Y0
            // act: sending the message
            var responses = await GetResponse(container, MakeRoot, toBot);
            Assert.IsTrue(responses.Count == 2);
            Assert.IsTrue(responses.Dequeue().Text.Contains("Hello"));
            Assert.IsTrue(responses.Dequeue().Text.Contains("name"));

            toBot.Text = "David";
            responses = await GetResponse(container, MakeRoot, toBot);
            Assert.IsTrue(responses.Count == 1);
            string response = ((HeroCard)responses.Dequeue().Attachments[0].Content).Text;
            Assert.IsTrue(response.Contains("access to REST"));

            //Y1
            toBot.Text = "no";
            responses = await GetResponse(container, MakeRoot, toBot);
            Assert.IsTrue(responses.Count == 1);
            response = responses.Dequeue().Text;
            Assert.IsTrue(response.Contains("credentials"));
        }

        [TestMethod]
        public async Task RootDialogNoClient()
        {
            Func<IDialog<object>> MakeRoot;
            IContainer container;
            var toBot = InitDialog(out MakeRoot, out container);

            //Y0
            // act: sending the message
            var responses = await GetResponse(container, MakeRoot, toBot);
            Assert.IsTrue(responses.Count == 2);
            Assert.IsTrue(responses.Dequeue().Text.Contains("Hello"));
            Assert.IsTrue(responses.Dequeue().Text.Contains("name"));

            toBot.Text = "David";
            responses = await GetResponse(container, MakeRoot, toBot);
            Assert.IsTrue(responses.Count == 1);
            string response = ((HeroCard)responses.Dequeue().Attachments[0].Content).Text;
            Assert.IsTrue(response.Contains("access to REST"));

            //Y1
            toBot.Text = "yes";
            responses = await GetResponse(container, MakeRoot, toBot);
            Assert.IsTrue(responses.Count == 2);
            response = responses.Dequeue().Text;
            Assert.IsTrue(response.Contains("great"));
            response = ((HeroCard)responses.Dequeue().Attachments[0].Content).Text;
            Assert.IsTrue(response.Contains("documentation"));

            //N2
            toBot.Text = "no";
            responses = await GetResponse(container, MakeRoot, toBot);
            Assert.IsTrue(responses.Count == 1);
            response = responses.Dequeue().Text;
            Assert.IsTrue(response.Contains("should ask someone"));
        }

        private static IMessageActivity InitDialog(out Func<IDialog<object>> MakeRoot, out IContainer container)
        {
            IMessageActivity toBot;
            IDialog<object> echoDialog = new RootDialog().BuildChain();
            // arrange
            toBot = DialogTestBase.MakeTestMessage();
            toBot.From.Id = Guid.NewGuid().ToString();
            toBot.Text = "ConversationUpdate";
            //toBot.Locale = "cs-CZ";

            MakeRoot = () => echoDialog;

            new FiberTestBase.ResolveMoqAssembly(echoDialog);
            container = Build(Options.MockConnectorFactory | Options.ScopedQueue, echoDialog);
            return toBot;
        }

        private async Task<Queue<IMessageActivity>> GetResponse(IContainer container, Func<IDialog<object>> makeRoot, IMessageActivity toBot)
        {
            using (var scope = DialogModule.BeginLifetimeScope(container, toBot))
            {
                DialogModule_MakeRoot.Register(scope, makeRoot);
                //  var token = CancellationToken.None;
                // act: sending the message
                using (new LocalizedScope(toBot.Locale))
                {
                    var task = scope.Resolve<IPostToBot>();
                    await task.PostAsync(toBot, default(CancellationToken));
                }
                // await Conversation.SendAsync(scope, toBot);
                return scope.Resolve<Queue<IMessageActivity>>();
            }
        }
    }
}