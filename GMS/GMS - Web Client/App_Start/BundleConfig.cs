using System.Web.Optimization;

namespace GMS___Web_Client
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap-bundle").Include(
                      "~/Scripts/bootstrap.bundle.js"));

            bundles.Add(new ScriptBundle("~/bundles/popper").Include(
                    "~/Scripts/umd/popper.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));


            // Bundles for datepicker using JQuery
            bundles.Add(new ScriptBundle("~/bundles/jquery-ui").Include(
                        /*"~/Scripts/jquery-3.5.1.min.js",*/
                        "~/Scripts/jquery-ui-1.12.1.min.js"));

            bundles.Add(new StyleBundle("~/Content/css/jquery-ui").Include(
                      "~/Content/themes/base/jquery-ui.min.css"));

            // Bundles for tiempicker using JQuery
            bundles.Add(new ScriptBundle("~/bundles/jquery-ui-timepicker-addon").Include(
                        "~/Scripts/jquery-ui-timepicker-addon.min.js"));

            bundles.Add(new StyleBundle("~/Content/css/jquery-ui-timepicker-addon").Include(
                      "~/Content/jquery-ui-timepicker-addon.min.css"));

            // Bundles for fontawesome
            bundles.Add(new StyleBundle("~/Content/css/fontawesome").Include(
                          "~/Content/bootstrap.css",
                          "~/Content/ekimba.css",
                          "~/Content/w3.css",
                          "~/Content/font-awesome.min.css",
                          "~/Content/site.css"));

            // Bundles for select2
            bundles.Add(new StyleBundle("~/Content/css/select2").Include(
                          "~/Content/css/select2.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/select2").Include(
                        /*"~/Scripts/jquery-3.5.1.min.js", */
                        "~/Scripts/select2.min.js"));
        }
    }
}
