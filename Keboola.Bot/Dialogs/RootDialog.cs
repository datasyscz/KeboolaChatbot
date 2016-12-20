using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using API;
using Keboola.Bot.Keboola;
using Keboola.Bot.Service;
using Keboola.Shared;
using Microsoft.Bot.Builder.Dialogs;

namespace Keboola.Bot.Dialogs
{
    [Serializable]
    public class RootDialog
    {
        public static WitAI WitAI;
        public static KeboolaClient KeboolaClient;
        IDatabaseContext _dbContext = new DatabaseContext();

        public IDialog<object> BuildChain()
        {
            return new IntroDialog(_dbContext)
                .WaitToBot()
                .WaitToBot()
                .WaitToBot()
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