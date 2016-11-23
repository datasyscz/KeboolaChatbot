using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.FormFlow;

namespace Keboola.Bot.Dialogs.ConfigurationDialogs.PagingType
{
    [Serializable]
    public class PageNum
    {
        public int PageParam;
        public int Limit;
        public int LimitParam;

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