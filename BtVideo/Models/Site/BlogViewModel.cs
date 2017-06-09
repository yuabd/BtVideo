using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BtVideo.Services;
using BtVideo.Models.Others;

namespace BtVideo.Models.Site
{
	public class BlogViewModel
	{
		public Movie Blog { get; private set; }

		public List<MovieComment> BlogComments { get; private set; }

		public MovieComment BlogComment { get; private set; }

		public List<MovieCategory> Categories { get; private set; }

		public List<Anonymous> PopularTags { get; private set; }

		public List<Archive> Archives { get; private set; }

		public PreNextBlog PreNextBlog { get; set; }

		public BlogViewModel(
			Movie blog,
			MovieComment blogComment,
            List<MovieComment> blogComments,
            List<MovieCategory> categories,
            List<Anonymous> popularTags,
            List<Archive> archives, PreNextBlog preNextBlog)
		{
			Blog = blog;
			BlogComments = blogComments;
			BlogComment = blogComment;
			Categories = categories;
			PopularTags = popularTags;
			Archives = archives;
			PreNextBlog = preNextBlog;
		}
	}
}