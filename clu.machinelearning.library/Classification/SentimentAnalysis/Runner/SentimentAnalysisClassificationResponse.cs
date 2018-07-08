using System.Collections.Generic;

namespace clu.machinelearning.library
{
    public class SentimentAnalysisClassificationResponse
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public List<SentimentAnalysisClassificationOutput> ClassificationOutput { get; set; }
    }
}
