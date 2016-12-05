using System;
using Microsoft.Bot.Builder.FormFlow;

namespace Keboola.Bot.Dialogs.ConfigurationDialogs.PagingType
{
    [Serializable]
    public class PageNum
    {
        public int Limit;
        public int LimitParam;
        public int PageParam;

        public static IForm<PageNum> BuildForm()
        {
            return new FormBuilder<PageNum>()
                .Field(nameof(PageParam), "pagenum pageparam")
                .Field(nameof(Limit), "pagenum limit")
                .Field(nameof(LimitParam), "pagenum limitparam")
                .Confirm("Is this your selection?\n{*}")
                .Build();
        }
    }
}