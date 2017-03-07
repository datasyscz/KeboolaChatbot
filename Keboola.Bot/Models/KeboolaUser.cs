using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Keboola.Bot
{
    [Serializable]
    public class KeboolaUser
    {
        [Key]
        public int Id { get; set; }

        [Index("KeboolaId", IsUnique = true)]
        public int KeboolaId { get; set; }

        [Required]
        public virtual KeboolaToken Token { get; set; }

        public virtual List<KeboolaToken> InactiveTokens { get; set; }

        /// <summary>
        ///     User is activated, user can be notified etc..
        /// </summary>
        public bool Active { get; set; }

        public void AddToken(string token, DateTime TokenExpiration)
        {
            var newToken = new KeboolaToken
            {
                Value = token,
                Expiration = TokenExpiration
            };
        }
    }
}