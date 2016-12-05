using System.Collections.Generic;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;
using Keboola.Bot.Facebook;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Connector;

namespace Keboola.Bot
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
        protected readonly IDatabaseContext _db;
        private readonly IBotToUser _inner;

        public BotToUserDatabaseWriter(IBotToUser inner, IDatabaseContext db)
        {
            _db = db;
            SetField.NotNull(out _inner, nameof(inner), inner);
        }

        public IMessageActivity MakeMessage()
        {
            return _inner.MakeMessage();
        }

        public virtual async Task PostAsync(IMessageActivity message,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            await _inner.PostAsync(message, cancellationToken);
            // loging outgoing message
            ConversationLogger logger = new ConversationLogger(_db);
            await logger.LogOutgoingMessage(message, cancellationToken);
        }
    }

    public class BotToUserDbTranslate : BotToUserDatabaseWriter
    {
        public BotToUserDbTranslate(IBotToUser inner, IDatabaseContext db) : base(inner, db)
        {
        }

        public override async Task PostAsync(IMessageActivity message,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            string intentAnswer;
            if (message.Text != string.Empty)
                message.Text = await GetIntent(message.Text, cancellationToken);
            else if (message.Attachments.Count > 0 && message.Attachments[0].Content is HeroCard)
            {
                var heroCard = (HeroCard) message.Attachments[0].Content;
                heroCard.Text = await GetIntent(heroCard.Text, cancellationToken);
            }

            await base.PostAsync(message, cancellationToken);
        }

        private async Task<string> GetIntent(string IntentName, CancellationToken cancellationToken)
        {
            var intent = await _db.IntentAnswer.FirstOrDefaultAsync(a => a.Name == IntentName, cancellationToken);
            if (intent == null)
                return IntentName;
            return intent.Answer;
        }
    }
}