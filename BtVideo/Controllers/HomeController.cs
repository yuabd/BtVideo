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
            
            var pBlogs = new Paginated<Movie>(blogs, page ?? 1, 24);

            //var popularTags = (from p in bs.GetTags()
            //                   group p by new { p.Tag } into t
            //                   orderby t.Count() descending
            //                   select new Anonymous { Tag = t.Key.Tag, Num = t.Count() }).Take(10).ToList();

            var model = new BlogsViewModel(pBlogs, null, null, null);
            ViewBag.PageTitle = "很好记bt";

            return View("~/Views/Movie/Index.cshtml", model);
        }

        [Route("~/about")]
        public ActionResult About()
        {

            return View();
        }
    }
}