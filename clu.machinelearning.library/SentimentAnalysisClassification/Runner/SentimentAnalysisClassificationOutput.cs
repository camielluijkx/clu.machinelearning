using Newtonsoft.Json;

using System;

namespace clu.machinelearning.library
{
    public class SentimentAnalysisClassificationOutput
    {
        /// <summary>
        /// Sentiment analysis identification.
        /// </summary>
        [JsonIgnore]
        public Guid Id { get; set; }

        /// <summary>
        /// Predicted text sentiment.
        /// </summary>
        public bool PredictedSentiment { get; set; }
    }
}
