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

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // 開発と学習には、Modernizr の開発バージョンを使用します。次に、実稼働の準備が
            // 運用の準備が完了したら、https://modernizr.com のビルド ツールを使用し、必要なテストのみを選択します。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/bootstrap-table-expandable.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap_select").Include(
                      "~/Scripts/bootstrap-select.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jstree").Include(
                      "~/Scripts/jstree.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/accordion").Include(
                      "~/Scripts/jquery.accordion.js",
                      "~/Scripts/jquery.easing.1.3.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                      "~/Scripts/jquery-ui.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/star-rating").Include(
                      "~/Scripts/star-rating.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/ckeditor/ckeditor").Include(
                      "~/Scripts/ckeditor/ckeditor.js"));

            bundles.Add(new ScriptBundle("~/bundles/ckeditor/adapters/cleditoradapters").Include(
                      "~/Scripts/ckeditor/adapters/jquery.js"));

            bundles.Add(new ScriptBundle("~/bundles/layout").Include(
                      "~/Scripts/jquery.layout.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-table-expandable.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/bootstrap_select").Include(
                      "~/Content/bootstrap-select.min.css"));

            bundles.Add(new StyleBundle("~/Content/jstree").Include(
                      "~/Content/jstree/style.min.css"));

            bundles.Add(new StyleBundle("~/Content/star-rating").Include(
                      "~/Content/star-rating.min.css"));

            bundles.Add(new StyleBundle("~/Content/accordion").Include(
                      "~/Content/jstree/FlexibleSlideToTopAccordion.css"));

            bundles.Add(new StyleBundle("~/Content/layout").Include(
                      "~/Content/layout-default.css"));

            bundles.Add(new StyleBundle("~/Content/jqueryui").Include(
                      "~/Content/jquery-ui.min.css",
                      "~/Content/jquery-ui.theme.min.css",
                      "~/Content/jquery-ui.structure.min.css"));
        }
    }
}
