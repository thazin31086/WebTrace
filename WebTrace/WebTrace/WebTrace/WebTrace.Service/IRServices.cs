
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using TraceLab.Components.DevelopmentKit.IO;
using TraceLab.Components.DevelopmentKit.Preprocessors;
using TraceLab.Components.DevelopmentKit.Utils;
using TraceLabSDK.Types;

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
			if(artifacts == null)
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


        public static List<TLSimilarityMatrix> ComputeSimilarity(string path)
        {
            List<TLSimilarityMatrix> results = new List<TLSimilarityMatrix>();
            TLArtifactsCollection sourceArtifacts = Artifacts.ImportDirectory(path + @"/Use Case", "txt");
            TLArtifactsCollection targetArtifact1 = Artifacts.ImportDirectory(path + @"/Source Code", "txt");
            TLArtifactsCollection targetArtifact2 = Artifacts.ImportDirectory(path + @"/Test Case", "txt");
            results.Add(VSM.Compute(StopWordsRemoval(sourceArtifacts), targetArtifact1));
            results.Add(VSM.Compute(StopWordsRemoval(sourceArtifacts), targetArtifact2));
            return results;
        }
    }
}
