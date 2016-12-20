using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Keboola.Bot.Service;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Keboola.Shared;

namespace Keboola.Bot.Dialogs
{
    [Serializable]
    public class IntroDialog : IDialog<object>
    {
        private static IDatabaseContext _context;
        private static DatabaseService service;
       
        public IntroDialog(IDatabaseContext db)
        {
            _context = db;
        }

        public async Task StartAsync(IDialogContext context)
        {
            service = new DatabaseService(_context);
            var msg = context.MakeMessage();
            var conversation =await service.FindConversationAsync(msg);
            if (conversation.User.KeboolaUser?.Active == true)
                await context.PostAsync("$Hello");
            else
                await context.PostAsync("$Hello inactive");
            context.Done(1);
        }
    }
}