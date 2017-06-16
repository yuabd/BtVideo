using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BtVideo.Models;
using BtVideo.Models.Others;
using BtVideo.Helpers;
using BtVideo.Models.Site;
using System.Net.Http;
using System.Net.Http.Headers;

namespace BtVideo.Services
{
	public class BlogService : DbAccess
	{
		public void InsertBlog(Movie blog, HttpPostedFileBase file)
		{
			if (string.IsNullOrEmpty(blog.PageTitle))
			{
				blog.PageTitle = blog.MovieTitle;
			}

			db.Movies.Add(blog);
			db.SaveChanges();

			// add slug after (depends on ID)
			blog.Slug =  blog.MovieID.ToString();

			// file
			if (file.ContentLength > 0)
			{
				UploadBlogPicture(blog, file);
			}
			db.SaveChanges();
		}

		public Movie GetBlog(int blogID)
		{
			return db.Movies.Find(blogID);
		}

		public void UpdateBlog(Movie blog, HttpPostedFileBase file)
		{
			var b = GetBlog(blog.MovieID);
            
			b.MovieTitle = blog.MovieTitle;
			b.MovieContent = blog.MovieContent;
			b.DateCreated = blog.DateCreated;
			b.PageTitle = blog.PageTitle;
			b.MetaDescription = blog.MetaDescription;
			b.MetaKeywords = blog.MetaKeywords;
			b.Slug = blog.MovieID.ToString();
			b.IsPublic = blog.IsPublic;
            b.AreaID = blog.AreaID;
            b.Director = blog.Director;
            b.LikeCount = blog.LikeCount;
            b.ShowDate = blog.ShowDate;
            b.SortOrder = blog.SortOrder;
            b.Stars = blog.Stars;
            b.Grade = blog.Grade;
            b.Magnet = blog.Magnet;
            b.ImdbLink = blog.ImdbLink;
            b.ImdbTitle = blog.ImdbTitle;
            b.AuthorID = blog.AuthorID;
            b.DateUpdate = blog.DateUpdate;

			// file
			if (file.ContentLength > 0)
			{
				UploadBlogPicture(b, file);
			}

			db.SaveChanges();
		}

		public void DeleteBlog(int blogID)
		{
			var b = GetBlog(blogID);
			db.Movies.Remove(b);

			DeleteBlogPicture(b);

			db.SaveChanges();
		}

		public Movie GetBlog(string slug)
		{
			var blog = db.Movies.FirstOrDefault(m => m.Slug == slug);
			if (blog != null)
			{
				blog.PageVisits += 1;
                if (string.IsNullOrEmpty(blog.MetaKeywords))
                {
                    blog.MetaKeywords = "";
                }
                db.SaveChanges();
			}

			return blog;
		}

		public Movie GetLastBlog()
		{
			return db.Movies.Where(m => m.IsPublic == true).OrderByDescending(m => m.DateCreated).Take(1).FirstOrDefault();
		}

		public IQueryable<Movie> GetBlogs()
		{
			return db.Movies.OrderByDescending(b => b.DateCreated);
		}

		public IQueryable<Movie> GetBlogsByCategory(int id)
		{
			var l = (from b in db.Movies
					 //join bc in db.BlogCategories on b.CategoryID equals bc.CategoryID
					 where b.MovieCategoryJoins.All(m => m.CategoryID == id)
					 orderby b.DateCreated descending
					 select b);

			return l;
		}

		public IQueryable<Movie> GetBlogsByTag(string tag)
		{
			var l = (from b in db.Movies
					 join bc in db.MovieTags on b.MovieID equals bc.MovieID
					 where bc.Tag == tag
					 orderby b.DateCreated descending
					 select b);

			return l;
		}

		public IQueryable<Movie> GetBlogsByArchive(string year, string month, string type)
		{
            if (string.IsNullOrEmpty(month))
            {
                month = "January";
            }
            //if (string.IsNullOrEmpty(year))
            //{
            //    year = DateTime.Now.Year.ToString();
            //}
            //DateTime fromTime = new DateTime();
            //if (!string.IsNullOrEmpty(year) && !string.IsNullOrEmpty(month))
            //{
            DateTime  fromTime = Convert.ToDateTime(year + "/" + month);
            //}
            //else if(!string.IsNullOrEmpty())
            //{

            //}
			
			DateTime toTime = fromTime;
			if (type == "year")
				toTime = fromTime.AddYears(1);
			else if (type == "month")
				toTime = fromTime.AddMonths(1);

			return db.Movies.Where(b => b.DateCreated >= fromTime && b.DateCreated <= toTime).OrderByDescending(b => b.DateCreated);
		}

		// Blog Comment
		public void InsertBlogComment(MovieComment blogComment)
		{
			blogComment.IsPublic = false;
			blogComment.DateCreated = DateTime.Now;
			db.MovieComments.Add(blogComment);
			db.SaveChanges();

            try
            {
                var movie = db.Movies.FirstOrDefault(m => m.MovieID == blogComment.MovieID);

                var subject = "有新的评论需要审核-bt.henhaoji.com.cn";

                var message = string.Format("<p>Hi {0},</p>" +
                    "<p>评论信息:</p>" +
                    "{1}" +
                    "<p>文章: <a href='http://bt.henhaoji.com.cn/{2}'>{3}</a></p>" +
                    "<p><a href='http://bt.henhaoji.com.cn/admin/blog/PendingComments'>点击审核</a></p>",
                    blogComment.Name,
                    blogComment.Message,
                    movie.Slug,
                    movie.MovieTitle
                    );

                //BtVideo.Models.Others.MailBag mailBag = new BtVideo.Models.Others.MailBag();

                //mailBag.ToMailAddress = "287313827@qq.com";
                ////mailBag.CcMailAddress = "";
                //mailBag.Subject = subject;
                //mailBag.Message = message;
                //mailBag.Send(true);

                IDictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("apiUser", "henhaoji");
                parameters.Add("apiKey", "Nca7r8U1ho2eYXpX");
                parameters.Add("from", "noreply@henhaoji.com.cn");
                parameters.Add("fromName", "很好记bt");
                parameters.Add("to", "287313827@qq.com");
                parameters.Add("subject", subject);
                parameters.Add("html", message);
                using (var client = new System.Net.Http.HttpClient())
                {
                    client.BaseAddress = new Uri("http://api.sendcloud.net");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // HTTP GET
                    HttpResponseMessage response = new System.Net.Http.HttpResponseMessage();

                    response = client.PostAsync("/apiv2/mail/send", new FormUrlEncodedContent(parameters)).Result;

                    var product = "";
                    if (response.IsSuccessStatusCode)
                    {
                        product = response.Content.ReadAsStringAsync().Result;
                    }
                }
            }
            catch (Exception e)
            {
                LogHelper.WriteLog("添加评论发邮件错误", e.Message);
            }
        }

		public void UpdateBlogComment(MovieComment blogComment)
		{
			var bf = GetBlogComment(blogComment.CommentID);
			bf.Message = blogComment.Message;
		}

		public void DeleteBlogComment(int commentID)
		{
			var c = GetBlogComment(commentID);
			db.MovieComments.Remove(c);

			db.SaveChanges();
		}

		public MovieComment GetBlogComment(int commentID)
		{
			return db.MovieComments.Find(commentID);
		}

		public IQueryable<MovieComment> GetBlogComments(int blogID)
		{
			return db.MovieComments.Where(p => p.MovieID == blogID).OrderByDescending(m => m.DateCreated);
		}

		public void ApproveBlogComment(int commentID)
		{
			var c = db.MovieComments.FirstOrDefault(m => m.CommentID == commentID);
            try
            {
                c.IsPublic = true;

                db.SaveChanges();
            }
            catch (Exception e)
            {
                LogHelper.WriteLog("pinglun:", e.StackTrace + e.Message + e.Source);
            }

            // notify posting user

            try
            {
                var subject = "您的评论已经审核通过-bt.henhaoji.com.cn";

                var message = string.Format("<p>Hi {0},</p>" +
                    "<p>您的评论已经被审核通过:</p>" +
                    "{1}" +
                    "<p>文章: <a href='http://bt.henhaoji.com.cn/{2}'>{3}</a></p>",
                    c.Name,
                    c.Message,
                    c.Movie.Slug,
                    c.Movie.MovieTitle
                    );
                
                IDictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("apiUser", "henhaoji");
                parameters.Add("apiKey", "Nca7r8U1ho2eYXpX");
                parameters.Add("from", "noreply@henhaoji.com.cn");
                parameters.Add("fromName", "很好记bt");
                parameters.Add("to", c.Email);
                parameters.Add("subject", subject);
                parameters.Add("html", message);
                using (var client = new System.Net.Http.HttpClient())
                {
                    client.BaseAddress = new Uri("http://api.sendcloud.net");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // HTTP GET
                    HttpResponseMessage response = new System.Net.Http.HttpResponseMessage();

                    response = client.PostAsync("/apiv2/mail/send", new FormUrlEncodedContent(parameters)).Result;

                    var product = "";
                    if (response.IsSuccessStatusCode)
                    {
                        product = response.Content.ReadAsStringAsync().Result;
                    }
                }
            }
            catch (Exception e)
            {
            }
        }

		public IQueryable<MovieComment> GetComments()
		{
			var list = (from l in db.MovieComments
						orderby l.DateCreated descending
						select l);

			return list;
		}

		public IQueryable<Movie> GetBlogsWithPendingComments()
		{
			var r = (from b in db.Movies
					 where db.MovieComments.Any(c => c.IsPublic == false && c.MovieID == b.MovieID)
					 select b);

			return r;
		}

		// Blog Category
		public void InsertBlogCategory(MovieCategory blogCategory)
		{
			if (string.IsNullOrEmpty(blogCategory.PageTitle))
			{
				blogCategory.PageTitle = blogCategory.CategoryName;
			}
			db.MovieCategories.Add(blogCategory);

            db.SaveChanges();

            blogCategory.Slug = blogCategory.CategoryID.ToString();

            db.SaveChanges();
        }

		public MovieCategory GetBlogCategory(int id)
		{
			return db.MovieCategories.Find(id);
		}

		public void UpdateBlogCategory(MovieCategory blogCategory)
		{
			var bc = GetBlogCategory(blogCategory.CategoryID);
            
			bc.CategoryName = blogCategory.CategoryName;
			bc.PageTitle = blogCategory.PageTitle;
			bc.MetaDescription = blogCategory.MetaDescription;
			bc.MetaKeywords = blogCategory.MetaKeywords;
			bc.SortOrder = blogCategory.SortOrder;

			db.SaveChanges();
		}

		public void DeleteBlogCategory(int categoryID)
		{
			var bc = GetBlogCategory(categoryID);
			db.MovieCategories.Remove(bc);

			db.SaveChanges();
		}

		public IQueryable<MovieCategory> GetBlogCategories()
		{
			return db.MovieCategories.OrderBy(m => m.SortOrder);
		}

        /// <summary>
        /// 保存分类
        /// </summary>
        /// <param name="movie"></param>
        /// <param name="categories"></param>
        public void SaveCategory(int movieId, string[] categories)
        {
            MovieCategoryJoin join = null;

            var cateJoins = db.MovieCategoryJoins.Where(m => m.MovieID == movieId).ToList();
            
            db.MovieCategoryJoins.RemoveRange(cateJoins);

            db.SaveChanges();

            foreach (var item in categories)
            {
                join = new MovieCategoryJoin()
                {
                    CategoryID = item.Uint(),
                    MovieID = movieId
                };

                db.MovieCategoryJoins.Add(join);
            }

            db.SaveChanges();
        }

		//Blog Tag
		public IQueryable<Anonymous> GetPopularTags()
		{
			var pt = (from p in db.MovieTags
					  group p by new { p.Tag } into t
					  orderby t.Count() descending
                      select new Anonymous { Tag = t.Key.Tag, Num = t.Count() }).Take(10);

			return pt;
		}

        public IQueryable<MovieTag> GetTags()
        {
            return db.MovieTags;
        }

		public IQueryable<MovieTag> GetBlogTags(int blogID)
		{
			return db.MovieTags.Where(b => b.MovieID == blogID);
		}

		public void SaveBlogTags(Movie blog, List<MovieTag> blogTags)
		{
			var bt = GetBlogTags(blog.MovieID);

			foreach (MovieTag tag in bt)
			{
				DeleteBlogTag(tag);
			}
            var b = db.Movies.FirstOrDefault(m => m.MovieID == blog.MovieID);

            foreach (MovieTag tag in blogTags)
			{
				tag.MovieID = blog.MovieID;
                
                if (!string.IsNullOrEmpty(tag.Tag))
				{
					tag.Tag = BtVideo.Helpers.Utilities.GenerateSlug(tag.Tag, 20);
                    
					InsertBlogTag(tag);
				}
			}

			db.SaveChanges();
		}

		public void InsertBlogTag(MovieTag blogTag)
		{
			db.MovieTags.Add(blogTag);
		}

		public void DeleteBlogTag(MovieTag blogTag)
		{
			db.MovieTags.Remove(blogTag);
		}

		public PreNextBlog GetPreNextBlog(int id)
		{
			PreNextBlog blog = new PreNextBlog();

			var pre = (from l in db.Movies
					   where l.MovieID < id
					   orderby l.MovieID descending
					   select new BlogIDName()
					   {
						   ID = l.MovieID,
						   Slug = l.Slug,
						   Title = l.MovieTitle
					   }).FirstOrDefault();

			var next = (from l in db.Movies
						where l.MovieID > id
						orderby l.MovieID
						select new BlogIDName()
						{
							ID = l.MovieID,
							Slug = l.Slug,
							Title = l.MovieTitle
						}).FirstOrDefault();
			blog.PreBlog = pre;
			blog.NextBlog = next;

			return blog;
		}

        //地区
        public IQueryable<MovieArea> GetAreas()
        {
            return db.MovieAreas;
        }

        public void InsertArea(MovieArea area)
        {
            db.MovieAreas.Add(area);

            db.SaveChanges();
        }

        // Others
        public IEnumerable<Archive> GetArchives()
		{
			// Get months list
			DateTime dt = new DateTime(2012, 1, 1);
			List<SelectListItem> months = new List<SelectListItem>();

			for (int i = 1; i <= 12; i++)
			{
				SelectListItem item = new SelectListItem();
				item.Value = i.ToString();
				item.Text = dt.AddMonths(i - 1).ToString("MMMM");
				months.Add(item);
			}

			// get the archives list
			var Archives = new List<Archive>();

			foreach (var item in months)  //this year per month
			{
				var Archive = new Archive();
				Archive.Month = item.Text;
				Archive.Year = DateTime.Now.Year.ToString();
				Archive.Count = GetBlogsByArchive(Archive.Year, Archive.Month, "month").Count();
				if (Archive.Count > 0)
				{
					Archives.Add(Archive);
				}
			}

			for (int i = 1; i <= 5; i++)  //last 5 years
			{
				var Archive = new Archive();
				Archive.Year = DateTime.Now.AddYears(-i).Year.ToString();
				Archive.Month = "";
				Archive.Count = GetBlogsByArchive(Archive.Year, Archive.Month, "year").Count();
				if (Archive.Count > 0)
				{
					Archives.Add(Archive);
				}
			}

			return Archives;
		}
        
		// Files
		public void UploadBlogPicture(Movie blog, HttpPostedFileBase file)
		{
			var fileName = string.Format("{0}-{1}", blog.MovieID, blog.Slug);

            blog.PictureFile = BtVideo.Helpers.Utilities.UploadImageFile(file.InputStream, blog.PictureFolder, fileName, 400, 400, BtVideo.Helpers.ImageSaveType.Width);
		}

		private void DeleteBlogPicture(Movie blog)
		{
			BtVideo.Helpers.Utilities.DeleteFile(blog.PictureFolder, blog.PictureFile);
		}


        public void SaveStar(string[] stars, string[] directors, int movieId)
        {
            //先清除所有明星
            var allStars = db.MovieStarJoins.Where(m => m.MovieID == movieId).ToList();

            db.MovieStarJoins.RemoveRange(allStars);

            db.SaveChanges();

            InsertStar(stars, StarType.actor, movieId);

            InsertStar(directors, StarType.director, movieId);
        }

        private void InsertStar(string[] stars, string type, int movieId)
        {
            MovieStar star = null;
            MovieStarJoin starJoin = null;
            var s = string.Empty;
            foreach (var item in stars)
            {
                s = item.UString();
                star = db.MovieStars.Where(m => m.StarName == s).FirstOrDefault();
            
                if (star == null)
                {
                    star = new MovieStar()
                    {
                        StarName = s
                    };

                    db.MovieStars.Add(star);

                    db.SaveChanges();
                }

                starJoin = new MovieStarJoin()
                {
                    MovieID = movieId,
                    StarID = star.StarID,
                    Type = type
                };

                db.MovieStarJoins.Add(starJoin);

                db.SaveChanges();
            }
        }


        #region bt链接

        public IQueryable<MovieLink> GetMovieLinks(int movieId)
        {
            return db.MovieLinks.Where(m => m.MovieID == movieId);
        }

        public MovieLink GetMovieLink(int id)
        {
            return db.MovieLinks.FirstOrDefault(m => m.LinkID == id);
        }

        public void InsertMovieLink(MovieLink link)
        {
            db.MovieLinks.Add(link);

            db.SaveChanges();
        }

        public void UpdateMovieLink(MovieLink link)
        {
            var link_db = db.MovieLinks.Where(m => m.LinkID == link.LinkID).FirstOrDefault();
            
            link_db.LinkName = link.LinkName;
            link_db.LinkUrl = link.LinkUrl;
            link_db.Magnet = link.Magnet;

            db.SaveChanges();
        }

        public void DeleteMovieLink(MovieLink link)
        {
            db.MovieLinks.Remove(link);

            db.SaveChanges();
        }

        #endregion

        // Save
        public void Save()
		{
			db.SaveChanges();
		}
	}
}