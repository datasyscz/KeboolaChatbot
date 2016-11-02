using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;


namespace DatabaseModel
{
    [Serializable]
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        public virtual Channel UserChannel {get;set;}
        public virtual Channel BotChannel { get; set; }
        public int ConversationID { get; set;}
        public string BaseUri { get; set; }


        public Customer()
        {

        }
    }
}