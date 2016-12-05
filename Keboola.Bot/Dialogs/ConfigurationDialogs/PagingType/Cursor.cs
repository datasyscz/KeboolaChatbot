using System;
using Microsoft.Bot.Builder.FormFlow;

namespace Keboola.Bot.Dialogs.ConfigurationDialogs.PagingType
{
    [Serializable]
    public class CursorForm
    {
        public string IdKey;
        public int Increment;
        public string Param;
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