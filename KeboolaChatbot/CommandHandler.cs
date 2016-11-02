using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Connector;

namespace KeboolaChatbot
{
    public class CommandHandler
    {
        public enum CommandType
        {
            Reset,
            None
        }

        public static CommandType Handle(Activity activity, BotData userData = null, BotData conversationData = null)
        {
            if (activity.Text == "//reset"|| activity.Text == "reset" || activity.Text == "restart")
            {
                return CommandType.Reset;
            }
            return CommandType.None;
        }
    }
}