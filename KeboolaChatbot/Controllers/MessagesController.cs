using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Autofac;
using KeboolaChatbot.Dialogs;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;

namespace KeboolaChatbot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        public MessagesController()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<BotToUserLogger>()
                .AsSelf()
                .InstancePerLifetimeScope();
            builder.Register(c => new BotToUserDatabaseWriter(c.Resolve<BotToUserLogger>()))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            builder.Update(Conversation.Container);
        }

        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody] Activity activity)
        {
            if (activity.Type == ActivityTypes.Message || activity.Type == ActivityTypes.ContactRelationUpdate ||
                activity.Type == ActivityTypes.ConversationUpdate)
            {
                var stateClient = activity.GetStateClient();
                var userData = await stateClient.BotState.GetUserDataAsync(activity.ChannelId, activity.From.Id);
                bool finish = userData?.GetProperty<bool>("Finish") ?? false;

                CommandHandler.CommandType command = CommandHandler.Handle(activity);
                if (command == CommandHandler.CommandType.Reset)
                {
                    await Reset(activity, userData, stateClient);
                }

                if (!finish)
                    try
                    {
                        await Conversation.SendAsync(activity, new RootDialog().BuildChain);
                    }
                    catch (Exception ex)
                    {
                        await Reset(activity, userData, stateClient);
                        await Conversation.SendAsync(activity, new RootDialog().BuildChain);
                    }
                else
                {
                    Activity reply = null;
                    var connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                    reply = activity.CreateReply("Type \"reset\" if you want to restart conversation");
                    await connector.Conversations.ReplyToActivityAsync(reply);
                }

            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private static async Task Reset(Activity activity, BotData userData, StateClient stateClient)
        {
            bool finish;
            finish = false;
            userData?.SetProperty("Finish", false);
            activity.Text = "/deleteprofile";
            await stateClient.BotState.SetUserDataAsync(activity.ChannelId, activity.From.Id, userData);
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}