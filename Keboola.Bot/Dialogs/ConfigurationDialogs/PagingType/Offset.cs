using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Keboola.Bot.Dialogs.ConfigurationDialogs.Auth;
using Microsoft.Bot.Builder.FormFlow;

namespace Keboola.Bot.Dialogs.ConfigurationDialogs.PagingType
{
    [Serializable]
    public class Offset
    {
        public int Limit;
        public int LimitParam;
        public int OffsetParam;

        public static IForm<Offset> BuildForm()
        {
            return new FormBuilder<Offset>()
                .Field(nameof(Limit), "offset limit")
                .Field(nameof(LimitParam), "offset limitparam")
                .Field(nameof(OffsetParam), "offset offsetparam")
                .Confirm("Is this your selection?\n{*}")
                .Build();
        }
    }
}