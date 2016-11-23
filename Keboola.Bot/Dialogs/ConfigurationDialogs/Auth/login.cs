using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Keboola.Bot.Dialogs.ConfigurationDialogs.PagingType;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;

namespace Keboola.Bot.Dialogs.ConfigurationDialogs.Auth
{
    [Serializable]
    public class LoginForm
    {
        public string EndPoint;
        public string UserName;
        public string Password;

        public static IForm<LoginForm> BuildForm()
        {
            return new FormBuilder<LoginForm>()
                .Field(nameof(EndPoint), "login endpoint")
                .Field(nameof(UserName), "username")
                .Field(nameof(Password), "password")
                .Confirm("Is this your selection?\n{*}")
                .Build();
        }

        public static IDialog<object> RootDialog()
        {
            return Chain.From(() => FormDialog.FromForm(LoginForm.BuildForm, FormOptions.PromptInStart))
                 .ContinueWith<object, object>(
                    async (ctx, res) =>
                    {
                        await res;
                        return PagingTypeForm.RootConversation();
                    });
        }
    }
}