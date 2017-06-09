using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BtVideo.Models.Admin
{
	public class BlogViewModel
	{
		public Movie Blog { get; private set; }

		public IEnumerable<MovieTag> BlogTags { get; private set; }

		public BlogViewModel(Movie blog, IEnumerable<MovieTag> blogTags)
		{
			Blog = blog;
			BlogTags = blogTags;
		}
	}
}