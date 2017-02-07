using System.ComponentModel.DataAnnotations.Schema;
using Chatbot.Shared.Models;

namespace Keboola.Bot
{
    [Table("Conversation")]
    public class ConversationExt : IConversation<UserExt, Message>
    {
    }
}