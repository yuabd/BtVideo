using BtVideo.Helpers;
using BtVideo.Models;
using BtVideo.Models.Others;
using BtVideo.Models.Site;
using BtVideo.Services;
using System.Linq;
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

            var model = new BlogsViewModel(pBlogs, null, null, null);
            ViewBag.PageTitle = "很好记bt";

            if (page.HasValue)
            {
                ViewBag.PageTitle += "_第" + page.Value + "页";
            }

            return View("~/Views/Movie/Index.cshtml", model);
        }

        [Route("~/about")]
        public ActionResult About()
        {

            return View();
        }
    }
}