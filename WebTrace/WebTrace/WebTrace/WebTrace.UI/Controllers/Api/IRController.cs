using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using WebTrace.Services;
using WebTrace.UI.Models;

namespace WebTrace.UI.Controllers.Api
{
    public class Martix
    {
        public string SourceFile { get; set; }
        public string TargetFile { get; set; }
        public double Score { get; set; }
    }

    public class MartixList
    {
        /*Source*/
        public int id { get; set; }
        public string FileName { get; set; }
        public string FileContent { get; set; }
        public double? Score { get; set; }

        /*Target*/
        public List<MartixList> Targets { get; set; }
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

        public JsonResult GetData()
        {
            var path = System.Web.Hosting.HostingEnvironment.MapPath("~/Data/EasyClinic"); 
            var _data = new TraceVisualizationModel();
            _data.data = new Data();
            _data.data.selected = true;
            _data.data.shared_name = "WebTrace";
            _data.data.SUID = "1";
            _data.data.name = "WebTrace";
            _data.elements = new Elements();

            /*Create Edges*/
            var rawmatrix = IRServices.ComputeSimilarity(path);
            var martix = new List<Martix>();
            if (rawmatrix[0].AllLinks.Count() > 0)
            {
                _data.elements.edges = new List<Edge>();
                int i = 1;
                foreach (var martrix in rawmatrix)
                {
                    foreach (var item in martrix.AllLinks.Where(x => x.Score >= 0.15))
                    {
                        var _edge = new Edge();
                        _edge.selected = false;
                        _edge.data = new Data
                        {
                            id = i.ToString(),
                            SUID = i.ToString(),
                            source = item.SourceArtifactId,
                            target = item.TargetArtifactId,
                            canonicalName = item.TargetArtifactId,
                            name = item.TargetArtifactId,
                            interaction = item.TargetArtifactId.Contains("Use") ? "cc" :
                                          item.TargetArtifactId.Contains("Test") ? "cw" :
                                          item.TargetArtifactId.Contains("Code") ? "cr" : "cc",
                            shared_interaction = item.TargetArtifactId.Contains("Use") ? "cc" :
                                          item.TargetArtifactId.Contains("Test") ? "cw" :
                                          item.TargetArtifactId.Contains("Code") ? "cr" : "cc",
                            shared_name = item.TargetArtifactId,                   
                        };
                        _data.elements.edges.Add(_edge);
                        i++;
                    }
                }
            }

            var FunctionMap = IRServices.TermsMatrixRaw(path)?.FunctionMap;

            /*Create Node*/
            var matrix = IRServices.TermsMatrix(path);
            if (matrix.DocIndexLookup != null)
            {
                _data.elements.nodes = new List<Node>();
                Random rnd = new Random();
                Random rnd2 = new Random();

                int xu = 1000;
                int yu = 1000;
                int xt = 500;
                int yt = 1500;
                int xs = 2000;
                int ys = 1500;
                foreach (var doc in matrix.DocIndexLookup)
                {                    
                    var _node = new Node();
                    if (doc.Key.Contains("Use"))
                    {
                        _node.selected = false;
                        _node.position = new Position
                        {
                            x = xu,
                            y = yu,
                        };
                        xu += 5;
                        yu += 5;
                    }
                    else if (doc.Key.Contains("Test"))
                    {
                        _node.selected = false;
                        _node.position = new Position
                        {
                            x = xt,
                            y = yt,
                        };
                        xt += 5;
                        yt += 5;
                    }
                    else
                    {
                        _node.selected = false;
                        _node.position = new Position
                        {
                            x = xs,
                            y = ys,
                        };
                        xs += 5;
                        ys += 5;
                    }

                    string functionshtmlformat = "";
                    var _filefolder=  doc.Key.Contains("Use") ? "Use Case" :
                                                doc.Key.Contains("Test") ? "Test Case" : "Source Code";
                    var funcLists = FunctionMap.Where(z => z.DocumentIndex == doc.Key.ToString()).Select(x => x.FunctionName).ToList();

                    if (funcLists != null && funcLists.Count() > 0)
                    {                           
                        foreach (var fun in funcLists)
                        { 
                            functionshtmlformat += WebUtility.HtmlEncode(fun + "()" + ",");
                        }                
                    }

                    string hostUrl = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host;
                    string fileurl = hostUrl + "/Data/EasyClinic/" + _filefolder + "/" + doc.Key + ".txt";
                    _node.data = new Data
                    {
                        id = doc.Key,
                        selected = false,
                        cytoscape_alias_list = new string[] { doc.Key },
                        name = doc.Key,
                        shared_name = doc.Key,
                        SUID = doc.Key,
                        NodeType = doc.Key.Contains("Use") ? "UseCase" :
                                    doc.Key.Contains("Test") ? "TestCase" : "Code",
                        Quality =  (doc.Key.Contains("Use") || doc.Key.Contains("Test")) 
                                        ? rnd2.Next(20, 100)
                                        :(funcLists.Count() / FunctionMap.Count() * 100),
                        FileUrl = WebUtility.UrlEncode(fileurl) ,
                        functionhtmlformatted = functionshtmlformat,
                    };
                    _data.elements.nodes.Add(_node);
                }
            }
                       
            return new JsonResult()
            {
                Data = _data,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}
