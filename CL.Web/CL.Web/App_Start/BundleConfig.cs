using System.Web;
using System.Web.Optimization;

namespace CL.Web
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/plugins/tagsinput/bootstrap-tagsinput.js"));

            bundles.Add(new ScriptBundle("~/bundles/backbone").Include(
                "~/Scripts/underscore.js",
                //"~/Scripts/underscore-min.map",
                "~/Scripts/backbone.js"
                //,"~/Scripts/backbone.min.map"
                ));

            /* Content */
            bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
                "~/Content/bootstrap.css",
                "~/Content/plugins/bootstrap-tagsinput.css",
                "~/Content/cl.css"));

            bundles.Add(new StyleBundle("~/Content/sb-admin").Include(
                "~/Content/sb-admin.css"));

            bundles.Add(new StyleBundle("~/Content/font-awesome").Include(
                "~/font-awesome-4.1.0/css/font-awesome.css"));


            bundles.IgnoreList.Clear();
        }
    }
}