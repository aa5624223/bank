using System.Web;
using System.Web.Optimization;

namespace bank2
{
    public class BundleConfig
    {
        // 有关捆绑的详细信息，请访问 https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/umd/popper.js",
                        "~/Scripts/jquery-confirm.min.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/vue").Include(
                      "~/Scripts/vue.js"));

            bundles.Add(new ScriptBundle("~/bundles/datatables").Include(
                        "~/Scripts/jquery.dataTables.min.css",
                        "~/Scripts/dataTables.bootstrap4.min.js"
                      ));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap-treeview").Include(
                "~/Scripts/bootstrap-treeview.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap.bundle").Include(
                      "~/Scripts/bootstrap.bundle.js"));

            bundles.Add(new ScriptBundle("~/bundles/slimscroll").Include(
                      "~/Scripts/slimscroll/slimscroll.min.js",
                      "~/Scripts/slimscroll/custom-scrollbar.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/jquery-confirm.min.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/datatables").Include(
                      "~/Content/dataTables.bootstrap4.min.css"
                      ));
        }
    }
}
