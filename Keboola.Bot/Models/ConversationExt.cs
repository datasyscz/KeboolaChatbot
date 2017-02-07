using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Keboola.Bot
{
    [Table("Conversation")]
    public class ConversationExt : Chatbot.Shared.Models.IConversation<UserExt,Chatbot.Shared.Models.Message>
    {

    }
}