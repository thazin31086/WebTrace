using Octokit;
using Octokit.Internal;
using Octokit.Reactive;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using WebTrace.DB;
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
            WebTraceDB result = new WebTraceDB();

            return View("Index");
        }

        public ActionResult Workbench()
        {
            return View("Workbench");
        }

        public ActionResult HRTracing()
        {           
            string path = Server.MapPath("~/Data/EasyClinic");
            ViewData["datafolders"] = PopulateTreeView(path);
            var rawmatrix = IRServices.TermsMatrixRaw(path);
            ViewData["RawTermMap"] = rawmatrix.TermMap;
            var matrix = IRServices.TermsMatrix(path);
            ViewData["TermMap"] = matrix.TermMap;
            ViewData["RawMatrix"] = matrix.RawMatrix;
            ViewData["DocMap"] = matrix.DocMap;
            ViewData["TermIndexLookup"] = matrix.TermIndexLookup;
            ViewData["DocIndexLookup"] = matrix.DocIndexLookup;
            ViewData["FunctionMap"] = matrix.FunctionMap;
            var rankedlist = IRServices.ComputeSimilarity(path, 10, false, false);
            ViewData["rankedlist"] = rankedlist;
            return View("HRTracing");
        }

        public ActionResult HRTracingChunk()
        {
            string path = Server.MapPath("~/Data/EasyClinic_");
            ViewData["datafolders"] = PopulateTreeView(path);
            var rawmatrix = IRServices.TermsMatrixRaw(path);
            ViewData["RawTermMap"] = rawmatrix.TermMap;
            var matrix = IRServices.TermsMatrix(path);
            ViewData["TermMap"] = matrix.TermMap;
            ViewData["RawMatrix"] = matrix.RawMatrix;
            ViewData["DocMap"] = matrix.DocMap;
            ViewData["TermIndexLookup"] = matrix.TermIndexLookup;
            ViewData["DocIndexLookup"] = matrix.DocIndexLookup;
            ViewData["FunctionMap"] = matrix.FunctionMap;
            var rankedlist = IRServices.ComputeSimilarity(path, 10, false, false);
            ViewData["rankedlist"] = rankedlist;
            return View("HRTracingChunk");
        }
        public ActionResult About()
		{
			ViewBag.Message = "Webtrace allow to trace between more than two artifacts at a times based on textual similarity between them.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "PhD Student at University of Technology Sydney";

			return View();
		}

        public ActionResult VisualizeTraceLinks()
        {
            ViewBag.Title = "Visualize Trace Link";
            return View();
        }

        public ActionResult ConnectGithub()
        {           
            return View();
        }

        public ActionResult GetRoslynIssues()
        {
            string owner = "github";
            string name = "gitignore";

            InMemoryCredentialStore credentials = new InMemoryCredentialStore(new Credentials("cf2001d7867d138126530157cb7cc07bd21364c8"));
            ObservableGitHubClient client = new ObservableGitHubClient(new ProductHeaderValue("ophion"), credentials);

            var request = new PullRequestRequest();
            var results = new Dictionary<string, List<int>>();

            // fetch all open pull requests
            client.PullRequest.GetAllForRepository(owner, name, request)
                .SelectMany(pr =>
                {
                    // for each pull request, get the files and associate
                    // with the pull request number - this is a bit kludgey
                    return client.PullRequest.Files(owner, name, pr.Number)
                            .Select(file => Tuple.Create(pr.Number, file.FileName));
                })
                .Subscribe(data =>
                {
                    if (results.ContainsKey(data.Item2))
                    {
                        // if we're already tracking this file, add it
                        results[data.Item2].Add(data.Item1);
                    }
                    else
                    {
                        // otherwise, we create a new list
                        var list = new List<int> { data.Item1 };
                        results[data.Item2] = list;
                    }
                },
                    ex =>
                    {
                        Console.WriteLine("Exception found: " + ex.ToString());
                    },
                    () =>
                    {
                        // after that's done, let's sort by the most popular files
                        var sortbyPopularity = results
                                .OrderByDescending(kvp => kvp.Value.Count);

                        // output each file with the related pull requests
                        foreach (var file in sortbyPopularity)
                        {
                            Console.WriteLine("File: {0}", file.Key);

                            foreach (var id in file.Value)
                            {
                                Console.WriteLine(" - PR: {0}", id);
                            }

                            Console.WriteLine();
                        }
                    });

            // this will take a while, let's wait for user input before exiting
            Console.ReadLine();


            return View();
        }

        #region Methods
        private TreeNode PopulateTreeView(string path)
        {
            TreeNode rootNode;

            DirectoryInfo info = new DirectoryInfo(path);
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