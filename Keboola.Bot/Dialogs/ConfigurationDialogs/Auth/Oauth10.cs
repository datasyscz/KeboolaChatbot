using System;
using Keboola.Bot.Dialogs.ConfigurationDialogs.PagingType;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;

namespace Keboola.Bot.Dialogs.ConfigurationDialogs.Auth
{
    [Serializable]
    public class Oauth10Form
    {
        public string AppKey;
        public string AppSecret;
        public string Data;

        public static IForm<Oauth10Form> BuildForm()
        {
            return new FormBuilder<Oauth10Form>()
                .Field(nameof(Data), "data")
                .Field(nameof(AppKey), "appKey")
                .Field(nameof(AppSecret), "appSecret")
                .Confirm("Is this your selection?\n{*}")
                .Build();
        }

        public static IDialog<object> RootDialog()
        {
            return Chain.From(() => FormDialog.FromForm(BuildForm, FormOptions.PromptInStart))
                .ContinueWith<object, object>(
                    async (ctx, res) =>
                    {
                        await res;
                        return PagingTypeForm.RootConversation();
                    });
        }
    }
}