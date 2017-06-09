using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BtVideo.Models
{
	public class UserRole
	{
		[Key]
		[MaxLength(15)]
		public string RoleID { get; set; }

		public virtual ICollection<UserRoleJoin> UserRoleJoins { get; set; }
	}
}