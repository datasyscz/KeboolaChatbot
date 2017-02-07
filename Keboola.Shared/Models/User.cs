using System;
using System.ComponentModel.DataAnnotations;

namespace Chatbot.Shared.Models
{
    [Serializable]
    public class User : IUser<Channel>
    {
    }

    /// <summary>
    ///     Chat bot user interface
    /// </summary>
    [Serializable]
    public class IUser<TChannel> where TChannel : Channel, new()
    {
        [Key]
        public int Id { get; set; }

        public virtual DateTime date { get; set; }
        public virtual TChannel UserChannel { get; set; }
        public virtual TChannel BotChannel { get; set; }
        public string BaseUri { get; set; }
        public string Name { get; set; }

        public virtual void SendMessage()
        {
            //TODO
        }
    }
}