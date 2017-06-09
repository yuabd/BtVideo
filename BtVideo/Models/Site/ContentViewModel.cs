using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BtVideo.Models.Site
{
	public class ContentViewModel
	{
		public Movie Content { get; private set; }
		public IEnumerable<Movie> Sidebar { get; private set; }

		public ContentViewModel(Movie content, IEnumerable<Movie> sidebar)
		{
			Content = content;
			Sidebar = sidebar;
		}
	}
}