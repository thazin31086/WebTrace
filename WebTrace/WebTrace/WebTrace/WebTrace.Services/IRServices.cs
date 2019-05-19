
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
			TLArtifactsCollection sourceArtifacts = Artifacts.ImportDirectory("C:\\Jasmine\\Study\\TraceLab-master\\TraceLab-master\\Main\\ComponentsSources\\SEMERU-WM\\1.0.0.0\\SEMERU.Datasets\\eTour_ITA\\UC", "txt");
			TLArtifactsCollection targetArtifacts = Artifacts.ImportDirectory("C:\\Jasmine\\Study\\TraceLab-master\\TraceLab-master\\Main\\ComponentsSources\\SEMERU-WM\\1.0.0.0\\SEMERU.Datasets\\eTour_ITA\\CC", "txt");


			TLSimilarityMatrix sims = TraceLab.Components.DevelopmentKit.Tracers.InformationRetrieval.VSM.Compute(StopWordsRemoval(sourceArtifacts), StopWordsRemoval(targetArtifacts), TraceLab.Components.DevelopmentKit.Tracers.InformationRetrieval.VSMWeightEnum.TFIDF);


			var result = TLSimilarityMatrixUtil.AverageSimilarity(sims);
			return result;
		}

		public static TLArtifactsCollection StopWordsRemoval(TLArtifactsCollection artifacts)
		{
			if(artifacts == null)
				return null;
			return SimpleStopwordsRemover.ProcessArtifacts(artifacts, 3, false);

		}

	}
}
