using Microsoft.Bot.Connector;
using System;
using System;
using System.Data.Entity;
using System.Linq;
using DatabaseModel;


namespace KeboolaChatbot
{
    public class DatabaseContext : DbContext
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