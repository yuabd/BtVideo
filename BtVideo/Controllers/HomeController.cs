using BtVideo.Helpers;
using BtVideo.Models;
using BtVideo.Models.Others;
using BtVideo.Models.Site;
using BtVideo.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BtVideo.Controllers
{
    [GlobalFilter]
    public class HomeController : Controller
    {
        private BlogService bs = new BlogService();

        // GET: Home
        public ActionResult Index(int? page)
        {
            var blogs = bs.GetBlogs().Where(m => m.IsPublic == true);
            
            ViewBag.Count = blogs.Select(m => m.MovieID).Count();

            var pBlogs = new Paginated<Movie>(blogs.ToList(), page ?? 1, 8);


            var popularTags = (from p in bs.GetTags()
                               group p by new { p.Tag } into t
                               orderby t.Count() descending
                               select new Anonymous { Tag = t.Key.Tag, Num = t.Count() }).Take(10).ToList();

            //var archives = bs.GetArchives().ToList();

            var model = new BlogsViewModel(pBlogs, null, popularTags, null);
            ViewBag.PageTitle = "很好记bt";
            ViewBag.Blog = "current";

            return View("~/Views/Movie/Index.cshtml", model);
        }

        [Route("~/about")]
        public ActionResult About()
        {

            return View();
        }
    }
}