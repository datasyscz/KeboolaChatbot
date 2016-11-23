using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Keboola.Bot.Dialogs.ConfigurationDialogs.Auth;
using Microsoft.Bot.Builder.FormFlow;

namespace Keboola.Bot.Dialogs.ConfigurationDialogs.PagingType
{
    [Serializable]
    public class CursorForm
    {
        public string IdKey;
        public string Param;
        public int Increment;
        public bool Reverse;

        public static IForm<CursorForm> BuildForm()
        {
            return new FormBuilder<CursorForm>()
                .Field(nameof(IdKey), "cursor idKey")
                .Field(nameof(Param), "cursor param")
                .Field(nameof(Increment), "cursor incremental")
                .Field(nameof(Reverse), "cursor reverse")
                .Confirm("Is this your selection?\n{*}")
                .Build();
        }
    }
}