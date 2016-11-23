using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.FormFlow;

namespace Keboola.Bot.Dialogs.ConfigurationDialogs.PagingType
{
    [Serializable]
    public class ResponseUrl
    {
        public string UrlKey;
        public string IncludeParams;

        public static IForm<ResponseUrl> BuildForm()
        {
            return new FormBuilder<ResponseUrl>()
                .Field(nameof(UrlKey), "responseurl urlKey(nextPage)")
                .Field(nameof(IncludeParams), "responseurl includeparam")
                .Confirm("Is this your selection?\n{*}")
                .Build();
        }
    }
}