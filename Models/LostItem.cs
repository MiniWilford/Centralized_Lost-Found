using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Centralized_Lost_Found.Models
{
    public class LostItem
    {
        public string ItemName { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime LastSeenDate { get; set; }
        public string ImagePath { get; set; } // Path to uploaded image
        public string ReportedBy { get; set; }
    }
}
