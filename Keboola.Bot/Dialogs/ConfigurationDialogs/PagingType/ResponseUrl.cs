using System;
using Microsoft.Bot.Builder.FormFlow;

namespace Keboola.Bot.Dialogs.ConfigurationDialogs.PagingType
{
    [Serializable]
    public class ResponseUrl
    {
        public string IncludeParams;
        public string UrlKey;

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