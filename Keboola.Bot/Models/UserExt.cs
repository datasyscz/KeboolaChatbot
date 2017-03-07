using System.ComponentModel.DataAnnotations.Schema;
using Chatbot.Shared.Models;

namespace Keboola.Bot
{
    [Table("Users")]
    public class UserExt : User
    {
        public virtual KeboolaUser KeboolaUser { get; set; }

        public virtual string IsActivatedStr
        {
            get { return IsActivated() ? "Yes" : "No"; }
        }

        public bool IsActivated()
        {
            return KeboolaUser?.Active == true;
        }
    }
}