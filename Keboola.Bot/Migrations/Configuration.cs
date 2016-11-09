using System.Data.Entity.Migrations;

namespace Keboola.Bot.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<DatabaseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DatabaseContext context)
        {
            /* IList<IntentAnswer> answers = new List<IntentAnswer>();
            context.IntentAnswer.Add(new IntentAnswer
            {
                Name = "Hello",
                Answer = "Hello, I'm RC bot, Let's jump to some brief dialogue."
            });
            context.IntentAnswer.Add(new IntentAnswer { Name = "Your name", Answer = "What's your name?" });
            context.IntentAnswer.Add(new IntentAnswer
            {
                Name = "Have access API?",
                Answer = "Do you have access to REST API?"
            });
            context.IntentAnswer.Add(new IntentAnswer
            {
                Name = "Try to call endpoint",
                Answer = "That is great! Try to call some endpoint via REST client!"
            });
            context.IntentAnswer.Add(new IntentAnswer
            {
                Name = "Try to get credentials",
                Answer = "Try to get your own credentials, we'll need it!"
            });
            context.IntentAnswer.Add(new IntentAnswer
            {
                Name = "Do you have doc?",
                Answer = "Do you have API documentation?"
            });
            context.IntentAnswer.Add(new IntentAnswer
            {
                Name = "Need some information",
                Answer = "That is great! We'll need to find out some information."
            });
            context.IntentAnswer.Add(new IntentAnswer
            {
                Name = "You should ask someone",
                Answer = "You should ask someone - we need to have it!"
            });
            context.IntentAnswer.Add(new IntentAnswer
            {
                Name = "Do you have REST Client?",
                Answer = "Do you have Desktop REST client?"
            });
            context.IntentAnswer.Add(new IntentAnswer
            {
                Name = "Try post man",
                Answer = "Try to use Postman and \"touch\" API on your own"
            });
            context.IntentAnswer.Add(new IntentAnswer
            {
                Name = "Did you try to get some data via REST Client?",
                Answer = "Did you try to get some data via REST Client?"
            });*/
            base.Seed(context);
        }
    }
}