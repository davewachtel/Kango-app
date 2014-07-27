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
                        "~/Scripts/bootstrap.min.js",
                        "~/Scripts/plugins/tagsinput/bootstrap-tagsinput.js"));

            bundles.Add(new ScriptBundle("~/bundles/backbone").Include(
                "~/Scripts/underscore.min.js",
                "~/Scripts/backbone.min.js"));

            /* Content */
            bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
                "~/Content/bootstrap.min.css",
                "~/Content/plugins/bootstrap-tagsinput.css",
                "~/Content/cl.css"));

            bundles.Add(new StyleBundle("~/Content/sb-admin").Include(
                "~/Content/sb-admin.css"));

            bundles.Add(new StyleBundle("~/Content/font-awesome").Include(
                "~/font-awesome-4.1.0/css/font-awesome.min.css"));


            bundles.IgnoreList.Clear();
        }
    }
}