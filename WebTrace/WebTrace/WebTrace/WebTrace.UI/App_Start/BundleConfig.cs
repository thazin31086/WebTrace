using System.Web;
using System.Web.Optimization;

namespace WebTrace.UI
{
	public class BundleConfig
	{
		// For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
		public static void RegisterBundles(BundleCollection bundles)
		{


            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqx").Include(                     
                     "~/Scripts/jqx*"));
            bundles.Add(new ScriptBundle("~/bundles/App").Include(
              "~/Scripts/app.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                      "~/Scripts/jquery-{version}-ui.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquerysteps").Include(
                       "~/Scripts/jquery.steps.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
						"~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/vis").Include(
                     "~/Scripts/fastclick.min.js",
                     "~/Scripts/jquery.min.js",
                     "~/Scripts/cytoscape.min.js",
                     "~/Scripts/jquery.qtip.min.js",
                     "~/Scripts/cytoscape-qtip.js",
                     "~/Scripts/bluebird.min.js",
                     "~/Scripts/bootstrap.min.js",
                     "~/Scripts/typeahead.bundle.js",
                     "~/Scripts/handlebars.min.js",
                     "~/Scripts/lodash.min.js",
                     "~/Scripts/demo.js"
                     ));
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
						"~/Scripts/modernizr-*"));

			bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
					  "~/Scripts/bootstrap.js",
					  "~/Scripts/respond.js"));

			bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/font-awesome.css",
                      "~/Content/bootstrap.css",
                      "~/Content/jquery.steps.css",
                      "~/Content/jqx.arctic.css",
                      "~/Content/site.css"));
		}
	}
}
