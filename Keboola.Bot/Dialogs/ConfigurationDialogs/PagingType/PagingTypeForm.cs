using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Keboola.Bot.Dialogs.ConfigurationDialogs.Auth;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;

namespace Keboola.Bot.Dialogs.ConfigurationDialogs.PagingType
{
    public enum PagingType
    {
        [Terms("offset")]
        [Describe("offset")]
        offset,

        [Terms("responseparam", "response.param", "param")]
        [Describe("response.param")]
        responseParam,

        [Terms("response.url", "responseurl", "url")]
        [Describe("response.url")]
        responseUrl,

        [Terms("page.num", "pagenum", "num")]
        [Describe("page.num")]
        pagenum,

        [Terms("cur", "cursor")]
        [Describe("cursor")]
        cursor
    }

    [Serializable]
    public class PagingTypeForm
    {
        public PagingType? Paging;

        public static IForm<PagingTypeForm> BuildForm()
        {
            return new FormBuilder<PagingTypeForm>()
                .Field(nameof(Paging), "Pagination Type{||}")
                .Build();
        }

        public static IDialog<object> RootConversation()
        {
            return Chain.From(() => FormDialog.FromForm(PagingTypeForm.BuildForm, FormOptions.PromptInStart))
                .Switch(
                    new Case<PagingTypeForm, IDialog<object>>(msg => msg.Paging == PagingType.offset,
                        (ctx, msg) =>
                        {
                            return Chain.From(() => FormDialog.FromForm(Offset.BuildForm, FormOptions.PromptInStart));
                        }),
                    new Case<PagingTypeForm, IDialog<object>>(msg => msg.Paging == PagingType.cursor,
                        (ctx, msg) =>
                        {
                            return Chain.From(() => FormDialog.FromForm(CursorForm.BuildForm, FormOptions.PromptInStart));
                        })
                    ,
                    new Case<PagingTypeForm, IDialog<object>>(msg => msg.Paging == PagingType.pagenum,
                        (ctx, msg) =>
                        {
                            return Chain.From(() => FormDialog.FromForm(PageNum.BuildForm, FormOptions.PromptInStart));
                        })
                    ,
                    new Case<PagingTypeForm, IDialog<object>>(msg => msg.Paging == PagingType.responseParam,
                        (ctx, msg) =>
                        {
                            return Chain.From(() => FormDialog.FromForm(ResponseParam.BuildForm, FormOptions.PromptInStart));
                        })
                    ,
                    new Case<PagingTypeForm, IDialog<object>>(msg => msg.Paging == PagingType.responseUrl,
                        (ctx, msg) =>
                        {
                            return Chain.From(() => FormDialog.FromForm(ResponseUrl.BuildForm, FormOptions.PromptInStart));
                        })
                ).Unwrap();
        }
    }
}