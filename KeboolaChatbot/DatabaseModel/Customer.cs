using System;
using System.ComponentModel.DataAnnotations;

namespace DatabaseModel
{
    /// <summary>
    ///     Chat bot user
    /// </summary>
    [Serializable]
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        public virtual Channel UserChannel { get; set; }
        public virtual Channel BotChannel { get; set; }

        /// <summary>
        ///     Bot framework id
        /// </summary>
        public int ConversationID { get; set; }

        public string BaseUri { get; set; }
    }
}