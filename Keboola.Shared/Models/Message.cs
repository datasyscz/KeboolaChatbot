﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Keboola.Shared
{
    /// <summary>
    ///     Message in database
    /// </summary>
    public class Message
    {
        [Key]
        public int MessageId { get; set; }

        public string Text { get; set; }

        [Required]
        public virtual DateTime Date { get; set; }

        public bool SendedByUser { get; set; }
    }
}