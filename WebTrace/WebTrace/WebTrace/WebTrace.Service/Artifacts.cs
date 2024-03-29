﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using TraceLabSDK.Types;

namespace WebTrace.Service
{
    public static class Artifacts
    {
        /// <summary>
        /// Imports a SEMERU corpus in the form (each line):
        /// ID TEXT TEXT TEXT TEXT TEXT ...
        /// </summary>
        /// <param name="filename">Corpus file location</param>
        /// <returns>Artifacts collection</returns>
        public static TLArtifactsCollection Import(string filename)
        {
            StreamReader file = new StreamReader(filename);
            TLArtifactsCollection answer = new TLArtifactsCollection();
            String line;
            while ((line = file.ReadLine()) != null)
            {
                List<string> artifacts = new List<string>(line.Split());
                String id = artifacts[0].Trim();
                artifacts.RemoveAt(0);
                String doc = String.Join(" ", artifacts);
                answer.Add(new TLArtifact(id, doc));
            }
            return answer;
        }

        /// <summary>
        /// Imports a SEMERU corpus in the form (each line):
        /// ID TEXT TEXT TEXT TEXT TEXT ...
        /// Stores a mapping to the IDs in the order they were read in.
        /// </summary>
        /// <param name="filename">Corpus file location</param>
        /// <param name="map">ID mapping</param>
        /// <returns>Artifacts collection</returns>
        public static TLArtifactsCollection Import(string filename, out List<string> map)
        {
            map = new List<string>();
            StreamReader file = new StreamReader(filename);
            TLArtifactsCollection answer = new TLArtifactsCollection();
            String line;
            while ((line = file.ReadLine()) != null)
            {
                List<string> artifacts = new List<string>(line.Split());
                String id = artifacts[0].Trim();
                artifacts.RemoveAt(0);
                String doc = String.Join(" ", artifacts);
                answer.Add(new TLArtifact(id, doc));
                map.Add(id);
            }
            return answer;
        }

        /// <summary>
        /// Imports a corpus from a directory containing artifacts files.
        /// </summary>
        /// <param name="path">Directory path</param>
        /// <returns>Artifacts collection</returns>
        public static TLArtifactsCollection ImportDirectory(string path, string filter)
        {
            TLArtifactsCollection artifacts = new TLArtifactsCollection();

            if (String.IsNullOrWhiteSpace(filter))
            {
                foreach (string file in Directory.GetFiles(path))
                {
                    artifacts.Add(new TLArtifact(Path.GetFileName(file), File.ReadAllText(file)));
                }
            }
            else
            {
                foreach (string file in Directory.GetFiles(path, "*." + filter))
                {
                    artifacts.Add(new TLArtifact(Path.GetFileNameWithoutExtension(file), File.ReadAllText(file)));
                }
            }

            return artifacts;
        }

        /// <summary>
        /// Imports INFORMATION about corpora from an XML file.
        /// For use with a loop importer.
        /// </summary>
        /// <param name="XMLfile">File location object</param>
        /// <returns>List of dataset information</returns>
        public static List<DataSet> ImportDatasets(FilePath XMLfile)
        {
            XmlDocument XMLdoc = new XmlDocument();
            XMLdoc.Load(XMLfile.Absolute);

            string[] s = XMLfile.Absolute.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder path = new StringBuilder();
            for (long i = 0; i < s.Length - 1; i++)
            {
                path.Append(s[i]);
                path.Append("\\");
            }
            path.Append(XMLdoc.GetElementsByTagName("Datasets")[0].Attributes["Directory"].Value.Replace('/', '\\'));

            List<DataSet> datasets = new List<DataSet>();
            XmlNodeList nodelist = XMLdoc.GetElementsByTagName("Dataset");

            foreach (XmlNode node in nodelist)
            {
                DataSet dataset = new DataSet();
                dataset.SourceArtifacts = path + node["Corpus"]["SourceArtifacts"].InnerText;
                dataset.TargetArtifacts = path + node["Corpus"]["TargetArtifacts"].InnerText;
                dataset.Oracle = path + node["Corpus"]["Oracle"].InnerText;
                dataset.Name = node.Attributes["Name"].Value;
                datasets.Add(dataset);
            }

            return datasets;
        }
    }
}
