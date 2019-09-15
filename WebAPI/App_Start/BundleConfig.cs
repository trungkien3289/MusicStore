using System.Web;
using System.Web.Optimization;

namespace WebAPI
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/Common/popper.min.js",
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/Common/icons.css",
                      "~/Content/Common/style.css",
                        "~/Content/Common/font-awesome.css",
                        "~/Content/main-menu.css"));

            bundles.Add(new StyleBundle("~/Content/dashboard_css").Include(
                      //"~/Content/bootstrap.css",
                      //"~/Content/Common/icons.css",
                      //  "~/Content/Common/font-awesome.css",
                        "~/Content/Dashboard/layout.css",
                        "~/node_modules/croppie/croppie.css"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/dashboard_js").Include(

                      "~/Scripts/DashBoard/dashboard-main.js",
                      //"~/node_modules/knockout.validation/dist/knockout.validation.js",
                      //"~/node_modules/knockout.validation/localization/el-GR.js",
                      "~/Scripts/Libraries/knockout-3.5.0.js",
                      "~/node_modules/croppie/croppie.min.js"
                      ));
        }
    }
}

