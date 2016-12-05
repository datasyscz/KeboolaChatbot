using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.FormFlow.Advanced;
using Keboola.Bot.Dialogs.ConfigurationDialogs.Auth;
using Keboola.Bot.Dialogs.ConfigurationDialogs.PagingType;

#pragma warning disable 649
#pragma warning disable CS1998

namespace Keboola.Bot.Dialogs
{


    public enum AuthTypeoption
    {
        [Describe("basic")]
        [Terms("basic")]
        basic,
        [Terms("query")]
        [Describe("query")]
        query,
        [Terms("login")]
        [Describe("login")]
        login,
        [Terms("oauth10")]
        [Describe("oauth10")]
        oauth10,
        [Terms("oauth20")]
        [Describe("oauth20")]
        oauth20
    }

    [Serializable]
    public class ConfigureForm
    {
        public string Name;
        public string Documentation;
        public string BaseUrl;
        public AuthTypeoption? AuthType;

        public static IForm<ConfigureForm> BuildForm()
        {
            return new FormBuilder<ConfigureForm>()
                .Message("Great now we configure json")
                .Field(nameof(Name), "API Name")
                .Field(nameof(Documentation), "API Documentation")
                .Field(nameof(BaseUrl), "API baseUrl")
                .Confirm("Is this your selection?\n{*}")
                .Field(nameof(AuthType), "Auth Type{||}")

                .Build();
        }

        public static IDialog<object> RootConversation()
        {
            return Chain.From(() => FormDialog.FromForm(ConfigureForm.BuildForm, FormOptions.PromptInStart))
                .Switch(
                    new Case<ConfigureForm, IDialog<object>>(msg => msg.AuthType == AuthTypeoption.login,
                        (ctx, msg) =>
                        {
                            return Chain.From(() => FormDialog.FromForm(LoginForm.BuildForm, FormOptions.PromptInStart));
                        }),
                    new Case<ConfigureForm, IDialog<object>>(msg => msg.AuthType == AuthTypeoption.basic,
                        (ctx, msg) =>
                        {
                            return Chain.From(() => FormDialog.FromForm(BasicForm.BuildForm, FormOptions.PromptInStart));
                        }),
                    new Case<ConfigureForm, IDialog<object>>(msg => msg.AuthType == AuthTypeoption.query,
                        (ctx, msg) =>
                        {
                            return Chain.From(() => FormDialog.FromForm(QueryForm.BuildForm, FormOptions.PromptInStart));
                        }),
                    new Case<ConfigureForm, IDialog<object>>(msg => msg.AuthType == AuthTypeoption.oauth10,
                        (ctx, msg) =>
                        {
                            return
                                Chain.From(() => FormDialog.FromForm(Oauth10Form.BuildForm, FormOptions.PromptInStart));
                        }),
                    new Case<ConfigureForm, IDialog<object>>(msg => msg.AuthType == AuthTypeoption.oauth20,
                        (ctx, msg) =>
                        {
                            return
                                Chain.From(() => FormDialog.FromForm(Oauth20Form.BuildForm, FormOptions.PromptInStart));
                        })
                ).Unwrap()
                .ContinueWith<object, object>(
                    async (ctx, res) =>
                    {
                        return PagingTypeForm.RootConversation();
                    }
                )
                .ContinueWith<object, object>(
                    async (ctx, res) =>
                    {
                        return new EndpointDialog();
                    });

        }
    }
}