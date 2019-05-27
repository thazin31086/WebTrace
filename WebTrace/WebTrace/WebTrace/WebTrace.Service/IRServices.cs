
using System;
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


        public static TermDocumentMatrix TermsMatrix()
        {
            TLArtifactsCollection sourceArtifacts = Artifacts.ImportDirectory("C:\\PhD\\WebTrace\\Datasets\\eTour_ENG\\UC", "txt");
            TLArtifactsCollection targetArtifacts = Artifacts.ImportDirectory("C:\\PhD\\WebTrace\\Datasets\\eTour_ENG\\CC", "txt");
           
            return new TermDocumentMatrix(StopWordsRemoval(sourceArtifacts), StopWordsRemoval(targetArtifacts));

           
        }


    }
}
