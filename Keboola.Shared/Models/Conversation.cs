using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.Bot.Connector;

namespace Chatbot.Shared.Models
{
    public class Conversation : IConversation<IUser<Channel>, Message>
    {
    }

    public class IConversation<TUser, TMessage>
        where TUser : IUser<Channel>, new()
        where TMessage : Message, new()
    {
        [Key]
        public int ConversationID { get; set; }

        public string Name { get; set; }
        public string FrameworkId { get; set; }
        public virtual List<TMessage> Messages { get; set; }
        public string Detail { get; set; }
        public string BaseUri { get; set; }
        //  public virtual User User { get; set; }
        public virtual TUser User { get; set; }

        public virtual string DateStr
        {
            get
            {
                var date = Messages.OrderByDescending(a => a.Date).FirstOrDefault().Date;
                return date.ToString("d.M.yyyy hh:mm");
            }
        }

        public virtual string StoryFull
        {
            get
            {
                var result = string.Empty;
                var list = GetChronologic();
                foreach (var msg in list)
                {
                    var type = msg.SendedByUser ? "User" : "Bot";
                    result += $"{type}: {msg.Text} " + Environment.NewLine;
                }
                return result;
            }
        }

        public virtual string StoryShort
        {
            get
            {
                var result = StoryFull;
                return result.Length < 150 ? result : result.Substring(0, 150) + Environment.NewLine + "...";
            }
        }

        public virtual void SendMessage(string text)
        {
            var connector = new ConnectorClient(new Uri(BaseUri));
            var newMessage = Activity.CreateMessageActivity();
            newMessage.Type = ActivityTypes.Message;

            newMessage.From = new ChannelAccount
            {
                Id = User.BotChannel.FrameworkId,
                Name = User.BotChannel.Name
            };

            newMessage.Conversation = new ConversationAccount(true, FrameworkId,
                Name);
            newMessage.Recipient = new ChannelAccount
            {
                Id = User.UserChannel.FrameworkId,
                Name = User.UserChannel.Name
            };

            newMessage.Text = text;
            connector.Conversations.SendToConversation((Activity) newMessage);
        }

        public virtual void AddMessage(IMessageActivity activity, bool messageFromUser)
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

            var message = new TMessage
            {
                Text = text,
                Date = DateTime.Now,
                SendedByUser = messageFromUser
            };

            if (Messages == null)
                Messages = new List<TMessage>();

            Messages.Add(message);
        }

        public virtual List<TMessage> GetChronologic()
        {
            return Messages.OrderBy(a => a.Date).ToList();
        }
    }
}