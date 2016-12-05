using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Keboola.Shared;
using Microsoft.Bot.Connector;

namespace Keboola.Bot
{
    public interface IDatabaseContext : IDisposable
    {
        DbSet<Message> Messages { get; set; }
        DbSet<Conversation> Conversation { get; set; }
        DbSet<User> Customer { get; set; }
        DbSet<Channel> Channel { get; set; }
        DbSet<IntentAnswer> IntentAnswer { get; set; }
        Task<int> SaveChangesAsync();
        Task<Conversation> FindConversation(IMessageActivity activity);
    }

    public class DatabaseContext : DbContext, IDatabaseContext
    {
        public DatabaseContext()
            : base("name=DatabaseContext")
        {
        }

        public DbSet<Message> Messages { get; set; }
        public DbSet<Conversation> Conversation { get; set; }
        public DbSet<User> Customer { get; set; }
        public DbSet<Channel> Channel { get; set; }
        public DbSet<IntentAnswer> IntentAnswer { get; set; }

        public async Task<Conversation> FindConversation(
            IMessageActivity activity)
        {
            return
                await Conversation
                    .FirstOrDefaultAsync(
                        a =>
                            (a.FrameworkId == activity.Conversation.Id)
                            &&
                            (a.BaseUri == activity.ServiceUrl)
                        //Need check service url too, ConversationID is unique only for serviceUrl 
                    );
        }
    }
}