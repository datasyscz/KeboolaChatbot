using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Chatbot.Shared.Models;

namespace Keboola.Bot
{
    public interface IDatabaseContext : IDisposable
    {
        DbSet<Message> Messages { get; set; }
        DbSet<ConversationExt> Conversation { get; set; }
        DbSet<UserExt> Customer { get; set; }
        DbSet<Channel> Channel { get; set; }
        DbSet<IntentAnswer> IntentAnswer { get; set; }
        DbSet<KeboolaToken> KeboolaToken { get; set; }
        DbSet<KeboolaUser> KeboolaUser { get; set; }
        Task<int> SaveChangesAsync();
        int SaveChanges();
        void MarkAsModified<T>(T item) where T : class;
    }

    [Serializable]
    public class DatabaseContext : DbContext, IDatabaseContext
    {
        public DatabaseContext()
            : base("name=DatabaseContext")
        {
        }

        public DbSet<Message> Messages { get; set; }
        public DbSet<ConversationExt> Conversation { get; set; }
        public DbSet<UserExt> Customer { get; set; }
        public DbSet<Channel> Channel { get; set; }
        public DbSet<IntentAnswer> IntentAnswer { get; set; }
        public DbSet<KeboolaToken> KeboolaToken { get; set; }
        public DbSet<KeboolaUser> KeboolaUser { get; set; }

        public void MarkAsModified<T>(T item) where T : class
        {
            Entry(item).State = EntityState.Modified;
        }
    }
}