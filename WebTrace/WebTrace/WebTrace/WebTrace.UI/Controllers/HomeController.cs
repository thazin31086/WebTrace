using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebTrace.Services;

namespace WebTrace.UI.Controllers
{
	public class HomeController : Controller
	{
        //public async Task<ActionResult> Index()
        //{
        //          var test = IRServices.TermsMatrix();
        //          ViewData["SimilartiyScore"] = IRServices.AverageSimilarity();


        //          return View("Index", await Github.getRepo());
        //}

        public ActionResult Index()
        {
          
           
            return View("Index");
        }

        public ActionResult Workbench()
        {
            ///var similarity = IRServices.AverageSimilarity();

            string path = Server.MapPath("~/Data/eTour_ENG");
            var matrix = IRServices.TermsMatrix(path);
            ViewData["TermMap"] = matrix.TermMap;
            ViewData["RawMatrix"] = matrix.RawMatrix;
            ViewData["DocMap"] = matrix.DocMap;
            ViewData["TermIndexLookup"] = matrix.TermIndexLookup;
            ViewData["DocIndexLookup"] = matrix.DocIndexLookup;

            return View("Workbench");
        }
        public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
	}
}