using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Centralized_Lost_Found.Models
{
    public class InboxMessage
    {
        public string Topic { get; set; }
        public string User { get; set; }
        public string Location { get; set; }
        public string Found { get; set; }
        public string FoundColor { get; set; }
    }
}
