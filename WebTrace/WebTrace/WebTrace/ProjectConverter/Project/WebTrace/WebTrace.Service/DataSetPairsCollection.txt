using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TraceLabSDK.Types;

namespace WebTrace.Service
{
    [Serializable]
    public class DataSetPairsCollection : List<DataSetPairs>
    {
        public DataSetPairsCollection() : base() { }

        public string ToOutputString()
        {
            StringBuilder txt = new StringBuilder("                 P    R    AvgP MeanAvgP\n");

            foreach (DataSetPairs metric in this)
            {
                txt.Append(metric.Name);
                for (int i = metric.Name.Length; i <= 16; i++)
                {
                    txt.Append(" ");
                }
                txt.Append(String.Format("{0:0.00}", CalculateAverage(metric.PrecisionData)));
                txt.Append(" ");
                txt.Append(String.Format("{0:0.00}", CalculateAverage(metric.RecallData)));
                txt.Append(" ");
                txt.Append(String.Format("{0:0.00}", CalculateAverage(metric.AveragePrecisionData)));
                txt.Append(" ");
                txt.Append(String.Format("{0:0.00}", CalculateAverage(metric.MeanAveragePrecisionData)));
                txt.AppendLine();
            }

            return txt.ToString();
        }

        public static double CalculateAverage(TLKeyValuePairsList metrics)
        {
            double val = 0.0;
            double non = 0.0;

            foreach (KeyValuePair<string, double> metric in metrics)
            {
                if (!Double.IsNaN(metric.Value))
                    val += metric.Value;
                else
                    non++;
            }

            return val / (metrics.Count - non);
        }
    }
}
