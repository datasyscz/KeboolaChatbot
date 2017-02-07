using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;
using Microsoft.Bot.Builder.Dialogs;
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
            List<CardImage> cardImages = new List<CardImage>();
            cardImages.Add(new CardImage(url: "https://<ImageUrl1>"));
            cardImages.Add(new CardImage(url: "https://<ImageUrl2>"));
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction yesButton = new CardAction()
            {
                Value = link,
                Type = "openUrl",
                Title = "Yes"
            };
            CardAction noButton = new CardAction()
            {
                Type = "imBack", 
                Title = "No",
                Value = "No"
            };

            cardButtons.Add(yesButton);
            cardButtons.Add(noButton);
            HeroCard plCard = new HeroCard()
            {
                Text = text,
                Buttons = cardButtons
            };
            Attachment plAttachment = plCard.ToAttachment();
            replyMsg.Attachments.Add(plAttachment);
            return replyMsg;
        }
    }
}