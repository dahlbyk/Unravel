using System.Web.Optimization;

namespace UnravelExamples.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/Site.css"));

            bundles.Add(new StyleBundle("~/Content/home").Include(
                      "~/Content/Home.css"));
        }
    }
}
