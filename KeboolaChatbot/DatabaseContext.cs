using Microsoft.Bot.Connector;
using System;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DatabaseModel;


namespace KeboolaChatbot
{
    public interface IDatabaseContext : IDisposable
    {
        DbSet<Message> Message { get; set; }
        DbSet<Conversation> Conversation { get; set; }
        DbSet<Customer> Customer { get; set; }
        DbSet<Channel> Channel { get; set; }
        DbSet<IntentAnswer> IntentAnswer { get; set; }
        Task<int> SaveChangesAsync();
    }

    public class DatabaseContext : DbContext, IDatabaseContext
    {
        public DbSet<Message> Message { get; set; }
        public DbSet<Conversation> Conversation { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Channel> Channel { get; set; }
        public DbSet<IntentAnswer> IntentAnswer { get; set; }

        public DatabaseContext()
            : base("name=DatabaseContext")
        {
         
        }
    }
}