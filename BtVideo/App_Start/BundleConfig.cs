using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Security;

namespace BtVideo.App_Start
{
    public class AsDefinedBundleOrderer : IBundleOrderer
    {
        public IEnumerable<BundleFile> OrderFiles(BundleContext context, IEnumerable<BundleFile> files)
        {
            //throw new NotImplementedException();
            return files;
        }
    }

    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundles/bootstrap").Include(
                "~/App_Themes/home/css/bootstrap.min.css",
                "~/App_Themes/home/css/style.css")
                );

            var bundle = new ScriptBundle("~/bundles/jquery").Include(
                "~/App_Themes/home/scripts/jquery-{version}.js",
                "~/App_Themes/home/scripts/bootstrap.min.js",
                "~/App_Themes/home/scripts/bootstrap.ediblog.slidemenu.min.js",
                "~/Scripts/jquery.validate.min.js",
                "~/Scripts/jquery.validate.unobtrusive.min.js",
                "~/App_Themes/home/scripts/jquery.unobtrusive-ajax.min.js"
                );
            bundle.Orderer = new AsDefinedBundleOrderer();

            bundles.Add(bundle);

            //post detail
            bundles.Add(new StyleBundle("~/bundles/postcss").Include(
                //"~/Scripts/SintaxHighlighter/sh.css"
                ));

            bundles.Add(new ScriptBundle("~/bundles/post").Include(
                //"~/Scripts/SintaxHighlighter/sh.min.js",
                "~/App_Themes/home/scripts/jquery.lazyload.min.js"
                //"~/Plugins/wordCount/readingTime.js"
                ));
        }
    }
}