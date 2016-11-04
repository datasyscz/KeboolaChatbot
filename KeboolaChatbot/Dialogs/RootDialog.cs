using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.FormFlow.Advanced;

namespace KeboolaChatbot.Dialogs
{
    [Serializable]
    public class RootDialog
    {
        public IDialog<object> BuildChain()
        {
            return Chain.Return("Hello, I'm RC bot, Let's jump to some brief dialogue")
                .PostToUser()
                 .WaitToBot()
                .Select(a=> "What's your name")
                .PostToUser()
                .WaitToBot()
                .ContinueWith<object, object>(async (ctx, res) =>
                {
                    await res;
                    return Chain.From(
                        () => new PromptDialog.PromptConfirm("Do you have access to REST API?", "Don't understand. Do you have access to REST API?", 20))
                        .ContinueWith<bool, object>(async (ctx2, res2) =>
                        {
                            if (await res2)
                            {
                                //Y1
                                return Chain.Return("That is great! Try to call some endpoint via REST client!")
                                    .PostToUser()
                                    .ContinueWith<object, object>(async (ctx3, res3) =>
                                    {
                                        await res3;
                                        return Documentation();
                                    });
                            }
                            else
                            {
                                //N1
                                ctx2.UserData.SetValue("Finish", true);
                                return Chain.Return("Try to get your own credentials, we'll need it!").PostToUser().WaitToBot();
                            }
                        });

                })
                .PostToUser();
        }

        private static IDialog<object> Documentation()
        {
            return Chain.From(
                () => new PromptDialog.PromptConfirm("Do you have API documentation?",
                    "Don't understand. Do you have API documentation?", 20)).ContinueWith<bool, object>(
                        async (ctx, res) =>
                        {
                            if (await res)
                            {
                                //Y2
                                return Chain.Return("That is great! We'll need to find out some information.")
                                    .PostToUser()
                                    .ContinueWith<object, object>(async (ctx2, res2) =>
                                    {
                                        await res2;
                                        return DesktopClient();
                                    });
                            }
                            else
                            {
                                //N2
                                ctx.UserData.SetValue("Finish", true);
                                return Chain.Return("You should ask someone - we need to have it!").PostToUser().WaitToBot();
                            }
                        });
        }

        private static IDialog<object> DesktopClient()
        {
            return Chain.From(
                () => new PromptDialog.PromptConfirm("Do you have Desktop REST client?",
                    "Don't understand. Do you have Desktop REST client?", 20)).ContinueWith<bool, object>(
                        async (ctx, res) =>
                        {
                            if (await res)
                            {
                                //Y3
                                ctx.UserData.SetValue("Finish", true);
                                return Chain.Return("Did you try to get some data via REST Client?").PostToUser().WaitToBot();
                            }
                            else
                            {
                                //N3
                                ctx.UserData.SetValue("Finish", true);
                                return Chain.Return("Try to use Postman and \"touch\" API on your own").PostToUser().WaitToBot();
                            }
                        });
        }
    }
}