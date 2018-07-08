using Microsoft.ML.Runtime.Api;

namespace clu.machinelearning.library
{
    public class SentimentAnalysisPredictionModel
    {
        [ColumnName("PredictedLabel")]
        public bool Sentiment;
    }
}
