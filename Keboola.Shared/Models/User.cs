using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Keboola.Shared.Models;

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
        public virtual KeboolaUser KeboolaUser { get; set; }

        public string BaseUri { get; set; }

        public string Name { get; set; }

        public bool IsActivated()
        {
            return KeboolaUser?.Active == true;
        }


        public string IsActivatedStr
        {
            get{return IsActivated() ? "Yes" : "No";}
        }
    }
}