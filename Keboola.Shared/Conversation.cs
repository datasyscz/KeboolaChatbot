using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Threading.Tasks;
using Keboola.Shared;
using Microsoft.Bot.Connector;

namespace Keboola.Shared
{
    public class Conversation
    {
        [Key]
        public int ConversationID { get; set; }

        public string Name { get; set; }
        public string FrameworkId { get; set; }
        public virtual List<Message> Messages { get; set; }
        public string Detail { get; set; }
        public string BaseUri { get; set; }
        public virtual User User { get; set; }

        public void AddMessage(IMessageActivity activity, bool messageFromUser)
        {
            var text = string.Empty;
            if (activity.Text != string.Empty) //Simple message
            {
                text = activity.Text;
            }
            else //Advanced message with buttons images etc...
            {
                if (activity.Attachments?.Count > 0 &&
                    activity.Attachments?[0].Content is HeroCard)
                {
                    var card
                        = (HeroCard) activity.Attachments[0].Content;
                    text = card.Text + "{Buttons}";
                }
                else
                {
                    text = "Unknown message type";
                }
            }

            var message = new Message
            {
                Text = text,
                Date = DateTime.Now,
                SendedByUser = messageFromUser
            };

            if (Messages == null)
                Messages = new List<Message>();

            Messages.Add(message);
        }
    }
}