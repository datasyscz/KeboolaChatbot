using System.Collections.Generic;
using Microsoft.Bot.Connector;

namespace Keboola.Bot.Dialogs
{
    public static class HeroCards
    {
        public static IMessageActivity YesNoMessageWithLink(Activity message, string text, string link)
        {
            var replyMsg = message.CreateReply();
            replyMsg.Type = "message";
            replyMsg.Attachments = new List<Attachment>();
            var cardImages = new List<CardImage>();
            cardImages.Add(new CardImage("https://<ImageUrl1>"));
            cardImages.Add(new CardImage("https://<ImageUrl2>"));
            var cardButtons = new List<CardAction>();
            var yesButton = new CardAction
            {
                Value = link,
                Type = "openUrl",
                Title = "Yes"
            };
            var noButton = new CardAction
            {
                Type = "imBack",
                Title = "No",
                Value = "No"
            };

            cardButtons.Add(yesButton);
            cardButtons.Add(noButton);
            var plCard = new HeroCard
            {
                Text = text,
                Buttons = cardButtons
            };
            var plAttachment = plCard.ToAttachment();
            replyMsg.Attachments.Add(plAttachment);
            return replyMsg;
        }
    }
}