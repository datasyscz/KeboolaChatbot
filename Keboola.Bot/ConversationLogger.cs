using System;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;
using Keboola.Bot.Service;
using Keboola.Shared;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;

namespace Keboola.Bot
{
    public interface IConversationLogger
    {
    }

    public class ConversationLogger
    {
        private readonly IDatabaseContext _db;
        private DatabaseService service;

        public ConversationLogger(IDatabaseContext db)
        {
            _db = db;
            service  =new DatabaseService(_db);
        }

        /// <summary>
        ///     Create or update conversation log in DbContext
        /// </summary>
        /// <param name="activity">Incoming message</param>
        /// <param name="db"></param>
        /// <returns></returns>
        public async Task<Conversation> AddOrUpdateConversation(IMessageActivity activity)
        {
            //Find conversation
            var conversation = await service.FindConversationAsync(activity);
            if (conversation == null)
            {
                conversation = new Conversation
                {
                    User = new User(),
                    Name = activity.Conversation.Name,
                    FrameworkId = activity.Conversation.Id,
                    BaseUri = activity.ServiceUrl
                };
                //log botchannel
                var botChannel =
                    await
                        _db.Channel.FirstOrDefaultAsync(
                            a => (a.FrameworkId == activity.Recipient.Id) && (a.Name == activity.Recipient.Name));
                if (botChannel == null)
                    conversation.User.BotChannel = new Channel
                    {
                        FrameworkId = activity.Recipient.Id,
                        Name = activity.Recipient.Name
                    };
                else
                    conversation.User.BotChannel = botChannel;

            
            //    var SendToMessageToUsModel = JsonConvert.DeserializeObject<Facebook.SendMessageToUsModel>(activity.ChannelData.);

            Type tt  =
                activity.ChannelData.GetType();
                //if (activity.ChannelData.GetType() == typeof())
             //   {
                    
              //  }
 


                //log userChannel
                var userChannel =
                    await
                        _db.Channel.FirstOrDefaultAsync(
                            a => (a.FrameworkId == activity.From.Id) && (a.Name == activity.From.Name));
                if (userChannel == null)
                    conversation.User.UserChannel = new Channel
                    {
                        FrameworkId = activity.From.Id,
                        Name = activity.From.Name
                    };
                else
                    conversation.User.UserChannel = userChannel;
                _db.Conversation.Add(conversation);
            }

            conversation.User.ConversationID = conversation.ConversationID;
            return conversation;
        }

        public async Task LogOutgoingMessage(IMessageActivity message, CancellationToken cancellationToken)
        {
            var conversation = await service.FindConversationAsync(message);

            if (conversation != null)
            {
                conversation.AddMessage(message, false);
                await _db.SaveChangesAsync();
            }
        }
    }
}