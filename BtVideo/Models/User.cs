using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BtVideo.Models
{
	public class User
	{
		public int UserID { get; set; }
		[MaxLength(32)]
		[Required]
		public string Password { get; set; }
		public DateTime DateCreated { get; set; }
		public DateTime DateLastLogin { get; set; }
		public bool IsActive { get; set; }

		public virtual UserProfile UserProfile { get; set; }

		public virtual ICollection<UserRoleJoin> UserRoleJoins { get; set; }
	}
}