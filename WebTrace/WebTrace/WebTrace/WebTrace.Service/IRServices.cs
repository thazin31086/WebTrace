
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using TraceLab.Components.DevelopmentKit.IO;
using TraceLab.Components.DevelopmentKit.Preprocessors;
using TraceLab.Components.DevelopmentKit.Utils;
using TraceLabSDK.Types;
using System.Linq;
using WebTrace.Model;

namespace WebTrace.Services
{
    public static class IRServices
    {
        public static double AverageSimilarity()
        {
            TLArtifactsCollection sourceArtifacts = Artifacts.ImportDirectory("C:\\PhD\\WebTrace\\Datasets\\eTour_ENG\\UC", "txt");
            TLArtifactsCollection targetArtifacts = Artifacts.ImportDirectory("C:\\PhD\\WebTrace\\Datasets\\eTour_ENG\\CC", "txt");

            TLSimilarityMatrix sims = WebTrace.Services.VSM.Compute(StopWordsRemoval(sourceArtifacts), StopWordsRemoval(targetArtifacts));

            var result = TLSimilarityMatrixUtil.AverageSimilarity(sims);
            return result;
        }


        public static TLArtifactsCollection StopWordsRemoval(TLArtifactsCollection artifacts)
        {
            if (artifacts == null)
                return null;
            return SimpleStopwordsRemover.ProcessArtifacts(artifacts, 3, false);

        }

        public static TermDocumentMatrix TermsMatrixRaw(string path)
        {

            TLArtifactsCollection sourceArtifacts = Artifacts.ImportDirectory(path + @"/Use Case", "txt");
            TLArtifactsCollection targetArtifacts = Artifacts.ImportDirectory(path + @"/Source Code", "txt");
            return new TermDocumentMatrix(sourceArtifacts, targetArtifacts);
        }



        public static TermDocumentMatrix TermsMatrix(string path)
        {
            TLArtifactsCollection sourceArtifacts = Artifacts.ImportDirectory(path + @"/Use Case", "txt");
            TLArtifactsCollection targetArtifact1 = Artifacts.ImportDirectory(path + @"/Source Code", "txt");
            TLArtifactsCollection targetArtifact2 = Artifacts.ImportDirectory(path + @"/Test Case", "txt");
            return new TermDocumentMatrix(StopWordsRemoval(sourceArtifacts), targetArtifact1, targetArtifact2);
        }


        public static VizResults ComputeSimilarity(string path, double threshold, bool excludeSC, bool excludeTC)
        {
            VizResults vizresults = new VizResults();
            List<TLSimilarityMatrix> results = new List<TLSimilarityMatrix>();
            TLArtifactsCollection sourceArtifacts = Artifacts.ImportDirectory(path + @"/Use Case", "txt");
            var source = StopWordsRemoval(sourceArtifacts);
            if (excludeTC) /*Exclude Test Case*/
            {
                TLArtifactsCollection targetArtifact1 = Artifacts.ImportDirectory(path + @"/Source Code", "txt");              
                var sourcevstarget1collection = VSM.Compute(source, targetArtifact1);               
                results.Add(VSM.Compute(StopWordsRemoval(sourceArtifacts), targetArtifact1));               
                vizresults.results = results;
                vizresults.termmatrix = new TermDocumentMatrix(source, targetArtifact1);
            }
            else if (excludeSC)/*Exclude Source Code*/
            {
                TLArtifactsCollection targetArtifact1 = Artifacts.ImportDirectory(path + @"/Test Case", "txt");
                var sourcevstarget1collection = VSM.Compute(source, targetArtifact1);
                results.Add(VSM.Compute(StopWordsRemoval(sourceArtifacts), targetArtifact1));
                vizresults.results = results;
                vizresults.termmatrix = new TermDocumentMatrix(source, targetArtifact1);
            }
            else /*ALL*/
            {
                TLArtifactsCollection targetArtifact1 = Artifacts.ImportDirectory(path + @"/Source Code", "txt");
                TLArtifactsCollection targetArtifact2 = Artifacts.ImportDirectory(path + @"/Test Case", "txt");
                var sourcevstarget1collection = VSM.Compute(source, targetArtifact1);
                var sourcevstarget2collection = VSM.Compute(source, targetArtifact2);
                results.Add(VSM.Compute(StopWordsRemoval(sourceArtifacts), targetArtifact1));
                results.Add(VSM.Compute(StopWordsRemoval(sourceArtifacts), targetArtifact2));
                vizresults.results = results;
                vizresults.termmatrix = new TermDocumentMatrix(source, targetArtifact1, targetArtifact2);
            }

            //#region "Nearest Neighbours Calculation Part (Rule - if two sources are pointing to same target, they are nearest neighbours)"
            //TLSimilarityMatrix sourcevssource = new TLSimilarityMatrix();
            //foreach (var st1item in sourcevstarget1collection.AllLinks)
            //{
            //    if (st1item.Score >= threshold)
            //    {
            //        foreach (var st1item2 in sourcevstarget1collection.AllLinks)
            //        {
            //            //Two Source Common Target Case
            //            if (st1item.TargetArtifactId == st1item2.TargetArtifactId
            //                && st1item.SourceArtifactId != st1item2.SourceArtifactId)
            //            {
            //                bool linkexist = false;
            //                if (sourcevssource != null)
            //                {
            //                    linkexist = sourcevssource.AllLinks.Contains(new TLSingleLink(st1item.SourceArtifactId, st1item2.SourceArtifactId, 1));
            //                }

            //                if (!linkexist)
            //                {
            //                    sourcevssource.AddLink(st1item.SourceArtifactId, st1item2.SourceArtifactId, 1);
            //                }
            //            }
            //        }
            //    }
            //}
            //foreach (var st1item in sourcevstarget2collection.AllLinks)
            //{
            //    if (st1item.Score >= threshold)
            //    {
            //        foreach (var st1item2 in sourcevstarget2collection.AllLinks)
            //        {
            //            //Two Source Common Target Case 
            //            if (st1item.TargetArtifactId == st1item2.TargetArtifactId
            //                && st1item.SourceArtifactId != st1item2.SourceArtifactId)
            //            {
            //                bool linkexist = false;
            //                if (sourcevssource != null)
            //                {
            //                    linkexist = sourcevssource.AllLinks.Contains(new TLSingleLink(st1item.SourceArtifactId, st1item2.SourceArtifactId, 1));
            //                }

            //                if (!linkexist)
            //                {
            //                    sourcevssource.AddLink(st1item.SourceArtifactId, st1item2.SourceArtifactId, 1);
            //                }
            //            }
            //        }
            //    }
            //}
            //#endregion "Nearest Neighbours Calculation Part"
            //results.Add(sourcevssource);            
            return vizresults;
        }
    }
}
