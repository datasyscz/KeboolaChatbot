using System;
using API;
using Keboola.Bot.Keboola;
using Microsoft.Bot.Builder.Dialogs;

namespace Keboola.Bot.Dialogs
{
    [Serializable]
    public class RootDialog
    {
        public static WitAI WitAI;
        public static KeboolaClient KeboolaClient;
        public static IDatabaseContext _dbContext;

        public RootDialog(IDatabaseContext db)
        {
            _dbContext = db;
        }

        public IDialog<object> BuildChain()
        {
            return new IntroDialog(_dbContext)
                .WaitToBot();

            /*  return Chain.Return("$Hello")
                  .PostToUser()
                  .WaitToBot()
                  .Select(a => "Your name")
                  .PostToUser()
                  .WaitToBot();*/
        }
    }
}