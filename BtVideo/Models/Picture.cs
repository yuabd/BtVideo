using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BtVideo.Models
{
	public class Picture
	{
		public int ID { get; set; }

		public int ParentID { get; set; }

		public string PictureFile { get; set; }

		public int Type { get; set; } //

		public string IsDefault { get; set; }

		public string Name { get; set; }
	}

	public static class PictureType
	{
		public const int CasePicture = 1;
		public const int BlogPicture = 2 ;
	}
}