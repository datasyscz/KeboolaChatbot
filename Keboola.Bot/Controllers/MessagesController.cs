﻿using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Http;
using API;
using Autofac;
using Keboola.Bot.Dialogs;
using Keboola.Bot.Service;
using log4net;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json.Linq;

namespace Keboola.Bot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        private readonly IDatabaseContext _db;
        private readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DatabaseService service;

        public MessagesController()
        {
            //Register own IBotToUser for messages loging

            _db = new DatabaseContext();

            service = new DatabaseService(_db);
            var builder = new ContainerBuilder();
            builder.RegisterType<BotToUserLogger>()
                .AsSelf()
                .InstancePerLifetimeScope();
            builder.Register(c => new BotToUserDbTranslate(c.Resolve<BotToUserLogger>(), _db))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            builder.Update(Conversation.Container);
            RootDialog.WitAI = new WitAI(WebConfigurationManager.AppSettings["WitAIToken"],
                WebConfigurationManager.AppSettings["WitAIAccept"]);
            DatabaseService.TokenExpiration =
                new TimeSpan(int.Parse(WebConfigurationManager.AppSettings["TokenExpirationDays"]));
        }

        /// <summary>
        ///     POST: api/Messages
        ///     Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody] Activity activity)
        {
            if (activity.Type == ActivityTypes.Message || activity.Type == ActivityTypes.ContactRelationUpdate ||
                activity.Type == ActivityTypes.ConversationUpdate)
            {
                //Default message
                Activity isTyping = null;
                var connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                isTyping = activity.CreateReply();
                isTyping.Type = ActivityTypes.Typing;
                await connector.Conversations.ReplyToActivityAsync(isTyping);


                if (activity.From.Id != WebConfigurationManager.AppSettings["BotId"])
                    //Ignor initial message from direct line service
                    if (activity.ChannelId.ToLower() != "facebook" ||
                        activity.Type != ActivityTypes.ConversationUpdate)
                    {
                        ConversationExt conversation = null;
                        //Dont log welcome message
                        if (
                            !(activity.ChannelId.ToLower() == "directline" &&
                              (activity.Type == ActivityTypes.ConversationUpdate ||
                               activity.Text == "ConversationStart")))
                            conversation = await LogMessage(activity);


                        //Load user context data
                        var stateClient = activity.GetStateClient();
                        var userData = await stateClient.BotState.GetUserDataAsync(activity.ChannelId, activity.From.Id);

                        //handle predefined commands
                        var command = CommandHandler.Handle(activity);
                        if (command == CommandHandler.CommandType.Reset || activity.Action?.ToLower() == "remove")
                        {
                            await Reset(activity, userData, stateClient);
                        }
                        else if (command == CommandHandler.CommandType.Help)
                        {
                            var helpActivity = activity.CreateReply(await service.GetIntentAsync("Help"));
                            await connector.Conversations.ReplyToActivityAsync(helpActivity);
                            return Request.CreateResponse(HttpStatusCode.OK);
                        }

                        if (conversation != null && conversation.User.IsActivated()) //Stop conversation if finish
                        {
                            try
                            {
                                await
                                    Conversation.SendAsync(activity,
                                        new RootDialog(_db).BuildChain);
                            }
                            catch (Exception ex)
                            {
                                //Reset conversation if exception
                                await Reset(activity, userData, stateClient);
                                await
                                    Conversation.SendAsync(activity,
                                        new RootDialog(_db).BuildChain);
                                logger.Error(ex);
                                Debug.Fail(ex.Message);
                            }
                        }
                        else
                        {
                            //Default message
                            var yesNoButton = HeroCards.YesNoMessageWithLink(activity,
                                await service.GetIntentAsync("Hello inactive"),
                                "https://connection.keboola.com/admin/account/chatbot");
                            await connector.Conversations.ReplyToActivityAsync((Activity) yesNoButton);
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

        private async Task<ConversationExt> LogMessage(Activity activity)
        {
            //Log incoming message
            var conLogger = new ConversationLogger(_db);
            var conversationLog = await conLogger.AddOrUpdateConversationAsync(activity);

            if (activity.ChannelData != null)
            {
                //Add token from keboola
                var obj = (JObject) activity.ChannelData;
                if (obj["optin"] != null)
                    if (conversationLog.User.KeboolaUser == null)
                        conversationLog.User.KeboolaUser = new KeboolaUser
                        {
                            KeboolaId = int.Parse(obj["optin"]["ref"].ToString()),
                            Active = true
                        };
                    else
                        conversationLog.User.KeboolaUser.Active = true;
            }

            conversationLog.AddMessage(activity, true);
            await _db.SaveChangesAsync();
            return conversationLog;
        }

        private async Task Reset(Activity activity, BotData userData, StateClient stateClient)
        {
            userData?.SetProperty("Finish", false);
            activity.Text = "/deleteprofile";
            await stateClient.BotState.SetUserDataAsync(activity.ChannelId, activity.From.Id, userData);
            await Conversation.SendAsync(activity, new RootDialog(_db).BuildChain);
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