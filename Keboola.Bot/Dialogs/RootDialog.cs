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


        public IDialog<object> BuildChain()
        {
            return Chain.Return("sdsa")
                .PostToUser()
                .WaitToBot()
                .Select(a => "Your name")
                .PostToUser()
                .WaitToBot();
        }
    }
}