using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
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
            ViewData["datafolders"] = PopulateTreeView();
            string path = Server.MapPath("~/Data/EasyClinic");
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
			ViewBag.Message = "Webtrace allow to trace between more than two artifacts at a times based on textual similarity between them.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}

        #region Methods
        private TreeNode PopulateTreeView()
        {
            TreeNode rootNode;

            DirectoryInfo info = new DirectoryInfo(Server.MapPath("~/Data"));
            if (info.Exists)
            {
                rootNode = new TreeNode(info.Name);
                GetDirectories(info.GetDirectories(), ref rootNode);
                return rootNode;
            }

            return null;
        }

        private void GetDirectories(DirectoryInfo[] subDirs,
            ref TreeNode nodeToAddTo)
        {
            TreeNode aNode;
            DirectoryInfo[] subSubDirs;
            foreach (DirectoryInfo subDir in subDirs)
            {
                aNode = new TreeNode(subDir.Name);              
                subSubDirs = subDir.GetDirectories();
                if (subSubDirs.Length != 0)
                {
                    GetDirectories(subSubDirs, ref aNode);
                }
                nodeToAddTo.ChildNodes.Add(aNode);
            }
        }

        #endregion
    }
}