using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using AI;
using API;
using Autofac;
using Keboola.Bot.Dialogs;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Builder.Tests;
using Microsoft.Bot.Connector;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Keboola.Bot.Tests
{
    [TestClass]
    public class RootDialogtest : DialogTestBase
    {
        private IContainer container;
        private Func<IDialog<object>> MakeRoot;
        private Queue<IMessageActivity> responses = new Queue<IMessageActivity>();
        private IMessageActivity toBot;


        [TestMethod]
        public async Task TestOffsetY10()
        {
            await TestLoginY5(); 
            await FromUser("offset");
            Assert.IsTrue(await FromBot("offset limit"));
            await FromUser("5");
            Assert.IsTrue(await FromBot("offset limitParam"));
            await FromUser("5");
            Assert.IsTrue(await FromBot("offset offsetParam"));
            await FromUser("5");
            Assert.IsTrue(await FromBot("Is this your selection?"));
            await FromUser("yes");
        }

        [TestMethod]
        public async Task TestResponsepParamY11()
        {

            await TestLoginY5();
            await FromUser("responseparam");
            Assert.IsTrue(await FromBot("responseparam"));
            await FromUser("5");
            Assert.IsTrue(await FromBot("responseparam queryparam"));
            await FromUser("5");
            Assert.IsTrue(await FromBot("responseparam scrollrequestendpoint"));
            await FromUser("5");
            Assert.IsTrue(await FromBot("responseparam scrollrequestmethod"));
            await FromUser("5");
            Assert.IsTrue(await FromBot("responseparam scrollrequestparams"));
            await FromUser("5");
            Assert.IsTrue(await FromBot("Is this your selection?"));
            await FromUser("yes");
        }

        [TestMethod]
        public async Task TestLoginY5()
        {
            await TestApiNameDocY4();
            await FromUser("login");
            Assert.IsTrue(await FromBot("login endpoint"));
            await FromUser("foo login endpoint");
            Assert.IsTrue(await FromBot("username"));
            //Y6
            await FromUser("Foo User");
            Assert.IsTrue(await FromBot("password"));
            await FromUser("Password654");
            Assert.IsTrue(await FromBot("Is this your selection?"));
            await FromUser("yes");
            Assert.IsTrue(await FromBot("Pagination Type"));
        }

        [TestMethod]
        public async Task TestApiNameDocY4()
        {
            await RootDialogAccesDocumentClient(); //LVL1
            Assert.IsTrue(await FromBot("Great now we configure json"));
            Assert.IsTrue(await FromBot("API Name"));
            await FromUser("Foo api name");
            Assert.IsTrue(await FromBot("API Documentation"));
            await FromUser("Api documentation foo");
            Assert.IsTrue(await FromBot("API baseUrl"));
            await FromUser("http://www.foo.com/");
            Assert.IsTrue(await  FromBot("Is this your selection?"));
            await FromUser("yes");
            Assert.IsTrue(await FromBot("Auth Type"));
        }

        [TestMethod]
        public async Task TestBasicY6()
        {
            await TestApiNameDocY4();
            await FromUser("basic");
            Assert.IsTrue(await FromBot("username"));
            await FromUser("Foo User");
            Assert.IsTrue(await FromBot("password"));
            await FromUser("Password654");
            Assert.IsTrue(await FromBot("Is this your selection?"));
            await FromUser("yes");
            Assert.IsTrue(await FromBot("Pagination Type"));
        }

        [TestMethod]
        public async Task TestQueryY7()
        {
            await TestApiNameDocY4();
            await FromUser("query");
            Assert.IsTrue(await FromBot("apiKey"));
            await FromUser("test api key 6+5sa4dsaAAD*+sad441");
            Assert.IsTrue(await FromBot("Is this your selection?"));
            await FromUser("yes");
            Assert.IsTrue(await FromBot("Pagination Type"));
        }

        [TestMethod]
        public async Task TestOAuth10Y8()
        {
            await TestApiNameDocY4();
            await FromUser("oauth10");
            Assert.IsTrue(await FromBot("data"));
            await FromUser("some data");
            Assert.IsTrue(await FromBot("appKey"));
            await FromUser("test api key 6+5sa4dsaAAD*+sad441");
            Assert.IsTrue(await FromBot("appSecret"));
            await FromUser("6+5sa4dsaAAD*+sad441");
            Assert.IsTrue(await FromBot("Is this your selection?"));
            await FromUser("yes");
            Assert.IsTrue(await FromBot("Pagination Type"));
        }

        [TestMethod]
        public async Task TestOAuth20Y9()
        {
            await TestApiNameDocY4();
            await FromUser("oauth20");
            Assert.IsTrue(await FromBot("TBD"));
            //Y6
            await FromUser("???");
            Assert.IsTrue(await FromBot("Is this your selection"));
            await FromUser("yes");
            Assert.IsTrue(await FromBot("Pagination Type"));
        }

        public async Task FromUser(string text)
        {
            Debug.WriteLine("User:" + text);
            toBot.Text = text;
            await GetResponses();
        }

        private async Task GetResponses()
        {
            var responsese = await GetResponse(container, MakeRoot, toBot);
            foreach (var messageActivity in responsese)
                responses.Enqueue(messageActivity);
        }

        public async Task<bool> FromBot(string text)
        {
            var msg = responses.Dequeue();
            if (msg.Attachments.Count > 0)
            {
                var response = ((HeroCard)msg.Attachments[0].Content).Text;
                Debug.WriteLine("Bot:" + response + " (hero)");
                return response.ToLower().Contains(text.ToLower());
            }
            Debug.WriteLine("Bot:" + msg.Text);
            return msg.Text.ToLower().Contains(text.ToLower());
        }


        [TestMethod]
        public async Task RootDialogAccesDocumentClient()
        {
            InitDialog(out MakeRoot, out container);
            await GetResponses();
            //Y0
            Assert.IsTrue(await FromBot("Hello"));
            Assert.IsTrue(await FromBot("Your name"));
            await FromUser("David");
            Assert.IsTrue(await FromBot("Have access API?"));
            //Y1
            await FromUser("yes");
            Assert.IsTrue(await FromBot("Try to call endpoint"));
            Assert.IsTrue(await FromBot("Do you have doc?"));
            //Y2
            await FromUser("yes");
            Assert.IsTrue(await FromBot("Need some information"));
            Assert.IsTrue(await FromBot("Do you have REST Client?"));
            //Y3
            await FromUser("yes");
            Assert.IsTrue(await FromBot("Did you try to get some data via REST Client?"));
            await FromUser("yes");
        }

        [TestMethod]
        public async Task RootDialogNoDocum()
        {
           
            InitDialog(out MakeRoot, out container);
            await GetResponses();

            //Y0
            Assert.IsTrue(await FromBot("Hello"));
            Assert.IsTrue(await FromBot("Your name"));
            await FromUser("David");
            Assert.IsTrue(await FromBot("Have access API?"));
            //Y1
            await FromUser("no");
            Assert.IsTrue(await FromBot("Try to get credentials"));
        }

        [TestMethod]
        public async Task RootDialogNoClient()
        {
             InitDialog(out MakeRoot, out container);
            await GetResponses();
            //Y0
            Assert.IsTrue(await FromBot("Hello"));
            Assert.IsTrue(await FromBot("Your name"));
            await FromUser("David");
            Assert.IsTrue(await FromBot("Have access API?"));
            //Y1
            await FromUser("yes");
            Assert.IsTrue(await FromBot("Try to call endpoint"));
            Assert.IsTrue(await FromBot("Do you have doc?"));
            //N2
            await FromUser("no");
            Assert.IsTrue(await FromBot("You should ask someone"));
        }

        private void InitDialog(out Func<IDialog<object>> MakeRoot, out IContainer container)
        {
            var echoDialog = new RootDialog().BuildChain();
            toBot = MakeTestMessage();
            toBot.From.Id = Guid.NewGuid().ToString();
            toBot.Text = "ConversationUpdate";
            MakeRoot = () => echoDialog;
            new FiberTestBase.ResolveMoqAssembly(echoDialog);
            container = Build(Options.MockConnectorFactory | Options.ScopedQueue, echoDialog);

        }

        private async Task<Queue<IMessageActivity>> GetResponse(IContainer container, Func<IDialog<object>> makeRoot,
            IMessageActivity toBot)
        {
            using (var scope = DialogModule.BeginLifetimeScope(container, toBot))
            {
                DialogModule_MakeRoot.Register(scope, makeRoot);
                //  var token = CancellationToken.None;
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