using Chatbot.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Keboola.Bot
{
    public class User : Chatbot.Shared.Models.User
    {
        public virtual KeboolaUser KeboolaUser { get; set; }

        public bool IsActivated()
        {
            return KeboolaUser?.Active == true;
        }

        public virtual string IsActivatedStr
        {
            get { return IsActivated() ? "Yes" : "No"; }
        }
    }
}