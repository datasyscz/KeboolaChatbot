using System.Threading;
using System.Threading.Tasks;
using DatabaseModel;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Connector;

namespace KeboolaChatbot
{
    public class BotToUserLogger : IBotToUser
    {
        private readonly IConnectorClient _client;
        private readonly IMessageActivity _toBot;

        public BotToUserLogger(IMessageActivity toBot, IConnectorClient client)
        {
            SetField.NotNull(out _toBot, nameof(toBot), toBot);
            SetField.NotNull(out _client, nameof(client), client);
        }

        public IMessageActivity MakeMessage()
        {
            var toBotActivity = (Activity) _toBot;
            return toBotActivity.CreateReply();
        }

        public async Task PostAsync(IMessageActivity message,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            await _client.Conversations.ReplyToActivityAsync((Activity) message, cancellationToken);
        }
    }

    public class BotToUserDatabaseWriter : IBotToUser
    {
        private readonly IBotToUser _inner;

        public BotToUserDatabaseWriter(IBotToUser inner)
        {
            SetField.NotNull(out _inner, nameof(inner), inner);
        }

        public IMessageActivity MakeMessage()
        {
            return _inner.MakeMessage();
        }

        public async Task PostAsync(IMessageActivity message,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                //Log outgoing message
                var conversation = await db.Conversation.FindByActivityAsync(message);
                conversation.AddMessage(message, false);
                await db.SaveChangesAsync(cancellationToken);
            }
            await _inner.PostAsync(message, cancellationToken);
        }
    }
}