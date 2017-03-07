using Microsoft.Bot.Connector;

namespace Keboola.Bot
{
    /// <summary>
    ///     Handle user commands
    /// </summary>
    public class CommandHandler
    {
        public enum CommandType
        {
            Reset,
            Help,
            None
        }

        public static CommandType Handle(Activity activity, BotData userData = null, BotData conversationData = null)
        {
            if (activity.Text == "//reset" || activity.Text == "reset" || activity.Text == "restart")
                return CommandType.Reset;
            if (activity.Text?.ToLower() == "//help" || activity.Text?.ToLower() == "help")
                return CommandType.Help;
            return CommandType.None;
        }
    }
}