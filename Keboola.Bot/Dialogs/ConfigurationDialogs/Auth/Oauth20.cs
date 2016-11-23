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
    public class Oauth20Form
    {
     //   public string TBD;
        public static IForm<Oauth10Form> BuildForm()
        {
            return new FormBuilder<Oauth10Form>()
                .Message("...")
                .Confirm("Is this your selection?\n{*}")
                .Build();
        }
    }
}