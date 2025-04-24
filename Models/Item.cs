using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Centralized_Lost_Found.Models
{
	// 3-27-2025: KD - Item Model for retrieving lost items and storing into the SQLite database
	public class Item
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		public string Location { get; set; }
		public string Picture { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public bool IsFound { get; set; }
		public DateTime LastSeenDate { get; set; }
		public string Uploader { get; set; }

		public string FoundByUser { get; set; }
		public string FoundColor { get; set; }
	}
}
