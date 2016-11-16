using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;

namespace Keboola.Bot.Dialogs.ConfigurationDialogs.Auth
{
    [Serializable]
    public class BasicForm
    {
        public string UserName;
        public string Password;

        public static IForm<BasicForm> BuildForm()
        {
            return new FormBuilder<BasicForm>()
                .Field(nameof(UserName), "username")
                .Field(nameof(Password), "password")
                .Build();
        }

        public static IDialog<BasicForm> RootDialog()
        {
            return Chain.From(() => FormDialog.FromForm(BasicForm.BuildForm));
        }
    }
}