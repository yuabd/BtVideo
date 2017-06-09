using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BtVideo.Models
{
	public class UserRoleJoin
	{
		[Key]
		[Column(Order = 1)]
        public int UserID { get; set; }
		[Key]
		[Column(Order = 2)]
        public string RoleID { get; set; }

		public virtual User User { get; set; }

        public virtual UserRole UserRole { get; set; }
	}
}