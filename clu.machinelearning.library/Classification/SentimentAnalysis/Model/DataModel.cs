using Microsoft.ML.Runtime.Api;

namespace clu.machinelearning.library.classification.sentimentanalysis
{
    public class DataModel
    {
        [Column(ordinal: "0", name: "Label")]
        public float Sentiment;

        [Column(ordinal: "1")]
        public string SentimentText;
    }
}
