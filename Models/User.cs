using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Centralized_Lost_Found.Models
{
    public class User
    {
        // 3-27-2025: RH - User Model for storing user information into the SQLite database
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; } 

        public string Avatar { get; set; } 

        public string Password { get; set; } 

        public string Email { get; set; } 

        public int ReportedItems { get; set; }

        public string Location { get; set; }

        public int Warnings { get; set; }

        public bool AccountTerminated { get; set; }  // KD - Add for helping in post moderation (delete account if becomes true)

    }

    
}
