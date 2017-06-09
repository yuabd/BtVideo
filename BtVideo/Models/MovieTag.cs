using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BtVideo.Models
{
	public class MovieTag
	{
		[Key, Column(Order = 1)]
		public int MovieID { get; set; }
		[Key, Column(Order = 2)]
		[MaxLength(20)]
		public string Tag { get; set; }

        public virtual Movie Blog { get; set; }
	}
}