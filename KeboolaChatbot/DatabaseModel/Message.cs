using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseModel
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }

        public string Text { get; set; }

        [Required]
        public virtual DateTime Date { get; set; }

        public bool SendedByUser { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
