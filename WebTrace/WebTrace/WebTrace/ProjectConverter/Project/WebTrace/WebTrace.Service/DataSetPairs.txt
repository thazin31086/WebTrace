using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TraceLabSDK.Types;

namespace WebTrace.Service
{
    [Serializable]
    public class DataSetPairs
    {
        public string Name;
        public TLKeyValuePairsList RecallData;
        public TLKeyValuePairsList PrecisionData;
        public TLKeyValuePairsList AveragePrecisionData;
        public TLKeyValuePairsList MeanAveragePrecisionData;

        public DataSetPairs()
        {
            Name = String.Empty;
            PrecisionData = new TLKeyValuePairsList();
            RecallData = new TLKeyValuePairsList();
            AveragePrecisionData = new TLKeyValuePairsList();
            MeanAveragePrecisionData = new TLKeyValuePairsList();
        }

    }
}
