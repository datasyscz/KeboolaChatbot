using System;
using API;
using Microsoft.Bot.Builder.Dialogs;

namespace Keboola.Bot.Dialogs
{
    [Serializable]
    public class RootDialog
    {
        public static WitAI WitAI;

        public IDialog<object> BuildChain()
        {
            return Chain.Return("Hello")
                .PostToUser()
                .WaitToBot()
                .Select(a => "Your name")
                .PostToUser()
                .WaitToBot()
                .ContinueWith<object, object>(async (ctx, res) =>
                {
                    await res;
                    return Chain.From(
                            () =>
                                new PromptDialog.PromptConfirm("Have access API?",
                                    "Don't understand. Do you have access to REST API?", 20))
                        .ContinueWith(async (ctx2, res2) =>
                        {
                            if (await res2)
                                return Chain.Return("Try to call endpoint")
                                    .PostToUser()
                                    .ContinueWith<object, object>(async (ctx3, res3) =>
                                    {
                                        await res3;
                                        return Documentation();
                                    });
                            //N1
                            ctx2.UserData.SetValue("Finish", true);
                            return Chain.Return("Try to get credentials").PostToUser().WaitToBot();
                        });
                })
                .PostToUser();
        }

        private static IDialog<object> Documentation()
        {
            return Chain.From(
                () => new PromptDialog.PromptConfirm("Do you have doc?",
                    "Don't understand. Do you have API documentation?", 20)).ContinueWith(
                async (ctx, res) =>
                {
                    if (await res)
                        return Chain.Return("Need some information")
                            .PostToUser()
                            .ContinueWith<object, object>(async (ctx2, res2) =>
                            {
                                await res2;
                                return DesktopClient();
                            });
                    //N2
                    ctx.UserData.SetValue("Finish", true);
                    return Chain.Return("You should ask someone").PostToUser().WaitToBot();
                });
        }

        private static IDialog<object> DesktopClient()
        {
            return Chain.From(
                () => new PromptDialog.PromptConfirm("Do you have REST Client?",
                    "Don't understand. Do you have Desktop REST client?", 20)).ContinueWith(
                async (ctx, res) =>
                {
                    if (await res)
                        return Chain.From(
                                () =>
                                    new PromptDialog.PromptConfirm("Did you try to get some data via REST Client?",
                                        "Don't understand. Did you try to get some data via REST Client?", 20))
                            .ContinueWith(
                                async (ctx2, res2) =>
                                {
                                    if (await res2)
                                        return ConfigureForm.RootConversation();
                                    return Chain.Return("...").PostToUser().WaitToBot();
                                }
                            );

                    //N3
                    ctx.UserData.SetValue("Finish", true);
                    return Chain.Return("Try post man").PostToUser().WaitToBot();
                });
        }
    }
}