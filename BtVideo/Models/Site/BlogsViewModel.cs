using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BtVideo.Helpers;
using BtVideo.Models;
using BtVideo.Models.Others;
using BtVideo.Services;

namespace BtVideo.Models.Site
{
	public class BlogsViewModel
	{
		public Paginated<Movie> Blogs { get; private set; }

		public IEnumerable<MovieCategory> Categories { get; private set; }

		public IEnumerable<Anonymous> PopularTags { get; private set; }

		public IEnumerable<Archive> Archives { get; private set; }

		public BlogsViewModel(
			Paginated<Movie> blogs,
			IEnumerable<MovieCategory> categories,
			IEnumerable<Anonymous> popularTags,
			IEnumerable<Archive> archives
			)
		{
			Blogs = blogs;
			Categories = categories;
			PopularTags = popularTags;
			Archives = archives;
		}
	}
}