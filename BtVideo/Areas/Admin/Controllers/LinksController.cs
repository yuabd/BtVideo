using BtVideo.Models;
using BtVideo.Models.Others;
using BtVideo.Services;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Linq;

namespace BtVideo.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class LinksController : Controller
    {
        private SiteService siteService = new SiteService();

        //
        // GET: /Admin/Links/

        public ActionResult Index(int? page)
        {
            var links = siteService.GetLinks();
            var model = new Paginated<Links>(links, page ?? 1, 20);

            return View(model);
        }

        public ActionResult Add()
        {
            var link = new Links();
            link.SortOrder = 0;

            return View(link);
        }

        [HttpPost]
        public ActionResult Add(Links links)
        {
            if (ModelState.IsValid)
            {
                var obj = siteService.InsertLink(links);
                if (obj.Tag <= 0)
                {
                    ViewBag.Error = obj.Message;
                    return View(links);
                }

                return RedirectToAction("Index");
            }

            return View(links);
        }

        public ActionResult Edit(int id)
        {
            var link = siteService.GetLink(id);

            return View(link);
        }

        [HttpPost]
        public ActionResult Edit(Links links)
        {
            if (ModelState.IsValid)
            {
                var obj = siteService.UpdateLink(links);
                if (obj.Tag <= 0)
                {
                    ViewBag.Error = obj.Message;
                    return View(links);
                }

                return RedirectToAction("Index");
            }

            return View(links);
        }

        public ActionResult Delete(int id)
        {
            var obj = siteService.DeleteLink(id);
            if (obj.Tag <= 0)
            {
                ViewBag.Error = obj.Message;
            }
            return RedirectToAction("Index");
        }
    }
}