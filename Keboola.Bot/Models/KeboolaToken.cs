using System;
using System.ComponentModel.DataAnnotations;

namespace Keboola.Bot
{
    public class KeboolaToken
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Value { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Expiration { get; set; }

        public bool Enabled { get; set; }
    }
}