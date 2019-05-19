using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebTrace.Services;
using WebTrace.Model;
using System.Threading.Tasks;

namespace WebTrace.UI.Controllers
{
	public class HomeController : Controller
	{
		public async Task<ActionResult> Index()
		{
            //ViewData["SimilartiyScore"] = IRServices.AverageSimilarity();


            return View("Index", await Github.getRepo());
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