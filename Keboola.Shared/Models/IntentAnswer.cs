using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chatbot.Shared.Models
{
    /// <summary>
    ///     Predefined answer
    /// </summary>
    public class IntentAnswer
    {
        [Key]
        public int Id { get; set; }

        [Index("IX_Name", IsUnique = true)]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public string Answer { get; set; }

        public bool Advanced { get; set; }
    }
}