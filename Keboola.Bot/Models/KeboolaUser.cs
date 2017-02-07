using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keboola.Bot
{
    [Serializable]
    public class KeboolaUser
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public virtual KeboolaToken Token { get; set; }
        public virtual List<KeboolaToken> InactiveTokens { get; set; }
        
        /// <summary>
        /// User is activated, user can be notified etc..
        /// </summary>
        public bool Active { get; set; }
    }
}
