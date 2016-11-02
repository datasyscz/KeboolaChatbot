using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseModel
{
    public class Channel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FrameworkId { get; set; }
    }
}
