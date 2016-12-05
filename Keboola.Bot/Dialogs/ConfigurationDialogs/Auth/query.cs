using System;
using Keboola.Bot.Dialogs.ConfigurationDialogs.PagingType;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;

namespace Keboola.Bot.Dialogs.ConfigurationDialogs.Auth
{
    [Serializable]
    public class QueryForm
    {
        public string ApiKey;

        public static IForm<QueryForm> BuildForm()
        {
            return new FormBuilder<QueryForm>()
                .Field(nameof(ApiKey), "apiKey")
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