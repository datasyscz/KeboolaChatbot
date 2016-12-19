using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keboola.Shared.Models
{
    public class KeboolaToken
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Value { get; set; }
        [Required]
        public DateTime Expiration { get; set; }
    }
}
