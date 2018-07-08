using Microsoft.ML.Runtime.Api;

namespace clu.machinelearning.library.classification.sentimentanalysis
{
    public class PredictionModel
    {
        [ColumnName("PredictedLabel")]
        public bool Sentiment;
    }
}
