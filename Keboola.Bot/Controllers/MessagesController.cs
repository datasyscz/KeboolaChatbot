using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Http;
using API;
using Autofac;
using Keboola.Bot.Dialogs;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;

namespace Keboola.Bot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        private readonly IDatabaseContext _db;

        public MessagesController()
        {
            //Register own IBotToUser for messages loging
            _db = new DatabaseContext();
            var builder = new ContainerBuilder();
            builder.RegisterType<BotToUserLogger>()
                .AsSelf()
                .InstancePerLifetimeScope();
            builder.Register(c => new BotToUserDbTranslate(c.Resolve<BotToUserLogger>(), _db))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            builder.Update(Conversation.Container);
            RootDialog.WitAI = new WitAI(WebConfigurationManager.AppSettings["WitAIToken"], WebConfigurationManager.AppSettings["WitAIAccept"]);
        }

        /// <summary>
        ///     POST: api/Messages
        ///     Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody] Activity activity)
        {
            if (activity.Type == ActivityTypes.Message || activity.Type == ActivityTypes.ContactRelationUpdate ||
                activity.Type == ActivityTypes.ConversationUpdate )
            {
                //Default message
                Activity isTyping = null;
                var connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                isTyping = activity.CreateReply();
                isTyping.Type = ActivityTypes.Typing;
                await connector.Conversations.ReplyToActivityAsync(isTyping);


                if (activity.From.Id != WebConfigurationManager.AppSettings["BotId"]) //Ignor initial message from direct line service
                {
                    //Ignor facebook first message
                    if (activity.ChannelId.ToLower() != "facebook" || activity.Type != ActivityTypes.ConversationUpdate)
                    {
                        //Dont log welcome message
                        if (!(activity.ChannelId.ToLower() == "directline" && (activity.Type == ActivityTypes.ConversationUpdate || activity.Text == "ConversationStart")))
                        {
                            //Log incoming message
                            await LogMessage(activity);
                        }

                        //Load user context data
                        var stateClient = activity.GetStateClient();
                        var userData = await stateClient.BotState.GetUserDataAsync(activity.ChannelId, activity.From.Id);
                        var finish = userData?.GetProperty<bool>("Finish") ?? false;

                        //handle predefined commands
                        var command = CommandHandler.Handle(activity);
                        if (command == CommandHandler.CommandType.Reset || activity.Action?.ToLower() == "remove")
                        {
                            await Reset(activity, userData, stateClient);
                            finish = false;
                        }

                        if (!finish) //Stop conversation if finish
                            try
                            {
                                //Dialog
                             //   await Conversation.SendAsync(activity, ConfigureDialog.RootConversation);
                                 await Conversation.SendAsync(activity, new RootDialog().BuildChain);
                            }
                            catch (Exception ex)
                            {
                                Debug.Fail(ex.Message);
                                await Reset(activity, userData, stateClient);
                                await Conversation.SendAsync(activity, new RootDialog().BuildChain);
                            }
                        else
                        {
                            //Default message
                            Activity reply = null;
                            reply = activity.CreateReply("Type \"reset\" if you want to restart conversation");
                            await connector.Conversations.ReplyToActivityAsync(reply);
                        }
                    }
                }
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private async Task LogMessage(Activity activity)
        {
            
            //Log incoming message
            var logger = new ConversationLogger(_db);
            var conversationLog = await logger.AddOrUpdateConversation(activity);
            conversationLog.AddMessage(activity, true);
            await _db.SaveChangesAsync();
        }

        private static async Task Reset(Activity activity, BotData userData, StateClient stateClient)
        {
            userData?.SetProperty("Finish", false);
            activity.Text = "/deleteprofile";
            await stateClient.BotState.SetUserDataAsync(activity.ChannelId, activity.From.Id, userData);
            await Conversation.SendAsync(activity, new RootDialog().BuildChain);
            activity.Text = "";
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