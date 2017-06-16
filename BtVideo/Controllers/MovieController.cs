using BtVideo.Services;
using System.Web.Mvc;
using System.Linq;
using BtVideo.Models;
using BtVideo.Models.Site;
using BtVideo.Models.Others;
using BtVideo.Helpers;
using System.IO;

namespace BtVideo.Controllers
{
    [GlobalFilter]
    public class MovieController : Controller
    {
        private BlogService bs = new BlogService();
        private SiteService sites = new SiteService();
        // GET: V
        [Route("~/list")]
        [Route("~/list{id:int}")]
        public ActionResult Index(int? id, int? page, string keywords)
        {
            var blogs = bs.GetBlogs().Where(m => m.IsPublic == true);

            if (!string.IsNullOrEmpty(keywords))
            {
                var key = keywords.Split(' ');
                foreach (var item in key)
                {
                    sites.SaveKeyword(item.Trim());

                    blogs = (from l in blogs
                             where l.MovieTitle.Contains(item) || l.MovieContent.Contains(item)
                             select l);
                }
            }

            if (id.HasValue)
            {
                blogs = (from l in blogs

                         where l.MovieCategoryJoins.Any(m => m.CategoryID == id.Value)
                         select l);
            }

            ViewBag.Count = blogs.Select(m => m.MovieID).Count();

            var pBlogs = new Paginated<Movie>(blogs.ToList(), page ?? 1, 8);

            //var popularTags = (from p in bs.GetTags()
            //                   group p by new { p.Tag } into t
            //                   orderby t.Count() descending
            //                   select new Anonymous { Tag = t.Key.Tag, Num = t.Count() }).Take(10).ToList();

            var model = new BlogsViewModel(pBlogs, null, null, null);
            ViewBag.PageTitle = string.IsNullOrEmpty(keywords) ? "所有影片" : "搜索结果: " + keywords;

            return View(model);
        }

        [Route("~/{id:int:min(1)}")]
        public ActionResult Detail(string id)
        {
            var blog = bs.GetBlog(id);

            if (blog == null)
            {
                //string newurl = "http://www.henhaoji.com.cn" + System.Web.HttpContext.Current.Request.RawUrl;
                System.Web.HttpContext.Current.Response.Clear();
                System.Web.HttpContext.Current.Response.StatusCode = 404;
                System.Web.HttpContext.Current.Response.Status = "404 Moved Permanently";
                //System.Web.HttpContext.Current.Response.AddHeader("Location", "");
                //Response.Redirect("/404.html");
                return View("NotFound");
            }

            //var blogComment = new BlogComment();
            var blogID = blog == null ? 0 : blog.MovieID;

            //var blogComments = bs.GetBlogComments(blogID).Where(m => m.IsPublic == true).ToList();
            //var categories = bs.GetBlogCategories().ToList();
            //var popularTags = bs.GetPopularTags().Take(10).ToList();
            //var archives = bs.GetArchives().ToList();

            var preNextBlog = bs.GetPreNextBlog(blogID);
            var model = new BlogViewModel(blog, new MovieComment() { MovieID = blog.MovieID }, null, null, null, null, preNextBlog);

            ViewBag.Blog = "current";
            return View(model);
        }

        [Route("~/tags/{id}")]
        public ActionResult Tags(string id, int? page)
        {
            var blogs = bs.GetBlogsByTag(id).Where(m => m.IsPublic == true);

            ViewBag.Count = blogs.Select(m => m.MovieID).Count();

            var pBlogs = new Paginated<Movie>(blogs, page ?? 1, 8);

            //var categories = bs.GetBlogCategories().ToList();
            var popularTags = bs.GetPopularTags().Take(10).ToList();
            //var archives = bs.GetArchives().ToList();

            var model = new BlogsViewModel(pBlogs, null, popularTags, null);

            ViewBag.PageTitle = "标签: " + id;
            ViewBag.Blog = "current";

            return View("Index", model);
        }

        [Route("~/star/{id}")]
        public ActionResult Star(string id, int? page)
        {
            var blogs = bs.GetBlogs().Where(m => m.Stars.Contains(id));

            ViewBag.Count = blogs.Select(m => m.MovieID).Count();

            var pBlogs = new Paginated<Movie>(blogs, page ?? 1, 8);

            var popularTags = bs.GetPopularTags().Take(10).ToList();

            var model = new BlogsViewModel(pBlogs, null, popularTags, null);

            ViewBag.PageTitle = "明星: " + id;
            ViewBag.Blog = "current";

            return View("Index", model);
        }

        public ActionResult Captcha()
        {
            Captcha captcha = new Captcha(85, 32, 5, 25f);
            Session["Captcha"] = captcha.Text;
            return File(captcha.ImageData, "image/jpeg");
        }

        public ActionResult GetApprovedCommentOfPost(int id)
        {
            var comments = bs.GetComments().Where(m => m.IsPublic == true && m.MovieID == id).ToList();

            return View(comments);
        }

        [HttpPost]
        public ActionResult AddComment(MovieComment blogComment, string CaptchaCode)
        {
            bs.InsertBlogComment(blogComment);

            return RedirectToAction("GetApprovedCommentOfPost", new { id = blogComment.MovieID });
        }

        [Route("~/download/{id}")]
        public ActionResult Download(int id)
        {
            var download = bs.GetMovieLink(id);

            if (download == null)
            {
                return Content("链接不存在！！");
            }

            download.DownloadCount += 1;
            bs.Save();
            if (download.LinkUrl.Contains("http"))
            {
                return Redirect(download.LinkUrl);
            }
            else
            {
                return File(new FileStream(HttpContext.Server.MapPath(download.PictureFolder + "/" + download.LinkUrl), FileMode.Open), "application/octet-stream", Server.UrlEncode(download.LinkName));
            }
        }
    }
}