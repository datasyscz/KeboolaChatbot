using System;
using System.ComponentModel.DataAnnotations;

namespace Keboola.Shared
{
    /// <summary>
    ///     Chat bot user
    /// </summary>
    [Serializable]
    public class User
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

        public string Name { get; set; }
    }
}