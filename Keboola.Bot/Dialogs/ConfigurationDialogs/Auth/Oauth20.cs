using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;

namespace Keboola.Bot.Dialogs.ConfigurationDialogs.Auth
{
    [Serializable]
    public class Oauth20Form
    {
     //   public string TBD;
        public static IForm<Oauth10Form> BuildForm()
        {
            return new FormBuilder<Oauth10Form>()
                .Message("...")
                .Build();
        }

        public static IDialog<Oauth10Form> RootDialog()
        {
            return Chain.From(() => FormDialog.FromForm(Oauth10Form.BuildForm));
        }
    }
}