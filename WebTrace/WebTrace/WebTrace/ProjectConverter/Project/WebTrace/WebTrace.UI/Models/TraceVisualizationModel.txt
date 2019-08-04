using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebTrace.UI.Models
{
    public class TraceVisualizationModel
    {
        public Data data { get; set; }
        public Elements elements { get; set; }
    }

    public class Elements
    {
        public List<Node> nodes { get; set; }
        public List<Edge> edges { get; set; }
    }
    public class Data
    {
        public string id { get; set; }
        public string SUID { get; set; }
        public int Quality { get; set; }
        public string FileUrl { get; set; }
        public string functionhtmlformatted { get; set; }
        public bool selected { get; set; }
        public string[] cytoscape_alias_list { get; set; }
        public string[] __Annotations { get; set; }
        public string shared_name { get; set; }
        public string canonicalName { get; set; }
        public string NodeType { get; set; }
        public string name { get; set; }
        public string interaction { get; set; }
        public string shared_interaction { get; set; }
        public string source { get; set; }
        public string target { get; set; }
    }

    public class Position
    {
        public double x { get; set; }
        public double y { get; set; }
    }

    public class Node
    {
        public Data data { get; set; }
        public Position position { get; set; }
        public bool selected { get; set; }
    }

    public class Edge
    {
        public Data data { get; set; }       
        public bool selected { get; set; }
    }
}