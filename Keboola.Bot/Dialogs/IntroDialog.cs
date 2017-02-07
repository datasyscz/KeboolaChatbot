using System;
using System.Threading.Tasks;
using Keboola.Bot.Service;
using Microsoft.Bot.Builder.Dialogs;

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
            var conversation = await service.FindConversationAsync(msg);
            if (conversation.User.KeboolaUser?.Active == true)
                await context.PostAsync("$Hello");
            else
                await context.PostAsync("$Hello inactive");
            context.Done(1);
        }
    }
}