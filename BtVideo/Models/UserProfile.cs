using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BtVideo.Models
{
	public class UserProfile
	{
		[Key]
		[ForeignKey("User")]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int UserID { get; set; }
		[MaxLength(20)]
		[Required]
		public string UserName { get; set; }
		[MaxLength(2)]
		public string Gender { get; set; }
		public DateTime DateCreated { get; set; } //入职时间
		[MaxLength(100)]
		[Required]
		public string Address { get; set; }
		[MaxLength(100)]
		[Required]
		public string School { get; set; } //毕业学校
		[MaxLength(20)]
		[Required]
		public string Position { get; set; }
		[MaxLength(15)]
		[Required]
		public string Phone { get; set; }
		[MaxLength(15)]
		[Required]
		public string EmergencyPhone { get; set; } //紧急联系电话
		[Required]
		[MaxLength(15)]
		public string EmergencyContact { get; set; } //紧急联系人
		[Required]
		[StringLength(18, MinimumLength = 18)]
		public string IDCode { get; set; }
		[MaxLength(400)]
		[Required]
		public string Description { get; set; } //简介
		[MaxLength(50)]
		public string PictureFile { get; set; }

		public virtual User User { get; set; }
	}
}