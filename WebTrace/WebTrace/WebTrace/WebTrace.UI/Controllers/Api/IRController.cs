using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using WebTrace.Services;

namespace WebTrace.UI.Controllers.Api
{
    public class Martix
    {
        public string SourceFile { get; set; }
        public string TargetFile { get; set; }
        public double Score { get; set; }
    }
    public class IRController : ApiController
    {
        public JsonResult GetSimilarityMartrixes(string path)
        {

            var rawmatrix = IRServices.ComputeSimilarity(path);
            var martix = new List<Martix>();
            if (rawmatrix[0].AllLinks.Count() > 0)
            {
                foreach (var item in rawmatrix[0].AllLinks)
                {
                    martix.Add(new Martix
                    {
                        SourceFile = item.SourceArtifactId,
                        TargetFile = item.TargetArtifactId,
                        Score = item.Score
                    });
                }
            }
            return new JsonResult()
            {
                Data = martix,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}
