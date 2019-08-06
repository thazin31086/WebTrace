using System.IO;
using System.Net;
using System.Web.Http;
using System.Web.Mvc;

namespace WebTrace.UI.Controllers.Api
{
    public class NLPController : ApiController
    {
        public JsonResult GetRoslynPullRequest()
        {
            HttpWebRequest request = WebRequest.Create("https://api.github.com/repos/dotnet/roslyn/pulls?state=closed") as HttpWebRequest;
            request.Method = "GET";
            request.Proxy = null;

            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                return new JsonResult()
                {
                    Data = response,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }
    }
}
