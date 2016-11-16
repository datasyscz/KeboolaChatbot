using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.FormFlow.Advanced;
using Keboola.Bot.Dialogs.ConfigurationDialogs.Auth;
#pragma warning disable 649
#pragma warning disable CS1998

namespace Keboola.Bot.Dialogs
{


    public enum AuthTypeoption
    {
        basic,
        query,
        login,
        oauth10,
        oauth20
    }

    [Serializable]
    public class ConfigureForm
    {
        public string Name;
        public string Documentation;
        public string BaseUrl;
        public AuthTypeoption AuthType;

        public static IForm<ConfigureForm> BuildForm()
        {
            return new FormBuilder<ConfigureForm>()
                .Message("Great now we configure json")
                .Field(nameof(Name), "API Name")
                .Field(nameof(Documentation), "API Documentation")
                .Field(nameof(BaseUrl), "API baseUrl")
                // .Field(nameof(Documentation))
                .Field(nameof(AuthType), "Auth Type{||}")
                .Build();
        }

        public static IDialog<object> RootConversation()
        {
            return Chain.From(() => FormDialog.FromForm(ConfigureForm.BuildForm))
                .Switch(
                    new Case<ConfigureForm, IDialog<object>>(msg => msg.AuthType == AuthTypeoption.login,
                        (ctx, msg) =>
                        {
                            return LoginForm.RootDialog();
                        }),
                     new Case<ConfigureForm, IDialog<object>>(msg => msg.AuthType == AuthTypeoption.basic,
                        (ctx, msg) =>
                        {
                            return BasicForm.RootDialog();
                        }),
                      new Case<ConfigureForm, IDialog<object>>(msg => msg.AuthType == AuthTypeoption.query,
                        (ctx, msg) =>
                        {
                            return QueryForm.RootDialog();
                        }),
                       new Case<ConfigureForm, IDialog<object>>(msg => msg.AuthType == AuthTypeoption.oauth10,
                        (ctx, msg) =>
                        {
                            return Oauth10Form.RootDialog();
                        }),
                        new Case<ConfigureForm, IDialog<object>>(msg => msg.AuthType == AuthTypeoption.oauth20,
                        (ctx, msg) =>
                        {
                            return Oauth20Form.RootDialog();
                        })
                        ).Unwrap().PostToUser();
        }

       
    }
}