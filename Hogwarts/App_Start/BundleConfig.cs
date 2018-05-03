using System.Web;
using System.Web.Optimization;

namespace Hogwarts
{
    public class BundleConfig
    {
        // バンドルの詳細については、https://go.microsoft.com/fwlink/?LinkId=301862 を参照してください
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/bootstrap-table-expandable.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            //bundles.Add(new ScriptBundle("~/bundles/umd/popper").Include(
            //          "~/Scripts/umd/popper.js"));

            bundles.Add(new ScriptBundle("~/bundles/polyfill").Include(
                      "~/Scripts/dialog-polyfill.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap_select").Include(
                      "~/Scripts/bootstrap-select.min.js"));

            //bundles.Add(new ScriptBundle("~/bundles/popper").Include(
            //          "~/Scripts/popper.js"));

            bundles.Add(new ScriptBundle("~/bundles/accordion").Include(
                      "~/Scripts/jquery.accordion.js",
                      "~/Scripts/jquery.easing.1.3.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                      "~/Scripts/jquery-ui.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/star-rating").Include(
                      "~/Scripts/star-rating.min.js"));


            bundles.Add(new ScriptBundle("~/bundles/ckeditor/adapters/ckeditoradapters").Include(
                      "~/Scripts/ckeditor/adapters/jquery.js"));

            bundles.Add(new ScriptBundle("~/bundles/ckeditor/ckeditor").Include(
                      "~/Scripts/ckeditor/ckeditor.js"));

            bundles.Add(new ScriptBundle("~/bundles/material").Include(
                      "~/Scripts/material.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-table-expandable.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/bootstrap_select").Include(
                      "~/Content/bootstrap-select.min.css"));

            bundles.Add(new StyleBundle("~/Content/star-rating").Include(
                      "~/Content/star-rating.min.css"));

            bundles.Add(new StyleBundle("~/Content/polyfill").Include(
                      "~/Content/dialog-polyfill.css"));

            bundles.Add(new StyleBundle("~/Content/accordion").Include(
                      "~/Content/jstree/FlexibleSlideToTopAccordion.css"));

            bundles.Add(new StyleBundle("~/Content/jqueryui").Include(
                      "~/Content/jquery-ui.min.css",
                      "~/Content/jquery-ui.theme.min.css",
                      "~/Content/jquery-ui.structure.min.css"));

            bundles.Add(new StyleBundle("~/Content/material").Include(
                      "~/Content/material.min.css"));

            BundleTable.EnableOptimizations = true;
        }
    }
}
