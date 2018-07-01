using Microsoft.ML.Runtime.Api;

namespace clu.machinelearning.library
{
    public class SentimentAnalysisDataModel
    {
        [Column(ordinal: "0", name: "Label")]
        public float Sentiment;

        [Column(ordinal: "1")]
        public string SentimentText;
    }
}
