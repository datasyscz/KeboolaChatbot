using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Keboola.Bot.Facebook
{
    public class SendMessageToUsModel
    {
        public Sender sender { get; set; }
        public Recipient recipient { get; set; }
        public long timestamp { get; set; }
        public Optin optin { get; set; }  ///Custom value from sender
    }

    public class Sender
    {
        public string id { get; set; }
    }

    public class Recipient
    {
        public string id { get; set; }
    }

    public class Optin
    {
        public string _ref { get; set; }
    }
}