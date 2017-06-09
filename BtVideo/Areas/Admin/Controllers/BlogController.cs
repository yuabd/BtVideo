using BtVideo.Helpers;
using BtVideo.Models;
using BtVideo.Models.Admin;
using BtVideo.Models.Others;
using BtVideo.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace BtVideo.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class BlogController : Controller
    {
        private BlogService blogService = new BlogService();

        //
        // GET: /Admin/Blog/
        public ActionResult Index(int? page)
        {
            var blogs = blogService.GetBlogs();
            var pblogs = new Paginated<Movie>(blogs, page ?? 1, 25);

            return View(pblogs);
        }

        public ActionResult Add()
        {
            var blog = new Movie();
            blog.AuthorID = GlobalHelper.UserName();
            blog.DateCreated = DateTime.Now;
            blog.IsPublic = true;
            blog.ShowDate = DateTime.Now;
            blog.AreaID = 1;

            IEnumerable<MovieTag> blogTags = null;

            var model = new BlogViewModel(blog, blogTags);

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(Movie blog, List<MovieTag> blogTags)
        {
            if (ModelState.IsValid)
            {
                var categories = Request["Categories"].Split(',');

                HttpPostedFileBase file = Request.Files["PictureFile"];

                if (string.IsNullOrEmpty(blog.MetaKeywords))
                {
                    foreach (var tag in blogTags)
                    {
                        if (!string.IsNullOrEmpty(tag.Tag))
                            blog.MetaKeywords += tag.Tag + ",";
                    }
                }
                blog.LikeCount = 0;
                blog.PageVisits = 0;
                blog.SortOrder = 0;

                blogService.InsertBlog(blog, file);

                blogService.SaveCategory(blog.MovieID, categories);

                blogService.SaveStar(blog.Stars.Split('|'), blog.Director.Split('|'), blog.MovieID);

                blogService.SaveBlogTags(blog, blogTags);

                return RedirectToAction("Edit", new { id = blog.MovieID });
            }
            else
            {
                var model = new BlogViewModel(blog, blogTags);
                return View(model);
            }
        }

        public ActionResult Edit(int id)
        {
            var blog = blogService.GetBlog(id);
            var blogTags = blogService.GetBlogTags(id);

            var model = new BlogViewModel(blog, blogTags);

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Movie blog, List<MovieTag> blogTags)
        {
            if (ModelState.IsValid)
            {
                var categories = Request["Categories"].Split(',');

                HttpPostedFileBase file = Request.Files["PictureFile"];

                if (string.IsNullOrEmpty(blog.MetaKeywords))
                {
                    foreach (var tag in blogTags)
                    {
                        if (!string.IsNullOrEmpty(tag.Tag))
                            blog.MetaKeywords += tag.Tag + ",";
                    }
                }

                blogService.UpdateBlog(blog, file);

                blogService.SaveCategory(blog.MovieID, categories);

                blogService.SaveStar(blog.Stars.Split('|'), blog.Director.Split('|'), blog.MovieID);

                blogService.SaveBlogTags(blog, blogTags);

                return RedirectToAction("Index");
            }
            else
            {
                var model = new BlogViewModel(blog, blogTags);
                return View(model);
            }
        }

        public ActionResult Delete(int id)
        {
            blogService.DeleteBlog(id);

            return RedirectToAction("Index");
        }

        //
        // Comments

        public ActionResult PendingComments()
        {
            var blogs = blogService.GetBlogsWithPendingComments().ToList();

            return View(blogs);
        }

        // TODO: is this being used?
        [HttpPost]
        public string AddComment(MovieComment comment)
        {
            if (ModelState.IsValid)
            {
                blogService.InsertBlogComment(comment);

                return "Thank you for your comment";
            }
            else
            {
                return "Error";
            }
        }

        public ActionResult ApproveComment(int id)
        {
            blogService.ApproveBlogComment(id);
            return RedirectToAction("PendingComments");
        }

        public ActionResult DeleteComment(int id)
        {
            blogService.DeleteBlogComment(id);

            return RedirectToAction("PendingComments");
        }

        //
        // GET: /Admin/Blog/Categories

        public ActionResult Categories()
        {
            var categories = blogService.GetBlogCategories().ToList();

            return View(categories);
        }

        [HttpPost]
        public ActionResult Categories(MovieCategory category)
        {
            if (ModelState.IsValid)
            {
                if (category.CategoryID > 0)
                {
                    blogService.UpdateBlogCategory(category);
                }
                else
                {
                    blogService.InsertBlogCategory(category);
                }

                return RedirectToAction("Categories");
            }
            else
            {
                return RedirectToAction("Categories");
            }
        }

        public ActionResult DeleteCategory(int id)
        {
            blogService.DeleteBlogCategory(id);
            //blogService.Save();

            return RedirectToAction("Categories");
        }

        public ActionResult UploadPicture(HttpPostedFileBase filedata)
        {
            xheditorModel model = new xheditorModel();

            try
            {
                if (filedata.ContentLength > 0)
                {
                    var fileName = Guid.NewGuid().ToString();
                    var file = BtVideo.Helpers.Utilities.UploadImageFile(filedata.InputStream, "/Content/Pictures/Blog", fileName, 800, 400, BtVideo.Helpers.ImageSaveType.Original);

                    model.msg = "/Content/Pictures/Blog" + file;
                }

                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                return this.Content(javaScriptSerializer.Serialize(model));
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                filedata = null;
            }
        }

        #region bt链接


        public ActionResult MovieLinks(int id)
        {
            ViewBag.Id = id;

            var blog = blogService.GetMovieLinks(id).ToList();

            return View(blog);
        }

        public ActionResult AddMovieLink(int id)
        {
            var link = new MovieLink();
            link.MovieID = id;

            return View(link);
        }

        [HttpPost]
        public ActionResult AddMovieLink(MovieLink model, HttpPostedFileBase file)
        {
            model.DownloadCount = 0;
            ModelState.Remove("LinkUrl");

            if (ModelState.IsValid)
            {
                model.LinkUrl = "1";
                blogService.InsertMovieLink(model);
                // file
                if (file.ContentLength > 0)
                {
                    var fileName = string.Format("{0}-{1}.torrent", model.LinkID, model.LinkName);
                    var filePath = HttpContext.Server.MapPath(model.PictureFolder + "/" + fileName);

                    string directory = Path.GetDirectoryName(filePath);
                    if (directory != null && !Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }

                    file.SaveAs(filePath);
                    model.LinkUrl = fileName;
                }

                blogService.UpdateMovieLink(model);
            }

            return RedirectToAction("MovieLinks", new { id = model.MovieID });
        }

        public ActionResult UpdateMovieLink(int id)
        {
            var link = blogService.GetMovieLink(id);

            return View(link);
        }

        [HttpPost]
        public ActionResult UpdateMovieLink(MovieLink model, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file.ContentLength > 0)
                {
                    var fileName = string.Format("{0}-{1}", model.MovieID, model.LinkName);
                    var filePath = HttpContext.Server.MapPath(fileName);

                    string directory = Path.GetDirectoryName(filePath);
                    if (directory != null && !Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }

                    file.SaveAs(filePath);

                    if (System.IO.File.Exists(model.LinkUrl))
                    {
                        System.IO.File.Delete(model.LinkUrl);
                    }

                    model.LinkUrl = filePath;
                }

                blogService.UpdateMovieLink(model);
            }

            return RedirectToAction("MovieLinks", new { id = model.MovieID });
        }

        public ActionResult DeleteMovieLink(int id)
        {
            var model = blogService.GetMovieLink(id);
            blogService.DeleteMovieLink(model);

            return RedirectToAction("MovieLinks", new { id = model.MovieID });
        }

        #endregion

    }
}