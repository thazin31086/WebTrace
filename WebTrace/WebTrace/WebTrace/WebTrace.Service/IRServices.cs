
using System;
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


        public static TermDocumentMatrix TermsMatrix(string path)
        {
            //    TLArtifactsCollection sourceArtifacts = Artifacts.ImportDirectory("C:\\PhD\\WebTrace\\Datasets\\eTour_ENG\\UC", "txt");
            //    TLArtifactsCollection targetArtifacts = Artifacts.ImportDirectory("C:\\PhD\\WebTrace\\Datasets\\eTour_ENG\\CC", "txt");

   
            //string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"eTour_ENG");
            TLArtifactsCollection sourceArtifacts = Artifacts.ImportDirectory(path + @"/UC", "txt");
            TLArtifactsCollection targetArtifacts = Artifacts.ImportDirectory(path + @"/CC", "txt");
            return new TermDocumentMatrix(StopWordsRemoval(sourceArtifacts), StopWordsRemoval(targetArtifacts));

           
        }


    }
}
