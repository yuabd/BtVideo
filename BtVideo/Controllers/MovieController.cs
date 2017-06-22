using BtVideo.Services;
using System.Web.Mvc;
using System.Linq;
using BtVideo.Models;
using BtVideo.Models.Site;
using BtVideo.Models.Others;
using BtVideo.Helpers;
using System.IO;
using System;
using System.Web;
using System.Collections.Generic;

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
        public ActionResult Index(int? id, int? page)
        {
            var blogs = bs.GetBlogs().Where(m => m.IsPublic == true);

            //根据分类搜索
            if (id.HasValue)
            {
                blogs = (from l in blogs
                         where l.MovieCategoryJoins.Any(m => m.CategoryID == id.Value)
                         select l);
            }

            var pBlogs = new Paginated<Movie>(blogs, page ?? 1, 24);

            var model = new BlogsViewModel(pBlogs, null, null, null);
            if (id.HasValue)
            {
                ViewBag.Category = bs.GetBlogCategory(id.Value);
                ViewBag.PageTitle = "最新" + ViewBag.Category.CategoryName;
                if (id != 3 || id != 4 || id != 38)
                {
                    ViewBag.PageTitle += "电影电视剧";
                }
            }
            else
            {
                ViewBag.PageTitle = "所有影片";
            }

            if (page.HasValue)
            {
                ViewBag.PageTitle += "_第" + page + "页";
            }

            return View(model);
        }

        [Route("~/{id:int:min(1)}")]
        public ActionResult Detail(string id)
        {
            var blog = bs.GetBlog(id);

            if (blog == null)
            {
                System.Web.HttpContext.Current.Response.Clear();
                System.Web.HttpContext.Current.Response.StatusCode = 404;
                System.Web.HttpContext.Current.Response.Status = "404 Moved Permanently";

                return View("NotFound");
            }
            
            var blogID = blog == null ? 0 : blog.MovieID;
            
            var preNextBlog = bs.GetPreNextBlog(blogID);
            var model = new BlogViewModel(blog, new MovieComment() { MovieID = blog.MovieID }, null, null, null, null, preNextBlog);

            ViewBag.suiji = bs.GetBlogs().OrderBy(m => Guid.NewGuid()).Take(4).ToList();

            ViewBag.Blog = "current";
            return View(model);
        }

        [Route("~/tags/{id}")]
        public ActionResult Tags(string id, int? page)
        {
            var blogs = bs.GetBlogsByTag(id).Where(m => m.IsPublic == true);

            //ViewBag.Count = blogs.Select(m => m.MovieID).Count();

            var pBlogs = new Paginated<Movie>(blogs, page ?? 1, 24);

            //var categories = bs.GetBlogCategories().ToList();
            //var popularTags = bs.GetPopularTags().Take(10).ToList();
            //var archives = bs.GetArchives().ToList();

            var model = new BlogsViewModel(pBlogs, null, null, null);

            ViewBag.PageTitle = "标签: " + id;
            ViewBag.Blog = "current";

            return View("Index", model);
        }

        [Route("~/star/{id}")]
        public ActionResult Star(string id, int? page)
        {
            var blogs = bs.GetBlogs().Where(m => m.Stars.Contains(id));

            //ViewBag.Count = blogs.Select(m => m.MovieID).Count();

            var pBlogs = new Paginated<Movie>(blogs, page ?? 1, 24);

            //var popularTags = bs.GetPopularTags().Take(10).ToList();

            var model = new BlogsViewModel(pBlogs, null, null, null);

            ViewBag.PageTitle = "明星: " + id;
            ViewBag.Blog = "current";

            return View("Index", model);
        }

        [Route("~/search")]
        public ActionResult Search(int? page, string k)
        {
            //var blogs = bs.GetBlogs().Where(m => m.IsPublic == true).ToList();
            
            //foreach (var item in blogs)
            //{
            //    IndexManager.bookIndex.Add(item);
            //}

            IEnumerable<Movie> list = null;

            if (!string.IsNullOrEmpty(k))
            {
                var search = new SearchHelper();
                list = search.Search(k, page);

                ViewBag.PageTitle = k;
            }

            var pBlogs = new Paginated<Movie>(list.AsQueryable(), page ?? 1, 24, );
            var model = new BlogsViewModel(pBlogs, null, null, null);
            if (page.HasValue)
            {
                ViewBag.PageTitle += "_第" + page + "页";
            }

            return View("~/Views/Movie/Index.cshtml", model);
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

        /// <summary>
        /// 赞
        /// </summary>
        /// <returns></returns>
        [Route("~/like/{id}")]
        public ActionResult Like(int id)
        {
            try
            {
                var ses = HttpContext.Request.Cookies["like-" + id];

                if (ses == null)
                {
                    var movie = bs.GetBlog(id);

                    movie.LikeCount += 1;

                    bs.Save();

                    HttpCookie cookie = new HttpCookie("like-" + id);
                    cookie.Value = id.ToString();
                    HttpContext.Response.Cookies.Add(cookie);

                    return Json(new { IsSuccess = 1 }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { Message = "你已经点过赞了！" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                LogHelper.WriteLog("点赞", e.Message);
            }

            return Json(new { Message = "未知错误" }, JsonRequestBehavior.AllowGet);
        }
    }
}