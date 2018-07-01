using Newtonsoft.Json;

using System;

namespace clu.machinelearning.library
{
    public class SentimentAnalysisClassificationInput
    {
        /// <summary>
        /// Sentiment analysis identification.
        /// </summary>
        [JsonIgnore]

        public Guid Id { get; set; }

        /// <summary>
        /// Actual text sentiment.
        /// </summary>
        public float ActualSentiment;

        /// <summary>
        /// Text for sentiment analysis.
        /// </summary>
        public string TextForAnalysis { get; set; }
    }
}
