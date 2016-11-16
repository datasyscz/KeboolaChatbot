using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
                .Build();
        }

        public static IDialog<QueryForm> RootDialog()
        {
            return Chain.From(() => FormDialog.FromForm(QueryForm.BuildForm));
        }
    }
}