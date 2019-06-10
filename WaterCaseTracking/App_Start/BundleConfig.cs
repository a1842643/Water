using System.Web;
using System.Web.Optimization;

namespace WaterCaseTracking
{
    public class BundleConfig
    {
        // 如需統合的詳細資訊，請瀏覽 https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                        "~/Content/jquery/dist/jquery.min.js",
                        "~/Content/bootstrap/dist/js/bootstrap.min.js",
                        "~/Content/dist/js/adminlte.min.js",
                        "~/Content/datatables.net/js/jquery.dataTables.min.js",
                        "~/Content/datatables.net-bs/js/dataTables.bootstrap.min.js",
                        "~/Content/jquery-slimscroll/jquery.slimscroll.min.js",
                        "~/Scripts/ckeditor/ckeditor.js"
                        ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap/dist/css/bootstrap.min.css",
                      "~/Content/font-awesome/css/font-awesome.min.css",
                      "~/Content/Ionicons/css/ionicons.min.css",
                      "~/Content/dist/css/AdminLTE.min.css",
                      "~/Content/dist/css/skins/skin-blue.min.css",
                       "~/Content/datatables.net-bs/css/dataTables.bootstrap.min.css"
                       ));
            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include("~/Scripts/jquery-ui-1.12.1.js"));
            bundles.Add(new StyleBundle("~/Content/jqueryui").Include("~/Content/themes/base/all.css"));
        }
    }
}
