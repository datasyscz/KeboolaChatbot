using System.ComponentModel.DataAnnotations.Schema;
using Chatbot.Shared.Models;

namespace Keboola.Bot
{
    [Table("Conversations")]
    public class ConversationExt : IConversation<UserExt, Message>
    {
    }
}